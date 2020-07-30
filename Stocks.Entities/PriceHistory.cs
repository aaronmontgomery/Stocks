using System;

namespace Stocks.Entities
{
    public class PriceHistory
    {
        public Guid Id { get; set; }
        public string Symbol { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public long Volume { get; set; }
        public DateTime DateTime { get; set; }
        public DateTime Updated { get; set; }
        public virtual Ticker SymbolNavigation { get; set; }
    }
}
