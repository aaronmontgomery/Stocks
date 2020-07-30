namespace Stocks.Entities
{
    public class Authorization
    {
        public System.Guid Id { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string TokenType { get; set; }
        public int ExpiresIn { get; set; }
        public string Scope { get; set; }
        public int RefreshTokenExpiresIn { get; set; }
        public System.DateTime Updated { get; set; }
    }
}
