using System.Net;
using System.Linq;
using System.Text.Json;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Stocks.Entities;

namespace Stocks.Modules.TdAmeritrade
{
    internal class Quote : IQuote
    {
        private IReadOnlyCollection<Models.TdAmeritrade.Quote.Quote> _quotes;
        private readonly IDataAdapter _dataAdapter;

        public Quote(IDataAdapter dataAdapter)
        {
            _dataAdapter = dataAdapter;
        }

        public async Task<Models.TdAmeritrade.Quote.Quote> GetQuoteAsync(string symbol, CancellationToken cancellationToken = default)
        {
            _dataAdapter.JsonSerializerOptions = new()
            {
                PropertyNameCaseInsensitive = true
            };
            _dataAdapter.Json = await Requester.SendRequestAsync(Enums.HttpVerb.Get, $"{_dataAdapter.Settings["MarketDataUrl"]}/{WebUtility.UrlEncode(symbol)}/quotes?apikey={_dataAdapter.Settings["ApiKey"]}", null, cancellationToken);
            _dataAdapter.Json = Toolbox.Json.RemoveTopLevelKeys(_dataAdapter.Json);
            _quotes = JsonSerializer.Deserialize<IReadOnlyCollection<Models.TdAmeritrade.Quote.Quote>>(_dataAdapter.Json, _dataAdapter.JsonSerializerOptions);
            return _quotes.Single();
        }

        public async Task<IReadOnlyCollection<Models.TdAmeritrade.Quote.Quote>> GetQuotesAsync(IEnumerable<string> symbols, CancellationToken cancellationToken)
        {
            _dataAdapter.JsonSerializerOptions = new()
            {
                PropertyNameCaseInsensitive = true
            };
            _dataAdapter.Json = await Requester.SendRequestAsync(Enums.HttpVerb.Get, $"{_dataAdapter.Settings["MarketDataUrl"]}/quotes?apikey={_dataAdapter.Settings["ApiKey"]}&symbol={WebUtility.UrlEncode(string.Join(',', symbols))}", null, cancellationToken);
            _dataAdapter.Json = Toolbox.Json.RemoveTopLevelKeys(_dataAdapter.Json);
            _quotes = JsonSerializer.Deserialize<IReadOnlyCollection<Models.TdAmeritrade.Quote.Quote>>(_dataAdapter.Json, _dataAdapter.JsonSerializerOptions);
            return _quotes;
        }
    }
}
