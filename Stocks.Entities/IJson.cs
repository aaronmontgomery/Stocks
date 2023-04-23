using System.Text.Json;

namespace Stocks.Entities
{
    public interface IJson
    {
        public string Json { get; set; }
        
        public JsonSerializerOptions JsonSerializerOptions { get; set; }
    }
}
