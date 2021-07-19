namespace Stocks.Entities
{
    public class Position : Models.TdAmeritrade.Account.Position
    {
        public Position()
        {

        }

        public Position(Models.TdAmeritrade.Account.Position position)
        {
            ShortQuantity = position.ShortQuantity;
            AveragePrice = position.AveragePrice;
            CurrentDayProfitLoss = position.CurrentDayProfitLoss;
            CurrentDayProfitLossPercentage = position.CurrentDayProfitLossPercentage;
            LongQuantity = position.LongQuantity;
            SettledLongQuantity = position.SettledLongQuantity;
            SettledShortQuantity = position.SettledShortQuantity;
            Instrument = new Instrument(position.Instrument);
            MarketValue = position.MarketValue;
            Updated = System.DateTime.UtcNow;
        }

        public System.Guid Id { get; set; }
        public string SecuritiesAccountId { get; set; }
        public new virtual Instrument Instrument { get; set; }
        public System.DateTime Updated { get; set; }
        public virtual SecuritiesAccount SecuritiesAccountIdNavigation { get; set; }
    }
}
