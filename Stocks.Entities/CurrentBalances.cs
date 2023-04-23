using System;

namespace Stocks.Entities
{
    public class CurrentBalances : Models.TdAmeritrade.Account.CurrentBalances
    {
        public string AccountId { get; set; }

        public DateTime Updated { get; set; }

        public Guid Id { get; set; }

        public virtual SecuritiesAccount SecuritiesAccount { get; set; }
        
        public CurrentBalances(Models.TdAmeritrade.Account.CurrentBalances currentBalances, string accountId)
        {
            AccountId = accountId;
            AccruedInterest = currentBalances.AccruedInterest;
            CashBalance = currentBalances.CashBalance;
            CashReceipts = currentBalances.CashReceipts;
            LongOptionMarketValue = currentBalances.LongOptionMarketValue;
            LiquidationValue = currentBalances.LiquidationValue;
            LongMarketValue = currentBalances.LongMarketValue;
            MoneyMarketFund = currentBalances.MoneyMarketFund;
            Savings = currentBalances.Savings;
            ShortMarketValue = currentBalances.ShortMarketValue;
            PendingDeposits = currentBalances.PendingDeposits;
            CashAvailableForTrading = currentBalances.CashAvailableForTrading;
            CashAvailableForWithdrawal = currentBalances.CashAvailableForWithdrawal;
            CashCall = currentBalances.CashCall;
            LongNonMarginableMarketValue = currentBalances.LongNonMarginableMarketValue;
            TotalCash = currentBalances.TotalCash;
            ShortOptionMarketValue = currentBalances.ShortOptionMarketValue;
            BondValue = currentBalances.BondValue;
            CashDebitCallValue = currentBalances.CashDebitCallValue;
            UnsettledCash = currentBalances.UnsettledCash;
            Updated = DateTime.UtcNow;
        }

        public CurrentBalances()
        {

        }
    }
}
