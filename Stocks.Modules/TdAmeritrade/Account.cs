using System;
using System.Text.Json;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Stocks.Entities;

namespace Stocks.Modules.TdAmeritrade
{
    public class Account : IAccount
    {
        private readonly IDataAdapter _dataAdapater;

        public Account(IDataAdapter dataAdapter)
        {
            _dataAdapater = dataAdapter;
        }

        public async Task<IAsyncEnumerable<Models.TdAmeritrade.Account.Account>> UpdateAccountAsync(CancellationToken cancellationToken = default)
        {
            IAsyncEnumerable<Models.TdAmeritrade.Account.Account> accounts;
            SecuritiesAccount securitiesAccount;

            _dataAdapater.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
            _dataAdapater.Json = await Requester.SendRequestAsync(Enums.HttpVerb.Get, $"{_dataAdapater.Settings["AccountsUrl"]}?fields=positions,orders", null, cancellationToken);
            accounts = JsonSerializer.Deserialize<IAsyncEnumerable<Models.TdAmeritrade.Account.Account>>(_dataAdapater.Json, _dataAdapater.JsonSerializerOptions);
            
            await foreach (Models.TdAmeritrade.Account.Account account in accounts)
            {
                try
                {
                    using StocksDbContext stocksDbContext = await _dataAdapater.StocksDbContextFactory.CreateDbContextAsync(cancellationToken);
                    securitiesAccount = await stocksDbContext.FindAsync<SecuritiesAccount>(new object[] { account.SecuritiesAccount.AccountId }, cancellationToken);
                    if (securitiesAccount == null)
                    {
                        securitiesAccount = new SecuritiesAccount(stocksDbContext, _dataAdapater.Settings, account.SecuritiesAccount);
                        await stocksDbContext.AddAsync(securitiesAccount, cancellationToken);
                    }

                    else
                    {
                        // update existing securitiesAccount

                    }
                    
                    await stocksDbContext.SaveChangesAsync(cancellationToken);
                }

                catch
                {

                }
            }

            //await StocksDbContext.SaveChangesAsync(cancellationToken);
            return accounts;
        }
    }
}
