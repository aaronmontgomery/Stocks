using System.Text;
using System.Text.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Stocks.Entities;

namespace Stocks.Modules.TdAmeritrade
{
    public class Order : IOrder
    {
        private readonly IDataAdapter _dataAdapter;
        private IReadOnlyCollection<Models.TdAmeritrade.Order.Order> _orders;
        
        public Order(IDataAdapter dataAdapter)
        {
            _dataAdapter = dataAdapter;
        }
        
        public async Task<IReadOnlyCollection<Models.TdAmeritrade.Order.Order>> GetOrdersByPathAsync(string accountId, CancellationToken cancellationToken)
        {
            _dataAdapter.JsonSerializerOptions = new()
            {
                PropertyNameCaseInsensitive = true
            };
            _dataAdapter.Json = await Requester.SendRequestAsync(Enums.HttpVerb.Get, $"{_dataAdapter.Settings["AccountsUrl"]}/{accountId}/orders", null, cancellationToken);
            _orders = JsonSerializer.Deserialize<IReadOnlyCollection<Models.TdAmeritrade.Order.Order>>(_dataAdapter.Json, _dataAdapter.JsonSerializerOptions);
            return _orders;
        }
        
        public async Task<string> PlaceOrderAsync(string accountId, Models.TdAmeritrade.Order.Limit order, CancellationToken cancellationToken)
        {
            _dataAdapter.JsonSerializerOptions = new()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            _dataAdapter.Json = JsonSerializer.Serialize(order, _dataAdapter.JsonSerializerOptions);
            _dataAdapter.Json = await Requester.SendRequestAsync(Enums.HttpVerb.Post, $"{_dataAdapter.Settings["AccountsUrl"]}/{accountId}/orders", new StringContent(_dataAdapter.Json, Encoding.UTF8, "application/json"), cancellationToken);
            return _dataAdapter.Json;
        }
        
        public async Task CancelOrderAsync(string accountId, uint orderId, CancellationToken cancellationToken)
        {
            await Requester.SendRequestAsync(Enums.HttpVerb.Delete, $"{_dataAdapter.Settings["AccountsUrl"]}/{accountId}/orders/{orderId}", null, cancellationToken);
        }
    }
}
