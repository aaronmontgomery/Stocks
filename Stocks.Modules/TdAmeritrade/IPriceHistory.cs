using System.Threading;
using System.Threading.Tasks;

namespace Stocks.Modules.TdAmeritrade
{
    public interface IPriceHistory
    {
        Task GetPriceHistoryAsync(CancellationToken cancellationToken);
    }
}
