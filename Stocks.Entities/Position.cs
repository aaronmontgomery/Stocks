using System;
using System.Collections.Generic;

namespace Stocks.Entities
{
    public class Position : Models.TdAmeritrade.Account.Position
    {
        public string AccountId { get; set; }
        
        public string Exchange { get; set; }

        public string Symbol { get; set; }

        public Guid Id { get; set; }

        public DateTime Updated { get; set; }

        public new virtual Instrument Instrument { get; set; }

        public virtual SecuritiesAccount SecuritiesAccount { get; set; }

        public Position(StocksDbContext stocksDbContext, Dictionary<string, string> settings, Models.TdAmeritrade.Account.Position position, string accountId)
        {
            Instrument = Instrument.GetInstrumentAsync(stocksDbContext, settings, position.Instrument).Result;
            //Instrument instrument = Instrument.GetInstrumentAsync(stocksDbContext, settings, position.Instrument).Result;
            Exchange = Instrument.Exchange;
            //Exchange = instrument.Exchange;
            Symbol = Instrument.Symbol;
            //Symbol = instrument.Symbol;
            AccountId = accountId;
            ShortQuantity = position.ShortQuantity;
            AveragePrice = position.AveragePrice;
            CurrentDayProfitLoss = position.CurrentDayProfitLoss;
            CurrentDayProfitLossPercentage = position.CurrentDayProfitLossPercentage;
            LongQuantity = position.LongQuantity;
            SettledLongQuantity = position.SettledLongQuantity;
            SettledShortQuantity = position.SettledShortQuantity;
            MarketValue = position.MarketValue;
            Updated = DateTime.UtcNow;
        }

        public Position()
        {

        }
    }
}
