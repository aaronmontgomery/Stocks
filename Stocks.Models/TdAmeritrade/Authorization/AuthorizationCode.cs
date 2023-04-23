using System.Text.Json.Serialization;

namespace Stocks.Models.TdAmeritrade.Authorization
{
    public class AuthorizationCode : RefreshToken
    {
        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; }
        
        [JsonPropertyName("refresh_token_expires_in")]
        public int RefreshTokenExpiresIn { get; set; }
    }
}
