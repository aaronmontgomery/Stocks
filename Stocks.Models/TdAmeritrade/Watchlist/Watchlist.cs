namespace Stocks.Models.TdAmeritrade.Watchlist
{
    public class Watchlist
    {
        public string Name { get; set; }
        public string WatchlistId { get; set; }
        public string AccountId { get; set; }
        public WatchlistItem[] WatchlistItems { get; set; }
    }
}
