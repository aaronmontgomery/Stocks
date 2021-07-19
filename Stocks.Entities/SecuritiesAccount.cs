namespace Stocks.Entities
{
    public class SecuritiesAccount : Models.TdAmeritrade.Account.SecuritiesAccount
    {
        public SecuritiesAccount()
        {
            Positions = new System.Collections.Generic.HashSet<Position>();
        }

        public SecuritiesAccount(Models.TdAmeritrade.Account.SecuritiesAccount securitiesAccount)
        {
            AccountId = securitiesAccount.AccountId;
            Type = securitiesAccount.Type;
            RoundTrips = securitiesAccount.RoundTrips;
            IsDayTrader = securitiesAccount.IsDayTrader;
            IsClosingOnlyRestricted = securitiesAccount.IsClosingOnlyRestricted;
            Positions = new System.Collections.Generic.HashSet<Position>();
            foreach (Models.TdAmeritrade.Account.Position position in securitiesAccount.Positions)
            {
                Positions.Add(new Position(position));
            }

            CurrentBalances = new CurrentBalance(securitiesAccount.CurrentBalances);
            Updated = System.DateTime.UtcNow;
        }

        //public System.Guid Id { get; set; }
        public new virtual System.Collections.Generic.ICollection<Position> Positions { get; set; }
        public new virtual CurrentBalance CurrentBalances { get; set; }
        public System.DateTime Updated { get; set; }
    }
}
