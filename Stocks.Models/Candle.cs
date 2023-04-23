namespace Stocks.Models
{
    public class Candle
    {
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public long Volume { get; set; }
        public double DateTime { get; set; }
    }
}
