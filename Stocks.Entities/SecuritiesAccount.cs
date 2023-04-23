using System;
using System.Collections.Generic;

namespace Stocks.Entities
{
    public class SecuritiesAccount : Models.TdAmeritrade.Account.SecuritiesAccount
    {
        public Guid Id { get; set; }

        public DateTime Updated { get; set; }

        public new virtual CurrentBalances CurrentBalances { get; set; }

        public new virtual ICollection<Position> Positions { get; set; }

        public SecuritiesAccount(StocksDbContext stocksDbContext, Dictionary<string, string> settings, Models.TdAmeritrade.Account.SecuritiesAccount securitiesAccount)
        {
            AccountId = securitiesAccount.AccountId;
            Type = securitiesAccount.Type;
            RoundTrips = securitiesAccount.RoundTrips;
            IsDayTrader = securitiesAccount.IsDayTrader;
            IsClosingOnlyRestricted = securitiesAccount.IsClosingOnlyRestricted;
            Positions = new HashSet<Position>();
            if (securitiesAccount.Positions != null)
            {
                foreach (Models.TdAmeritrade.Account.Position position in securitiesAccount.Positions)
                {
                    Positions.Add(new Position(stocksDbContext, settings, position, securitiesAccount.AccountId));
                }
            }

            if (securitiesAccount.CurrentBalances != null)
            {
                CurrentBalances = new CurrentBalances(securitiesAccount.CurrentBalances, securitiesAccount.AccountId);
            }

            Updated = DateTime.UtcNow;
        }

        public SecuritiesAccount()
        {
            Positions = new HashSet<Position>();
        }
    }
}
