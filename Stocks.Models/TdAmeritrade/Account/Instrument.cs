namespace Stocks.Models.TdAmeritrade.Account
{
    public class Instrument
    {
        public string AssetType { get; set; } //'EQUITY' or 'OPTION' or 'INDEX' or 'MUTUAL_FUND' or 'CASH_EQUIVALENT' or 'FIXED_INCOME' or 'CURRENCY'

        public string Description { get; set; }

        public string Cusip { get; set; }

        public string Exchange { get; set; }

        public string Symbol { get; set; }
    }
}
