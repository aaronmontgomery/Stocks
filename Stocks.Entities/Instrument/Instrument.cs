using System;
using System.Collections.Generic;

namespace Stocks.Entities
{
    public partial class Instrument : Models.TdAmeritrade.Account.Instrument
    {
        public Guid Id { get; set; }

        public DateTime Created { get; set; }

        public DateTime Updated { get; set; }

        public virtual ICollection<Position> Positions { get; set; }

        public virtual ICollection<PriceHistory> PriceHistories { get; set; }

        public Instrument(Models.TdAmeritrade.Account.Instrument instrument)
        {
            AssetType = instrument.AssetType;
            Description = instrument.Description;
            Cusip = instrument.Cusip;
            Exchange = instrument.Exchange;
            Symbol = instrument.Symbol;
            Created = DateTime.UtcNow;
            Updated = DateTime.UtcNow;
        }
        
        public Instrument()
        {
            PriceHistories = new HashSet<PriceHistory>();
        }
    }
}
