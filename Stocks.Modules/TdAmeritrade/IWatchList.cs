using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Stocks.Modules.TdAmeritrade
{
    public interface IWatchList
    {
        Task<IReadOnlyCollection<Models.TdAmeritrade.Watchlist.Watchlist>> GetWatchlistsForMultipleAccountsAsync(CancellationToken cancellationToken);
        Task<IReadOnlyCollection<Models.TdAmeritrade.Watchlist.Watchlist>> GetWatchListsForSingleAccountAsync(string accountId, CancellationToken cancellationToken);
    }
}
