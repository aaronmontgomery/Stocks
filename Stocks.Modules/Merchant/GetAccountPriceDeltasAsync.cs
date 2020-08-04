using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Stocks.Modules
{
    public partial class Merchant
    {
        public static async Task<Dictionary<string, IEnumerable<Models.PriceDelta>>> GetAccountPriceDeltasAsync()
        {
            Dictionary<string, IEnumerable<Models.PriceDelta>> accountPriceDeltas = new Dictionary<string, IEnumerable<Models.PriceDelta>>();
            Entities.Authorization authorization = TdAmeritrade.Authorization.Update();
            IEnumerable<Models.TdAmeritrade.Account.Account> accounts = TdAmeritrade.Account.Update(authorization);
            if (!accountPriceDeltas.All(x => accounts.Select(x => x.SecuritiesAccount.AccountId).Contains(x.Key)))
            {
                var accountPriceDeltasToRemove = accountPriceDeltas.Where(x => !accounts.Select(x => x.SecuritiesAccount.AccountId).Contains(x.Key));
                foreach (var accountPriceDelta in accountPriceDeltasToRemove)
                {
                    accountPriceDeltas.Remove(accountPriceDelta.Key);
                }
            }

            else
            {
                foreach (Models.TdAmeritrade.Account.Account account in accounts)
                {
                    IEnumerable<Models.PriceDelta> priceDeltas = await GetPriceDeltasAsync(account);
                    priceDeltas = await GetQuotesAsync(priceDeltas);
                    if (accountPriceDeltas.ContainsKey(account.SecuritiesAccount.AccountId))
                    {
                        accountPriceDeltas[account.SecuritiesAccount.AccountId] = priceDeltas;
                    }

                    else
                    {
                        accountPriceDeltas.Add(account.SecuritiesAccount.AccountId, priceDeltas);
                    }
                }
            }

            return accountPriceDeltas;
        }
    }
}
