using System.Linq;
using System.Text.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Stocks.Modules.TdAmeritrade
{
    public static class Watchlist
    {
        private static readonly Dictionary<string, string> _settings = new Entities.StocksContext().Setting.ToDictionary(x => x.Key, x => x.Value);

        private static readonly HttpClient _httpClient = new HttpClient();

        public static async Task<IEnumerable<Models.TdAmeritrade.Watchlist.Watchlist>> GetWatchlistsForMultipleAccountsAsync()
        {
            Entities.Authorization authorization = await Authorization .Update();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(authorization.TokenType, authorization.AccessToken);
            using HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync($"{_settings["AccountsUri"]}/watchlists");
            string json = await httpResponseMessage.Content.ReadAsStringAsync();
            IEnumerable<Models.TdAmeritrade.Watchlist.Watchlist> watchlists = JsonSerializer.Deserialize<IEnumerable<Models.TdAmeritrade.Watchlist.Watchlist>>(json, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            });
            return watchlists;
        }

        public static async Task<IEnumerable<Models.TdAmeritrade.Watchlist.Watchlist>> GetWatchListsForSingleAccountAsync(string accountId)
        {
            Entities.Authorization authorization = await Authorization.Update();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(authorization.TokenType, authorization.AccessToken);
            using HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync($"{_settings["AccountsUri"]}/{accountId}/watchlists");
            string json = await httpResponseMessage.Content.ReadAsStringAsync();
            IEnumerable<Models.TdAmeritrade.Watchlist.Watchlist> watchlists = JsonSerializer.Deserialize<IEnumerable<Models.TdAmeritrade.Watchlist.Watchlist>>(json, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            });
            return watchlists;
        }
    }
}
