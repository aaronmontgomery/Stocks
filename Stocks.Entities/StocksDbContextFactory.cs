using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace Stocks.Entities
{
    public class StocksDbContextFactory : IDesignTimeDbContextFactory<StocksDbContext>
    {
        public StocksDbContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder dbContextOptionsBuilder = new DbContextOptionsBuilder<StocksDbContext>();
            dbContextOptionsBuilder.UseLazyLoadingProxies();
            dbContextOptionsBuilder.UseSqlServer("Server=localhost;Database=Stocks;Trusted_Connection=True;");
            return new StocksDbContext((DbContextOptions<StocksDbContext>)dbContextOptionsBuilder.Options);
        }
    }
}
