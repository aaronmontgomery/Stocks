using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Stocks.Modules.TdAmeritrade
{
    public interface IAccount
    {
        Task<IAsyncEnumerable<Models.TdAmeritrade.Account.Account>> UpdateAccountAsync(CancellationToken cancellationToken = default);
    }
}
