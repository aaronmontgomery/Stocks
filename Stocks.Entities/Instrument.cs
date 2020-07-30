namespace Stocks.Entities
{
    public class Instrument : Models.TdAmeritrade.Account.Instrument
    {
        public Instrument()
        {

        }

        public Instrument(Models.TdAmeritrade.Account.Instrument instrument)
        {
            AssetType = instrument.AssetType;
            Cusip = instrument.Cusip;
            Symbol = instrument.Symbol;
            Updated = System.DateTime.UtcNow;
        }

        public System.Guid Id { get; set; }
        public System.Guid PositionId { get; set; }
        public System.DateTime Updated { get; set; }
        public virtual Position PositionIdNavigation { get; set; }
    }
}
