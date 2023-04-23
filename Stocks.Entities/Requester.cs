using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Stocks.Entities
{
    public static class Requester
    {
        private static IDataAdapter _dataAdapter;

        private static AuthenticationHeaderValue _authenticationHeaderValue;

        private static readonly Stopwatch _stopwatch = new();

        private const long _throttle = 500;

        private static bool _isInitialized;
        public static bool IsInitialized
        {
            get => _isInitialized;
            set => _isInitialized = value;
        }

        private static async Task UpdateAuthenticationAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                _authenticationHeaderValue = null;
                var authorization = await _dataAdapter.UpdateTokenAsync(null, cancellationToken);
                _authenticationHeaderValue = new(authorization.TokenType, authorization.AccessToken);
            }

            catch
            {

            }
        }

        internal static async Task<string> QueueRequestAsync(Enums.HttpVerb httpVerb, string requestUrl, dynamic content, CancellationToken cancellationToken = default)
        {
            HttpClient httpClient;
            HttpResponseMessage httpResponseMessage;
            string json;
            long t;

            try
            {
                _stopwatch.Stop();
                t = _stopwatch.ElapsedMilliseconds - _throttle;
                if (t < 0)
                {
                    await Task.Delay(Convert.ToInt32(Math.Abs(t)), cancellationToken);
                }
                
                using (httpClient = new())
                {
                    using (httpResponseMessage = new())
                    {
                        httpClient.DefaultRequestHeaders.Authorization = _authenticationHeaderValue;

                        switch (httpVerb)
                        {
                            case Enums.HttpVerb.Get:
                                httpResponseMessage = await httpClient.GetAsync(requestUrl, cancellationToken);

                                break;

                            case Enums.HttpVerb.Post:
                                httpResponseMessage = await httpClient.PostAsync(requestUrl, content, cancellationToken);

                                break;

                            case Enums.HttpVerb.Delete:
                                httpResponseMessage = await httpClient.DeleteAsync(requestUrl, cancellationToken);

                                break;

                            default:

                                break;
                        }

                        json = await httpResponseMessage.EnsureSuccessStatusCode().Content.ReadAsStringAsync(cancellationToken);
                    }
                }
            }

            catch
            {
                json = string.Empty;
            }

            finally
            {
                _stopwatch.Restart();
            }

            return json;
        }

        public static async Task<string> SendRequestAsync(Enums.HttpVerb httpVerb, string requestUrl, dynamic content, CancellationToken cancellationToken = default)
        {
            UpdateAuthenticationAsync(cancellationToken).Wait(cancellationToken);
            return await QueueRequestAsync(httpVerb, requestUrl, content, cancellationToken);
        }
        
        public static void Initialize(this IDataAdapter dataAdapter)
        {
            if (!IsInitialized)
            {
                _dataAdapter = dataAdapter;
                UpdateAuthenticationAsync().Wait();
                IsInitialized = true;
            }
        }
    }
}
