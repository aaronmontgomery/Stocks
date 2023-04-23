namespace Stocks.Models.TdAmeritrade.Account
{
    public class Position
    {
        public double ShortQuantity { get; set; }
        public decimal AveragePrice { get; set; }
        public decimal CurrentDayProfitLoss { get; set; }
        public double CurrentDayProfitLossPercentage { get; set; }
        public double LongQuantity { get; set; }
        public double SettledLongQuantity { get; set; }
        public double SettledShortQuantity { get; set; }
        public Instrument Instrument { get; set; }
        public double MarketValue { get; set; }
    }
}
