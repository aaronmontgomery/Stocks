namespace Stocks.Entities
{
    public class CurrentBalance : Models.TdAmeritrade.Account.CurrentBalance
    {
        public CurrentBalance()
        {

        }

        public CurrentBalance(Models.TdAmeritrade.Account.CurrentBalance currentBalance)
        {
            AccruedInterest = currentBalance.AccruedInterest;
            CashBalance = currentBalance.CashBalance;
            CashReceipts = currentBalance.CashReceipts;
            LongOptionMarketValue = currentBalance.LongOptionMarketValue;
            LiquidationValue = currentBalance.LiquidationValue;
            LongMarketValue = currentBalance.LongMarketValue;
            MoneyMarketFund = currentBalance.MoneyMarketFund;
            Savings = currentBalance.Savings;
            ShortMarketValue = currentBalance.ShortMarketValue;
            PendingDeposits = currentBalance.PendingDeposits;
            CashAvailableForTrading = currentBalance.CashAvailableForTrading;
            CashAvailableForWithdrawal = currentBalance.CashAvailableForWithdrawal;
            CashCall = currentBalance.CashCall;
            LongNonMarginableMarketValue = currentBalance.LongNonMarginableMarketValue;
            TotalCash = currentBalance.TotalCash;
            ShortOptionMarketValue = currentBalance.ShortOptionMarketValue;
            BondValue = currentBalance.BondValue;
            CashDebitCallValue = currentBalance.CashDebitCallValue;
            UnsettledCash = currentBalance.UnsettledCash;
            Updated = System.DateTime.UtcNow;
        }

        //public System.Guid Id { get; set; }
        public string SecuritiesAccountId { get; set; }
        public System.DateTime Updated { get; set; }
        public virtual SecuritiesAccount SecuritiesAccountIdNavigation { get; set; }
    }
}
