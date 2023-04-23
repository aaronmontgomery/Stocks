namespace Stocks.Models
{
    public class StockCandles
    {
        public Candle[] Candles { get; set; }

        public bool Empty { get; set; }

        public string Symbol { get; set; }
    }
}
