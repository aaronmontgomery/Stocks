using System;
using System.Linq;
using System.Collections.Generic;
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
            Dictionary<string, IEnumerable<Models.PriceDelta>> accountPriceDeltas = new Dictionary<string, IEnumerable<Models.PriceDelta>>();
            while (!stoppingToken.IsCancellationRequested)
            {
                Entities.Authorization authorization = Modules.TdAmeritrade.Authorization.Update();
                IEnumerable<Models.TdAmeritrade.Account.Account> accounts = Modules.TdAmeritrade.Account.Update(authorization);
                var accountPriceDeltasToRemove = accountPriceDeltas.Where(x => !accounts.Select(x => x.SecuritiesAccount.AccountId).Contains(x.Key));
                foreach (var accountPriceDeltaToRemove in accountPriceDeltasToRemove)
                {
                    accountPriceDeltas.Remove(accountPriceDeltaToRemove.Key);
                }

                foreach (Models.TdAmeritrade.Account.Account account in accounts)
                {
                    IEnumerable<Models.PriceDelta> priceDeltas = await Modules.Merchant.GetPriceDeltasAsync(account);
                    _logger.LogInformation("{accountId} GetPriceDeltasAsync completed: {time}", account.SecuritiesAccount.AccountId, DateTime.UtcNow);
                    priceDeltas = await Modules.Merchant.GetQuotesAsync(priceDeltas);
                    _logger.LogInformation("{accountId} GetQuotesAsync completed: {time}", account.SecuritiesAccount.AccountId, DateTime.UtcNow);
                    if (accountPriceDeltas.ContainsKey(account.SecuritiesAccount.AccountId))
                    {
                        accountPriceDeltas[account.SecuritiesAccount.AccountId] = priceDeltas; 
                    }

                    else
                    {
                        accountPriceDeltas.Add(account.SecuritiesAccount.AccountId, priceDeltas);
                    }
                }

                await Task.Delay(60000, stoppingToken);
            }
        }
    }
}
