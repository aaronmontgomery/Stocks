using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Stocks.Trader
{
    public static class Merchant
    {
        public static async Task RunAsync(CancellationToken stoppingToken)
        {
            IEnumerable<Models.PriceDelta> priceDeltas = null;
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    priceDeltas = await GetInstrumentsAsync(priceDeltas);
                    foreach (Models.PriceDelta priceDelta in priceDeltas)
                    {
                        Models.TdAmeritrade.Quote.Quote quote = await Modules.TdAmeritrade.Quote.GetQuoteAsync(priceDelta.Instrument.Symbol);
                        priceDelta.Quotes.Enqueue(quote);
                        if (priceDelta.Quotes.Count > 1)
                        {
                            Models.TdAmeritrade.Quote.Quote previousQuote = priceDelta.Quotes.Dequeue();
                            priceDelta.QuoteDelta = quote.Mark - previousQuote.Mark;
                            priceDelta.QuoteDeltaPercent = (quote.Mark / previousQuote.Mark - 1) * 100;
                            if (priceDelta.QuoteDelta > 0)
                            {
                                priceDelta.DeltaIndex++;
                            }

                            if (priceDelta.QuoteDelta < 0)
                            {
                                priceDelta.DeltaIndex--;
                            }
                        }

                        if (priceDelta.Position != null)
                        {
                            priceDelta.PositionDelta = quote.Mark - priceDelta.Position.AveragePrice;
                            priceDelta.PositionDeltaPercent = (quote.Mark / priceDelta.Position.AveragePrice - 1) * 100;
                            if (Math.Abs(priceDelta.PositionDeltaPercent) > 10)
                            {
                                priceDelta.StopPrice = quote.AskPrice;
                            }
                        }
                    }
                }

                catch (Exception exception)
                {

                }

                finally
                {
                    await Task.Delay(60000, stoppingToken);
                }
            }
        }

        private static async Task<IEnumerable<Models.PriceDelta>> GetInstrumentsAsync(IEnumerable<Models.PriceDelta> priceDeltas = null)
        {
            List<Models.PriceDelta> priceDeltasList = priceDeltas == null ? new List<Models.PriceDelta>() : priceDeltas.ToList();
            Entities.Authorization authorization = Modules.TdAmeritrade.Authorization.Update();
            IEnumerable<Models.TdAmeritrade.Account.Account> accounts = Modules.TdAmeritrade.Account.Update(authorization);
            IEnumerable<Models.TdAmeritrade.Watchlist.Watchlist> accountWatchlists = await Modules.TdAmeritrade.Watchlist.GetWatchlistsForMultipleAccountsAsync();
            foreach (Models.TdAmeritrade.Account.Account account in accounts)
            {
                IEnumerable<Models.TdAmeritrade.Account.Instrument> positionInstruments = account.SecuritiesAccount.Positions.Select(x => x.Instrument);
                IEnumerable<Models.TdAmeritrade.Account.Instrument> stocksWatchlistInstruments = accountWatchlists.Single(x => x.AccountId == account.SecuritiesAccount.AccountId && x.Name == "Stocks").WatchlistItems.Select(x => x.Instrument).Where(x => !positionInstruments.Any(y => y.Symbol == x.Symbol));
                IEnumerable<Models.TdAmeritrade.Account.Instrument> instruments = positionInstruments.Concat(stocksWatchlistInstruments);
                foreach (Models.TdAmeritrade.Account.Instrument instrument in instruments)
                {
                    if (priceDeltasList.Any(x => x.AccountId == account.SecuritiesAccount.AccountId && x.Instrument.Symbol == instrument.Symbol))
                    {

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
            }

            return priceDeltasList.AsEnumerable();
        }
    }
}
