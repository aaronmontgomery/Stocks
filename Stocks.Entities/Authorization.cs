using System;

namespace Stocks.Entities
{
    public class Authorization
    {
        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }

        public string TokenType { get; set; }

        public int ExpiresIn { get; set; }

        public string Scope { get; set; }

        public int RefreshTokenExpiresIn { get; set; }

        public DateTime Updated { get; set; }

        public Guid Id { get; set; }
    }
}
