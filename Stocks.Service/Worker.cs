using System;
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
            Dictionary<string, IEnumerable<Models.PriceDelta>> accountPriceDeltas;
            while (!stoppingToken.IsCancellationRequested)
            {
                accountPriceDeltas = await Modules.Merchant.GetAccountPriceDeltasAsync();
                _logger.LogInformation("GetAccountPriceDeltasAsync completed: {time}", DateTime.UtcNow);
                await Task.Delay(60000, stoppingToken);
            }
        }
    }
}
