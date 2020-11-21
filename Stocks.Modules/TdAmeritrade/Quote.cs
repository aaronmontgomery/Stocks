using System.Linq;
using System.Text.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Stocks.Modules.TdAmeritrade
{
    public class Quote
    {
        private static readonly Dictionary<string, string> _settings = new Entities.StocksContext().Setting.ToDictionary(x => x.Key, x => x.Value);

        private static readonly HttpClient _httpClient = new HttpClient();

        public static async Task<Models.TdAmeritrade.Quote.Quote> GetQuoteAsync(string symbol)
        {
            string quoteUri = $"{_settings["MarketDataUri"]}/{WebUtility.UrlEncode(symbol)}/quotes?apikey={_settings["ApiKey"]}";
            Entities.Authorization authorization = Authorization.Update();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(authorization.TokenType, authorization.AccessToken);
            using HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync(quoteUri);
            string json = await httpResponseMessage.Content.ReadAsStringAsync();
            IEnumerable<Models.TdAmeritrade.Quote.Quote> quote = JsonSerializer.Deserialize<IEnumerable<Models.TdAmeritrade.Quote.Quote>>(Toolbox.Json.RemoveTopLevelKeys(json), new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            });

            try
            {
                Models.TdAmeritrade.Quote.Quote quoteRtn = quote.Single();
            }

            catch (System.Exception exception)
            {

            }

            return quote.Single();
        }

        public static async Task<IEnumerable<Models.TdAmeritrade.Quote.Quote>> GetQuotesAsync(IEnumerable<string> symbols)
        {
            string quoteUri = $"{_settings["MarketDataUri"]}/quotes?apikey={_settings["ApiKey"]}&symbol={WebUtility.UrlEncode(string.Join(',', symbols))}";
            Entities.Authorization authorization = Authorization.Update();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(authorization.TokenType, authorization.AccessToken);
            using HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync(quoteUri);
            string json = await httpResponseMessage.Content.ReadAsStringAsync();
            IEnumerable<Models.TdAmeritrade.Quote.Quote> quotes = JsonSerializer.Deserialize<IEnumerable<Models.TdAmeritrade.Quote.Quote>>(Toolbox.Json.RemoveTopLevelKeys(json), new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            });
            return quotes;
        }
    }
}
