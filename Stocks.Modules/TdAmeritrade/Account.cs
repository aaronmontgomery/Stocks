using System.Linq;
using System.Text.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Stocks.Modules.TdAmeritrade
{
    public class Account
    {
        static readonly Entities.StocksContext _stocksContext = new Entities.StocksContext();

        static readonly Dictionary<string, string> _settings = new Entities.StocksContext().Setting.ToDictionary(x => x.Key, x => x.Value);

        static readonly HttpClient _httpClient = new HttpClient();

        public static IEnumerable<Models.TdAmeritrade.Account.Account> Update(Entities.Authorization authorization)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(authorization.TokenType, authorization.AccessToken);
            using HttpResponseMessage httpResponseMessage = _httpClient.GetAsync($"{_settings["AccountsUri"]}?fields=positions,orders").Result;
            string json = httpResponseMessage.Content.ReadAsStringAsync().Result;
            IEnumerable<Models.TdAmeritrade.Account.Account> accounts = JsonSerializer.Deserialize<IEnumerable<Models.TdAmeritrade.Account.Account>>(json, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
            });
            foreach (Models.TdAmeritrade.Account.Account account in accounts)
            {
                if (_stocksContext.SecuritiesAccount.Any(x => x.AccountId == account.SecuritiesAccount.AccountId))
                {
                    Entities.SecuritiesAccount securitiesAccount = _stocksContext.SecuritiesAccount.Single(x => x.AccountId == account.SecuritiesAccount.AccountId);
                    IQueryable<Entities.Instrument> instruments = securitiesAccount.Positions.Select(x => x.Instrument).AsQueryable();
                    _stocksContext.RemoveRange(instruments);
                    _stocksContext.RemoveRange(securitiesAccount.Positions);
                    _stocksContext.Remove(securitiesAccount.CurrentBalances);
                    _stocksContext.Remove(securitiesAccount);
                    _stocksContext.SaveChanges();
                    securitiesAccount = new Entities.SecuritiesAccount(account.SecuritiesAccount);
                    _stocksContext.Add(securitiesAccount);
                    _stocksContext.SaveChanges();
                }

                else
                {
                    Entities.SecuritiesAccount securitiesAccount = new Entities.SecuritiesAccount(account.SecuritiesAccount);
                    _stocksContext.Add(securitiesAccount);
                    _stocksContext.SaveChanges();
                }
            }

            return accounts;
        }
    }
}
