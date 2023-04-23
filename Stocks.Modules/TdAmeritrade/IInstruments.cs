using System.Threading;
using System.Threading.Tasks;

namespace Stocks.Modules.TdAmeritrade
{
    public interface IInstruments
    {
        Task GetInstrumentsAsync(CancellationToken cancellationToken);
    }
}
