using System.Linq;
using System.Text.Json;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Stocks.Entities
{
    public partial class Instrument
    {
        public static async Task<Instrument> GetInstrumentAsync(StocksDbContext stocksDbContext, Dictionary<string, string> settings, Models.TdAmeritrade.Account.Instrument instrument, CancellationToken cancellationToken = default)
        {
            Instrument ins;
            IReadOnlyCollection<Models.TdAmeritrade.Account.Instrument> instruments;
            JsonSerializerOptions jsonSerializerOptions;
            string json;

            jsonSerializerOptions = new()
            {
                PropertyNameCaseInsensitive = true
            };

            try
            {
                ins = stocksDbContext.Instrument.Single(x => x.Symbol == instrument.Symbol && x.Cusip == instrument.Cusip && x.AssetType == instrument.AssetType);
            }

            catch
            {
                json = await Requester.SendRequestAsync(Enums.HttpVerb.Get, $"{settings["InstrumentsUrl"]}/{instrument.Cusip}?apikey={settings["ApiKey"]}", null, cancellationToken);
                instruments = JsonSerializer.Deserialize<IReadOnlyCollection<Models.TdAmeritrade.Account.Instrument>>(json, jsonSerializerOptions);
                ins = new Instrument(instruments.Single());
            }

            return ins;
        }
    }
}
