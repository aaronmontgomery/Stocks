using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Stocks.Web.Hubs;

namespace Stocks.Web.Services
{
    // empty background service for testing
    public class StocksWorker : BackgroundService
    {
        private readonly ILogger<StocksWorker> _logger;
        private readonly IHubContext<StocksHub> _stocksHub;

        public StocksWorker(ILogger<StocksWorker> logger, IHubContext<StocksHub> stocksHub)
        {
            _logger = logger;
            _stocksHub = stocksHub;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {Time}", DateTime.Now);
                await Task.Delay(1000);
            }
        }
    }
}
