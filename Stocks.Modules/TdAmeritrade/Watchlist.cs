using System.Text.Json;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Stocks.Entities;

namespace Stocks.Modules.TdAmeritrade
{
    public class Watchlist : IWatchList
    {
        private readonly IDataAdapter _dataAdapter;
        private IReadOnlyCollection<Models.TdAmeritrade.Watchlist.Watchlist> _watchlists;

        public Watchlist(IDataAdapter dataAdapter)
        {
            _dataAdapter = dataAdapter;
        }
        
        public async Task<IReadOnlyCollection<Models.TdAmeritrade.Watchlist.Watchlist>> GetWatchlistsForMultipleAccountsAsync(CancellationToken cancellationToken)
        {
            _dataAdapter.JsonSerializerOptions = new()
            {
                PropertyNameCaseInsensitive = true
            };
            _dataAdapter.Json = await Requester.SendRequestAsync(Enums.HttpVerb.Get, $"{_dataAdapter.Settings["AccountsUrl"]}/watchlists", null, cancellationToken);
            _watchlists = JsonSerializer.Deserialize<IReadOnlyCollection<Models.TdAmeritrade.Watchlist.Watchlist>>(_dataAdapter.Json, _dataAdapter.JsonSerializerOptions);
            return _watchlists;
        }

        public async Task<IReadOnlyCollection<Models.TdAmeritrade.Watchlist.Watchlist>> GetWatchListsForSingleAccountAsync(string accountId, CancellationToken cancellationToken)
        {
            _dataAdapter.JsonSerializerOptions = new()
            {
                PropertyNameCaseInsensitive = true
            };
            _dataAdapter.Json = await Requester.SendRequestAsync(Enums.HttpVerb.Get, $"{_dataAdapter.Settings["AccountsUrl"]}/{accountId}/watchlists", null, cancellationToken);
            _watchlists = JsonSerializer.Deserialize<IReadOnlyCollection<Models.TdAmeritrade.Watchlist.Watchlist>>(_dataAdapter.Json, _dataAdapter.JsonSerializerOptions);
            return _watchlists;
        }
    }
}
