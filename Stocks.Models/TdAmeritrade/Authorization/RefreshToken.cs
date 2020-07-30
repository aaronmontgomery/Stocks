namespace Stocks.Models.TdAmeritrade.Authorization
{
    public class RefreshToken
    {
#pragma warning disable IDE1006
        public string access_token { get; set; }
        public string scope { get; set; }
        public int expires_in { get; set; }
        public string token_type { get; set; }
#pragma warning restore IDE1006
    }
}
