using System.Linq;
using System.Text;
using System.Text.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Stocks.Modules.TdAmeritrade
{
    public static class Order
    {
        static readonly Dictionary<string, string> _settings = new Entities.StocksContext().Setting.ToDictionary(x => x.Key, x => x.Value);
        
        static readonly HttpClient _httpClient = new HttpClient();

        public static async Task<IEnumerable<Models.TdAmeritrade.Order.Order>> GetOrdersByPath(string accountId)
        {
            Entities.Authorization authorization = await Authorization.Update();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(authorization.TokenType, authorization.AccessToken);
            using HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync($"{_settings["AccountsUri"]}/{accountId}/orders");
            string json = await httpResponseMessage.Content.ReadAsStringAsync();
            IEnumerable<Models.TdAmeritrade.Order.Order> orders = JsonSerializer.Deserialize<IEnumerable<Models.TdAmeritrade.Order.Order>>(json, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            });
            return orders;
        }

        public static async Task<string> PlaceOrder(Models.TdAmeritrade.Account.Account account, Models.TdAmeritrade.Order.Limit order)
        {
            string json = JsonSerializer.Serialize(order, new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            Entities.Authorization authorization = await Authorization.Update();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(authorization.TokenType, authorization.AccessToken);
            using HttpResponseMessage httpResponseMessage = await _httpClient.PostAsync($"{_settings["AccountsUri"]}/{account.SecuritiesAccount.AccountId}/orders", new StringContent(json, Encoding.UTF8, "application/json"));
            string content = await httpResponseMessage.Content.ReadAsStringAsync();
            return content;
        }

        public static async Task CancelOrder(string accountId, uint orderId)
        {
            Entities.Authorization authorization = await Authorization.Update();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(authorization.TokenType, authorization.AccessToken);
            using HttpResponseMessage httpResponseMessage = await _httpClient.DeleteAsync($"{_settings["AccountsUri"]}/{accountId}/orders/{orderId}");
        }

        //public static /*async*/ void ReplaceOrder(string accountId, Models.TdAmeritrade.Order.Order order)
        //{
            //_httpClient.PutAsync($"{_settings["AccountsUri"]}/{accountId}/orders/{order.OrderId}");
        //}
    }
}
