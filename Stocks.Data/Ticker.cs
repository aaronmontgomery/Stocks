using System.Linq;
using System.Collections;
using Microsoft.EntityFrameworkCore;

namespace Stocks.Data
{
    public static partial class CompiledQueries
    {
        public static IEnumerable Ticker()
        {
            Entities.StocksContext stocksContext = new Entities.StocksContext();
            IEnumerable ticker = stocksContext.Ticker.Select(x => new
            {
                x.Symbol,
                x.Name,
                x.Sector,
                x.Industry,
                x.IpoYear,
                x.MarketCap,
                LastSale = string.Format("${0}", x.LastSale),
                Updated = x.Updated.ToShortDateString()
            }).AsNoTracking();
            return ticker;
        }
    }
}
