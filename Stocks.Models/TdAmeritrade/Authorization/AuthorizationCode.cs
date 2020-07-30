namespace Stocks.Models.TdAmeritrade.Authorization
{
    public class AuthorizationCode : RefreshToken
    {
#pragma warning disable IDE1006
        public string refresh_token { get; set; }
        public int refresh_token_expires_in { get; set; }
#pragma warning restore IDE1006
    }
}
