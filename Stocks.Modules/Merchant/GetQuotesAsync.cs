using System.Collections.Generic;
using System.Threading.Tasks;

namespace Stocks.Modules
{
    public static partial class Merchant
    {
        public static async Task<IEnumerable<Models.PriceDelta>> GetQuotesAsync(IEnumerable<Models.PriceDelta> priceDeltas)
        {
            foreach (Models.PriceDelta priceDelta in priceDeltas)
            {
                Models.TdAmeritrade.Quote.Quote quote = await TdAmeritrade.Quote.GetQuoteAsync(priceDelta.Instrument.Symbol);
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

                if (priceDelta.Position == null)
                {

                }

                else
                {
                    priceDelta.PositionDelta = quote.Mark - priceDelta.Position.AveragePrice;
                    priceDelta.PositionDeltaPercent = (quote.Mark / priceDelta.Position.AveragePrice - 1) * 100;
                }
            }

            return priceDeltas;
        }
    }
}
