using System.Linq;
using System.Text.Json;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Stocks.Entities
{
    public class DataAdapter : IDataAdapter // internal
    {
        public IDbContextFactory<StocksDbContext> StocksDbContextFactory { get; }
        
        public Dictionary<string, string> Settings { get; }

        public JsonSerializerOptions JsonSerializerOptions { get; set; }

        public string Json { get; set; }

        public DataAdapter(IDbContextFactory<StocksDbContext> stocksDbContextFactory)
        {
            StocksDbContextFactory = stocksDbContextFactory;
            using var stocksDbContext = stocksDbContextFactory.CreateDbContext();
            Settings = stocksDbContext.Setting.ToDictionary(x => x.Key, x => x.Value);
            if (!Requester.IsInitialized)
            {
                this.Initialize();
            }
        }
    }
}
