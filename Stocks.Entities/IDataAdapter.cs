using System.Collections.Generic;

namespace Stocks.Entities
{
    public interface IDataAdapter : IStocksDbContext, IJson
    {
        Dictionary<string, string> Settings { get; }
    }
}
