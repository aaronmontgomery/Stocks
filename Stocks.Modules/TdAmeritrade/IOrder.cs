using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Stocks.Modules.TdAmeritrade
{
    public interface IOrder
    {
        Task<IReadOnlyCollection<Models.TdAmeritrade.Order.Order>> GetOrdersByPathAsync(string accountId, CancellationToken cancellationToken);
        Task<string> PlaceOrderAsync(string accountId, Models.TdAmeritrade.Order.Limit order, CancellationToken cancellationToken);
        Task CancelOrderAsync(string accountId, uint orderId, CancellationToken cancellationToken);
    }
}
