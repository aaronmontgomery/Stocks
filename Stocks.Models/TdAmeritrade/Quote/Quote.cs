namespace Stocks.Models.TdAmeritrade.Quote
{
    public class Quote
    {
        public string AssetType { get; set; }
        public string Cusip { get; set; }
        public string Symbol { get; set; }
        public string Description { get; set; }
        public decimal BidPrice { get; set; }
        public uint BidSize { get; set; }
        public string BidId { get; set; }
        public decimal AskPrice { get; set; }
        public uint AskSize { get; set; }
        public string AskId { get; set; }
        public decimal LastPrice { get; set; }
        public uint LastSize { get; set; }
        public string LastId { get; set; }
        public decimal OpenPrice { get; set; }
        public decimal HighPrice { get; set; }
        public decimal LowPrice { get; set; }
        public string BidTick { get; set; }
        public decimal ClosePrice { get; set; }
        public decimal NetChange { get; set; }
        public uint TotalVolume { get; set; }
        public ulong QuoteTimeInLong { get; set; }
        public ulong TradeTimeInLong { get; set; }
        public decimal Mark { get; set; }
        public string Exchange { get; set; }
        public string ExchangeName { get; set; }
        public bool Marginable { get; set; }
        public bool Shortable { get; set; }
        public decimal Volatility { get; set; }
        public uint Digits { get; set; }
        //public decimal 52WkHigh { get; set; }
        //public decimal 52WkLow { get; set; }
        public double NAV { get; set; }
        public double PeRatio { get; set; }
        public decimal DivAmount { get; set; }
        public double DivYield { get; set; }
        public string DivDate { get; set; }
        public string SecurityStatus { get; set; }
        public decimal RegularMarketLastPrice { get; set; }
        public uint RegularMarketLastSize { get; set; }
        public decimal RegularMarketNetChange { get; set; }
        public ulong RegularMarketTradeTimeInLong { get; set; }
        public double NetPercentChangeInDouble { get; set; }
        public double MarkChangeInDouble { get; set; }
        public double MarkPercentChangeInDouble { get; set; }
        public double RegularMarketPercentChangeInDouble { get; set; }
        public bool Delayed { get; set; }
    }
}
