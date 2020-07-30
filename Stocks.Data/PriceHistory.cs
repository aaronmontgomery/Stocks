using System.Linq;
using System.Collections;
using Microsoft.EntityFrameworkCore;
using Stocks.Models;

namespace Stocks.Data
{
    public static partial class CompiledQueries
    {
        public static IEnumerable PriceHistory(string symbol, DateTimeRange dateTimeRange)
        {
            Entities.StocksContext stocksContext = new Entities.StocksContext();
            IEnumerable priceHistory = stocksContext.PriceHistory.Where(x => x.Symbol == symbol).Where(x => x.DateTime.Date >= dateTimeRange.Start.Date && x.DateTime.Date <= dateTimeRange.End.Date).Select(x => new
            {
                Date = x.DateTime.ToShortDateString(),
                x.DateTime,
                x.Low,
                x.Open,
                x.High,
                x.Close
            }).AsNoTracking();
            return priceHistory;
        }
    }
}
