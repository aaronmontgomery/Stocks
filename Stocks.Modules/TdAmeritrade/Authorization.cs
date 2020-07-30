using System;
using System.Linq;
using System.Text.Json;
using System.Net.Http;
using System.Collections.Generic;
using Stocks.Models.TdAmeritrade.Authorization;

namespace Stocks.Modules.TdAmeritrade
{
    public static class Authorization
    {
        private static readonly Entities.StocksContext _stocksContext = new Entities.StocksContext();

        private static readonly Dictionary<string, string> _settings = new Entities.StocksContext().Setting.ToDictionary(x => x.Key, x => x.Value);

        private static readonly HttpClient _httpClient = new HttpClient();

        public static Entities.Authorization Update(string code = null)
        {
            Entities.Authorization authorization;
            if (code != null)
            {
                authorization = PostAccessToken(code);
                _stocksContext.Authorization.RemoveRange(_stocksContext.Authorization);
                _stocksContext.SaveChanges();
                _stocksContext.Add(authorization);
                _stocksContext.SaveChanges();
            }

            else
            {
                try
                {
                    authorization = _stocksContext.Authorization.Single();
                    if (!authorization.IsRefreshTokenExpired())
                    {
                        if (authorization.IsAccessTokenExpired())
                        {
                            authorization = PostRefreshToken();
                            _stocksContext.RemoveRange(_stocksContext.Authorization);
                            _stocksContext.SaveChanges();
                            _stocksContext.Add(authorization);
                            _stocksContext.SaveChanges();
                        }
                    }

                    else
                    {
                        throw new TimeoutException("Refresh Token Expired");
                    }
                }

                catch
                {
                    throw new NullReferenceException("Token Does Not Exist");
                }
            }

            return authorization;
        }

        private static Entities.Authorization PostAccessToken(string code)
        {
            Dictionary<string, string> postAccessToken = new Dictionary<string, string>()
            {
                { "grant_type", "authorization_code" },
                { "refresh_token", string.Empty },
                { "access_type", "offline" },
                { "code", code },
                { "client_id", $"{_settings["ApiKey"]}@AMER.OAUTHAP" },
                { "redirect_uri", $"https://localhost:44397/api/authorization/postaccesstoken" }
            };
            using HttpResponseMessage httpResponseMessage = _httpClient.PostAsync(_settings["AuthenticationUri"], new FormUrlEncodedContent(postAccessToken)).Result;
            string data = httpResponseMessage.Content.ReadAsStringAsync().Result;
            AuthorizationCode authorizationCode = JsonSerializer.Deserialize<AuthorizationCode>(data);
            Entities.Authorization authorization = new Entities.Authorization()
            {
                AccessToken = authorizationCode.access_token,
                RefreshToken = authorizationCode.refresh_token,
                TokenType = authorizationCode.token_type,
                ExpiresIn = authorizationCode.expires_in,
                Scope = authorizationCode.scope,
                RefreshTokenExpiresIn = authorizationCode.refresh_token_expires_in,
                Updated = DateTime.UtcNow
            };
            return authorization;
        }

        private static Entities.Authorization PostRefreshToken()
        {
            Entities.Authorization authorization = _stocksContext.Authorization.Single();
            Dictionary<string, string> postAccessToken = new Dictionary<string, string>()
            {
                { "grant_type", "refresh_token" },
                { "refresh_token", authorization.RefreshToken },
                { "access_type", string.Empty },
                { "code", string.Empty },
                { "client_id", $"{_settings["ApiKey"]}@AMER.OAUTHAP" },
                { "redirect_uri", string.Empty }
            };
            using HttpResponseMessage httpResponseMessage = _httpClient.PostAsync(_settings["AuthenticationUri"], new FormUrlEncodedContent(postAccessToken)).Result;
            string data = httpResponseMessage.Content.ReadAsStringAsync().Result;
            RefreshToken refreshToken = JsonSerializer.Deserialize<RefreshToken>(data);
            authorization.AccessToken = refreshToken.access_token;
            authorization.Scope = refreshToken.scope;
            authorization.ExpiresIn = refreshToken.expires_in;
            authorization.TokenType = refreshToken.token_type;
            authorization.Updated = DateTime.UtcNow;
            return authorization;
        }

        private static bool IsAccessTokenExpired(this Entities.Authorization authorization)
        {
            if (authorization.Updated.AddSeconds(authorization.ExpiresIn).CompareTo(DateTime.UtcNow.AddSeconds(5)) < 0)
            {
                return true;
            }

            else
            {
                return false;
            }
        }

        private static bool IsRefreshTokenExpired(this Entities.Authorization authorization)
        {
            if (authorization.Updated.AddSeconds(authorization.RefreshTokenExpiresIn).CompareTo(DateTime.UtcNow.AddSeconds(5)) < 0)
            {
                return true;
            }

            else
            {
                return false;
            }
        }
    }
}
