using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Stocks.Data;
using Stocks.Models;

namespace Stocks.Web.Hubs
{
    public class StocksHub : Hub
    {
        private readonly ILogger<StocksHub> _logger;

        public StocksHub(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<StocksHub>();
        }

        public async Task GetPriceHistoryAsync(object tickerTableSelectedRowData, object priceHistoryDateRange)
        {
            Entities.Ticker ticker = JsonConvert.DeserializeObject<Entities.Ticker>(tickerTableSelectedRowData.ToString());
            _logger.LogInformation($"GetPriceHistoryAsync {ticker.Symbol} {Context.ConnectionId}");
            DateTimeRange dateTimeRange = JsonConvert.DeserializeObject<DateTimeRange>(priceHistoryDateRange.ToString());
            await Clients.Caller.SendCoreAsync("drawPriceHistory", new object[]
            {
                CompiledQueries.PriceHistory(ticker.Symbol, dateTimeRange),
                ticker
            });
        }

        public async Task GetPriceHistoryDifferentialAsync(object analyticsDateRange)
        {
            _logger.LogInformation($"GetPriceHistoryDifferentialAsync {Context.ConnectionId}");
            using Entities.StocksContext stocksContext = new Entities.StocksContext();
            DateTimeRange dateTimeRange = JsonConvert.DeserializeObject<DateTimeRange>(analyticsDateRange.ToString());
            // debugging hub methods not running concurrently
            // Task.Delay proves the concurrency issue is not with the PriceHistoryDifferential query
            //await Task.Delay(5000);
            await Clients.Caller.SendCoreAsync("drawPriceHistoryDifferential", new object[]
            {
                JsonConvert.SerializeObject(CompiledQueries.PriceHistoryDifferential(dateTimeRange))
            });
        }
    }
}
