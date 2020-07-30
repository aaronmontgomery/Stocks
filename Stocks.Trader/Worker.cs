using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Stocks.Trader
{
    public class Worker : BackgroundService
    {
        readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTime.UtcNow);
                await Merchant.RunAsync(stoppingToken);
                /*
                Models.TdAmeritrade.Account.Instrument instrument = new Models.TdAmeritrade.Account.Instrument()
                {
                    Symbol = position.Instrument.Symbol,
                    AssetType = position.Instrument.AssetType
                };

                Models.TdAmeritrade.Order.OrderLeg orderLeg = new Models.TdAmeritrade.Order.OrderLeg()
                {
                    Instruction = "SELL",
                    Quantity = position.LongQuantity,
                    Instrument = instrument
                };

                Models.TdAmeritrade.Order.Limit order = new Models.TdAmeritrade.Order.Limit()
                {
                    OrderType = "LIMIT",
                    Session = "NORMAL",
                    Duration = "DAY",
                    OrderStrategyType = "SINGLE",
                    OrderLegCollection = new Models.TdAmeritrade.Order.OrderLeg[] { orderLeg },
                    Price = price
                };
                */
                //Modules.TdAmeritrade.Order.PlaceOrder(account, order);
            }
        }
    }
}
