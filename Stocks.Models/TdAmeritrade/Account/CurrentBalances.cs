namespace Stocks.Models.TdAmeritrade.Account
{
    public class CurrentBalances
    {
        public decimal AccruedInterest { get; set; }
        public decimal CashBalance { get; set; }
        public decimal CashReceipts { get; set; }
        public decimal LongOptionMarketValue { get; set; }
        public decimal LiquidationValue { get; set; }
        public decimal LongMarketValue { get; set; }
        public decimal MoneyMarketFund { get; set; }
        public decimal Savings { get; set; }
        public decimal ShortMarketValue { get; set; }
        public decimal PendingDeposits { get; set; }
        public decimal CashAvailableForTrading { get; set; }
        public decimal CashAvailableForWithdrawal { get; set; }
        public decimal CashCall { get; set; }
        public decimal LongNonMarginableMarketValue { get; set; }
        public decimal TotalCash { get; set; }
        public decimal ShortOptionMarketValue { get; set; }
        public decimal BondValue { get; set; }
        public decimal CashDebitCallValue { get; set; }
        public decimal UnsettledCash { get; set; }
    }
}
