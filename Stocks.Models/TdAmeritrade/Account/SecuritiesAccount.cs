namespace Stocks.Models.TdAmeritrade.Account
{
    public class SecuritiesAccount
    {
        public string AccountId { get; set; }
        public string Type { get; set; }
        public int RoundTrips { get; set; }
        public bool IsDayTrader { get; set; }
        public bool IsClosingOnlyRestricted { get; set; }
        public Position[] Positions { get; set; }
        public CurrentBalances CurrentBalances { get; set; }
    }
}
