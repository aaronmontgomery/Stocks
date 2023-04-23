using Microsoft.EntityFrameworkCore;

namespace Stocks.Entities
{
    public interface IStocksDbContext
    {
        IDbContextFactory<StocksDbContext> StocksDbContextFactory { get; }
    }
}
