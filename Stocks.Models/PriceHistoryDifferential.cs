namespace Stocks.Models
{
    public class PriceHistoryDifferential
    {
        public string Symbol { get; set; }
        public decimal LastPriceClose { get; set; }
        public decimal AveragePriceHigh { get; set; }
        public decimal AveragePriceLow { get; set; }
        public decimal Differential { get; set; }
        public int DaysLapsedHighLow { get; set; }
        public int DaysLapsedLast { get; set; }
    }
}
