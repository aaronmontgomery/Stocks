using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Stocks.Modules
{
    public static partial class Merchant
    {
        public static async Task<IEnumerable<Models.PriceDelta>> GetPriceDeltasAsync(Models.TdAmeritrade.Account.Account account, IEnumerable<Models.PriceDelta> priceDeltas = null)
        {
            List<Models.PriceDelta> priceDeltasList = priceDeltas == null ? new List<Models.PriceDelta>() : priceDeltas.ToList();
            IEnumerable<Models.TdAmeritrade.Watchlist.Watchlist> accountWatchlists = await TdAmeritrade.Watchlist.GetWatchListsForSingleAccountAsync(account.SecuritiesAccount.AccountId);
            IEnumerable<Models.TdAmeritrade.Account.Instrument> positionInstruments = account.SecuritiesAccount.Positions.Select(x => x.Instrument);
            IEnumerable<Models.TdAmeritrade.Account.Instrument> stocksWatchlistInstruments = accountWatchlists.Single(x => x.AccountId == account.SecuritiesAccount.AccountId && x.Name == "Stocks").WatchlistItems.Select(x => x.Instrument).Where(x => !positionInstruments.Any(y => y.Symbol == x.Symbol));
            IEnumerable<Models.TdAmeritrade.Account.Instrument> instruments = positionInstruments.Concat(stocksWatchlistInstruments);
            List<Models.PriceDelta> priceDeltasToRemove = priceDeltasList.Where(x => x.AccountId == account.SecuritiesAccount.AccountId && !instruments.Any(y => y.Symbol == x.Instrument.Symbol)).ToList();
            foreach (Models.PriceDelta priceDelta in priceDeltasToRemove)
            {
                priceDeltasList.Remove(priceDelta);
            }

            foreach (Models.TdAmeritrade.Account.Instrument instrument in instruments)
            {
                if (priceDeltasList.Any(x => x.AccountId == account.SecuritiesAccount.AccountId && x.Instrument.Symbol == instrument.Symbol))
                {
                    Models.PriceDelta priceDelta = priceDeltasList.Single(x => x.AccountId == account.SecuritiesAccount.AccountId && x.Instrument.Symbol == instrument.Symbol);
                    if (priceDelta.Position == null)
                    {

                    }

                    else
                    {
                        priceDeltasList[priceDeltasList.IndexOf(priceDelta)].Position = account.SecuritiesAccount.Positions.Single(x => x.Instrument.Symbol == instrument.Symbol);
                    }
                }

                else
                {
                    Models.PriceDelta priceDelta = new Models.PriceDelta()
                    {
                        Instrument = instrument,
                        AccountId = account.SecuritiesAccount.AccountId,
                        Position = account.SecuritiesAccount.Positions.SingleOrDefault(x => x.Instrument.Symbol == instrument.Symbol),
                        Quotes = new Queue<Models.TdAmeritrade.Quote.Quote>()
                    };
                    priceDeltasList.Add(priceDelta);
                }
            }

            return priceDeltasList.AsEnumerable();
        }
    }
}
