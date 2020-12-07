using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Stocks.Service
{
    public class Test : BackgroundService
    {
        readonly ILogger<Worker> _logger;

        public Test(ILogger<Worker> logger)
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
            bool orderPlaced = false;

            Entities.StocksContext stocksContext = new Entities.StocksContext();
            Models.TdAmeritrade.Account.Account account = new Models.TdAmeritrade.Account.Account()
            {
                SecuritiesAccount = stocksContext.SecuritiesAccount.Single()
            };

            Models.TdAmeritrade.Quote.Quote quote = await Modules.TdAmeritrade.Quote.GetQuoteAsync("BLSP");

            Models.TdAmeritrade.Account.Instrument instrument = new Models.TdAmeritrade.Account.Instrument()
            {
                AssetType = "EQUITY",
                Cusip = quote.Cusip,
                Symbol = quote.Symbol
            };

            Models.TdAmeritrade.Order.OrderLeg orderLeg = new Models.TdAmeritrade.Order.OrderLeg()
            {
                OrderLegType = "EQUITY",
                Instrument = instrument,
                Instruction = "BUY",
                PositionEffect = "AUTOMATIC",
                Quantity = 100000,
                QuantityType = "SHARES"
            };

            Models.TdAmeritrade.Order.Limit limit = new Models.TdAmeritrade.Order.Limit()
            {
                OrderType = "LIMIT",
                Session = "NORMAL",
                Duration = "DAY",
                OrderStrategyType = "NONE",
                OrderLegCollection = new Models.TdAmeritrade.Order.OrderLeg[] { orderLeg },
                Price = new decimal(0.0005)
            };

            while (!stoppingToken.IsCancellationRequested)
            {
                TimeSpan time = DateTime.Now.TimeOfDay;
                _logger.LogInformation($"{time.Hours}:{time.Minutes}:{time.Seconds}:{time.Milliseconds}");
                if (time.Hours == 9 && time.Minutes > 30 && !orderPlaced)
                {
                    orderPlaced = true;
                    string content = await Modules.TdAmeritrade.Order.PlaceOrder(account, limit);
                }
            }
        }
    }
}
