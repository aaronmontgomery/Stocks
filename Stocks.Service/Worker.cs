using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Stocks.Service
{
    public class Worker : BackgroundService
    {
        readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        public override async Task StartAsync(CancellationToken stoppingToken)
        {
            await base.StartAsync(stoppingToken);
            _logger.LogInformation("started: {time}", DateTime.UtcNow);
            await Task.CompletedTask;
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            await base.StopAsync(stoppingToken);
            _logger.LogInformation("stopped: {time}", DateTime.UtcNow);
            await Task.CompletedTask;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Dictionary<Models.TdAmeritrade.Account.Account, IEnumerable<Models.PriceDelta>> accountPriceDeltas = new Dictionary<Models.TdAmeritrade.Account.Account, IEnumerable<Models.PriceDelta>>();
            while (!stoppingToken.IsCancellationRequested)
            {
                Entities.Authorization authorization = Modules.TdAmeritrade.Authorization.Update();
                IEnumerable<Models.TdAmeritrade.Account.Account> accounts = Modules.TdAmeritrade.Account.Update(authorization);

                // if account in accountPriceDeltas and not in accounts then remove from accountPriceDeltas
                var accountPriceDeltasToRemove = accountPriceDeltas.Where(x => !accounts.Select(x => x.SecuritiesAccount.AccountId).Contains(x.Key.SecuritiesAccount.AccountId));
                foreach (var accountPriceDeltaToRemove in accountPriceDeltasToRemove)
                {
                    accountPriceDeltas.Remove(accountPriceDeltaToRemove.Key);
                }

                // if account in accounts and not in accountPriceDeltas then add to accountPriceDeltas
                var accountsToAdd = accounts.Where(x => !accountPriceDeltas.Select(x => x.Key.SecuritiesAccount.AccountId).Contains(x.SecuritiesAccount.AccountId));

                // update existing accountPriceDeltas and add new accountPriceDeltas
                foreach (Models.TdAmeritrade.Account.Account account in accounts)
                {
                    IEnumerable<Models.PriceDelta> priceDeltas = await Modules.Merchant.GetPriceDeltasAsync(account);
                    _logger.LogInformation("GetPriceDeltasAsync completed: {time}", DateTime.UtcNow);
                    priceDeltas = await Modules.Merchant.GetQuotesAsync(priceDeltas);
                    _logger.LogInformation("GetQuotesAsync completed: {time}", DateTime.UtcNow);
                    accountPriceDeltas.Add(account, priceDeltas);
                }

                await Task.Delay(60000, stoppingToken);
            }
        }
    }
}
