using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Stocks.Modules.TdAmeritrade
{
    public interface IQuote
    {
        Task<Models.TdAmeritrade.Quote.Quote> GetQuoteAsync(string symbol, CancellationToken cancellationToken);
        Task<IReadOnlyCollection<Models.TdAmeritrade.Quote.Quote>> GetQuotesAsync(IEnumerable<string> symbols, CancellationToken cancellationToken);
    }
}
