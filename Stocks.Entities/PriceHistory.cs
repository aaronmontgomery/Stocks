namespace Stocks.Entities
{
    public class PriceHistory
    {
        public string Exchange { get; set; }

        public string Symbol { get; set; }

        public decimal Open { get; set; }

        public decimal High { get; set; }

        public decimal Low { get; set; }

        public decimal Close { get; set; }

        public long Volume { get; set; }

        public System.DateTime DateTime { get; set; }

        public System.DateTime Updated { get; set; }

        public System.Guid Id { get; set; }

        public virtual Instrument Instrument { get; set; }
    }
}
