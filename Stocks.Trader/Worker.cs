using System;
using System.Collections.Generic;
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

        public override async Task StartAsync(CancellationToken stoppingToken)
        {
            await base.StartAsync(stoppingToken);
            _logger.LogInformation("started: {time}", DateTime.UtcNow);
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            await base.StopAsync(stoppingToken);
            _logger.LogInformation("stopped: {time}", DateTime.UtcNow);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            IEnumerable<Models.PriceDelta> priceDeltas = null;
            while (!stoppingToken.IsCancellationRequested)
            {
                priceDeltas = await Merchant.GetPriceDeltasAsync(priceDeltas);
                _logger.LogInformation("GetPriceDeltasAsync completed: {time}", DateTime.UtcNow);
                priceDeltas = await Merchant.GetQuotesAsync(priceDeltas);
                _logger.LogInformation("GetQuotesAsync completed: {time}", DateTime.UtcNow);
                await Task.Delay(60000, stoppingToken);
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
