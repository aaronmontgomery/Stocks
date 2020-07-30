using System;
using System.Collections.Generic;

namespace Stocks.Entities
{
    public class Ticker
    {
        public Ticker()
        {
            PriceHistory = new HashSet<PriceHistory>();
        }

        public string Symbol { get; set; }
        public string Name { get; set; }
        public string LastSale { get; set; }
        public string MarketCap { get; set; }
        public string IpoYear { get; set; }
        public string Sector { get; set; }
        public string Industry { get; set; }
        public string SummaryQuote { get; set; }
        public DateTime Updated { get; set; }
        public virtual ICollection<PriceHistory> PriceHistory { get; set; }
    }
}
