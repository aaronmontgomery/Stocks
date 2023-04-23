using System;
using System.Linq;
using System.Text.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Stocks.Models.TdAmeritrade.Authorization;

namespace Stocks.Entities
{
    public static class Authenticator
    {
        private static Authorization _authorization;
        
        private static Dictionary<string, string> _postAccessTokenParameters;

        private static AuthorizationCode _authorizationCode;

        private static RefreshToken _refreshToken;

        private static string _json;

        private static bool IsAccessTokenExpired(this Authorization authorization)
        {
            if (authorization.Updated.AddSeconds(authorization.ExpiresIn).CompareTo(DateTime.UtcNow.AddSeconds(10)) < 0)
            {
                return true;
            }

            else
            {
                return false;
            }
        }

        private static bool IsRefreshTokenExpired(this Authorization authorization)
        {
            if (authorization.Updated.AddSeconds(authorization.RefreshTokenExpiresIn).CompareTo(DateTime.UtcNow.AddSeconds(10)) < 0)
            {
                return true;
            }
            
            else
            {
                return false;
            }
        }

        private async static Task<Authorization> PostAccessTokenAsync(IDataAdapter dataAdapter, string code, CancellationToken cancellationToken = default)
        {
            _postAccessTokenParameters = new Dictionary<string, string>()
            {
                { "grant_type", "authorization_code" },
                { "refresh_token", string.Empty },
                { "access_type", "offline" },
                { "code", code },
                { "client_id", $"{dataAdapter.Settings["ApiKey"]}@AMER.OAUTHAP" },
                { "redirect_uri", $"{dataAdapter.Settings["BaseUrl"]}/api/authorization/postaccesstoken" }
            };
            _json = await Requester.QueueRequestAsync(Enums.HttpVerb.Post, dataAdapter.Settings["AuthenticationUrl"], new FormUrlEncodedContent(_postAccessTokenParameters), cancellationToken);
            _authorizationCode = JsonSerializer.Deserialize<AuthorizationCode>(_json);
            _authorization = new Authorization()
            {
                AccessToken = _authorizationCode.AccessToken,
                RefreshToken = _authorizationCode.RefreshToken,
                TokenType = _authorizationCode.TokenType,
                ExpiresIn = _authorizationCode.ExpiresIn,
                Scope = _authorizationCode.Scope,
                RefreshTokenExpiresIn = _authorizationCode.RefreshTokenExpiresIn,
                Updated = DateTime.UtcNow
            };
            return _authorization;
        }

        private async static Task<Authorization> PostRefreshTokenAsync(IDataAdapter dataAdapter, CancellationToken cancellationToken = default)
        {
            _postAccessTokenParameters = new()
            {
                { "grant_type", "refresh_token" },
                { "refresh_token", _authorization.RefreshToken },
                { "access_type", string.Empty },
                { "code", string.Empty },
                { "client_id", $"{dataAdapter.Settings["ApiKey"]}@AMER.OAUTHAP" },
                { "redirect_uri", string.Empty }
            };
            _json = await Requester.QueueRequestAsync(Enums.HttpVerb.Post, dataAdapter.Settings["AuthenticationUrl"], new FormUrlEncodedContent(_postAccessTokenParameters), cancellationToken);
            _refreshToken = JsonSerializer.Deserialize<RefreshToken>(_json);
            _authorization.AccessToken = _refreshToken.AccessToken;
            _authorization.Scope = _refreshToken.Scope;
            _authorization.ExpiresIn = _refreshToken.ExpiresIn;
            _authorization.TokenType = _refreshToken.TokenType;
            _authorization.Updated = DateTime.UtcNow;
            return _authorization;
        }

        public async static Task<Authorization> UpdateTokenAsync(this IDataAdapter dataAdapter, string code, CancellationToken cancellationToken = default)
        {
            using (var stocksDbContext = await dataAdapter.StocksDbContextFactory.CreateDbContextAsync(cancellationToken))
            {
                if (code == null)
                {
                    try
                    {
                        _authorization = stocksDbContext.Authorization.Single();
                        if (!_authorization.IsRefreshTokenExpired())
                        {
                            if (_authorization.IsAccessTokenExpired())
                            {
                                await PostRefreshTokenAsync(dataAdapter, cancellationToken);
                                stocksDbContext.Update(_authorization);
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

                else
                {
                    _authorization = await PostAccessTokenAsync(dataAdapter, code, cancellationToken);
                    stocksDbContext.Update(_authorization);
                }

                await stocksDbContext.SaveChangesAsync(cancellationToken);
            }

            return _authorization;
        }
    }
}
