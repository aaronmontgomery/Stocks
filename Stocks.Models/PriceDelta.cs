namespace Stocks.Models
{
    public class PriceDelta
    {
        public TdAmeritrade.Account.Instrument Instrument { get; set; }
        public string AccountId { get; set; }
        public TdAmeritrade.Account.Position Position { get; set; }
        public decimal PositionDelta { get; set; }
        public decimal PositionDeltaPercent { get; set; }
        public decimal QuoteDelta { get; set; }
        public decimal QuoteDeltaPercent { get; set; }
        public decimal StopPrice { get; set; }
        public sbyte DeltaIndex { get; set; }
        public System.Collections.Generic.Queue<TdAmeritrade.Quote.Quote> Quotes { get; set; }
    }
}
