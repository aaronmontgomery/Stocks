namespace Stocks.Models.TdAmeritrade.Watchlist
{
    public class WatchlistItem
    {
        public uint SequenceId { get; set; }
        public double Quantity { get; set; }
        public decimal AveragePrice { get; set; }
        public decimal Commission { get; set; }
        public Account.Instrument Instrument { get; set; }
    }
}
