namespace Stocks.Models.TdAmeritrade.Order
{
    public class OrderLeg
    {
        public ulong LegId { get; set; }
        public string OrderLegType { get; set; } //'EQUITY' or 'OPTION' or 'INDEX' or 'MUTUAL_FUND' or 'CASH_EQUIVALENT' or 'FIXED_INCOME' or 'CURRENCY'
        public Account.Instrument Instrument { get; set; }
        public string Instruction { get; set; } //'BUY' or 'SELL' or 'BUY_TO_COVER' or 'SELL_SHORT' or 'BUY_TO_OPEN' or 'BUY_TO_CLOSE' or 'SELL_TO_OPEN' or 'SELL_TO_CLOSE' or 'EXCHANGE'
        public string PositionEffect { get; set; } //'OPENING' or 'CLOSING' or 'AUTOMATIC'
        public double Quantity { get; set; }
        public string QuantityType { get; set; } //'ALL_SHARES' or 'DOLLARS' or 'SHARES'
    }
}
