using Microsoft.EntityFrameworkCore;

namespace Stocks.Entities
{
    public partial class StocksContext : DbContext
    {
        public StocksContext()
        {
        }

        public StocksContext(DbContextOptions<StocksContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Setting> Setting { get; set; }
        public virtual DbSet<Ticker> Ticker { get; set; }
        public virtual DbSet<PriceHistory> PriceHistory { get; set; }
        public virtual DbSet<Authorization> Authorization { get; set; }
        public virtual DbSet<SecuritiesAccount> SecuritiesAccount { get; set; }
        public virtual DbSet<Position> Position { get; set; }
        public virtual DbSet<Instrument> Instrument { get; set; }
        public virtual DbSet<CurrentBalance> CurrentBalance { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder
                .UseLazyLoadingProxies()
                .UseSqlServer("Server=localhost;Database=Stocks;Trusted_Connection=True;");
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Setting>(entity =>
            {
                entity.HasKey(e => e.Key);

                entity.Property(e => e.Key)
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(e => e.Type)
                    .IsRequired();

                entity.Property(e => e.Value)
                    .IsRequired();

                entity.Property(e => e.Updated)
                    .HasDefaultValueSql("GETUTCDATE()")
                    .IsRequired();
            });

            modelBuilder.Entity<Ticker>(entity =>
            {
                entity.HasKey(e => e.Symbol);

                entity.Property(e => e.Symbol)
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(e => e.Name)
                    .IsRequired();

                entity.Property(e => e.LastSale)
                    .IsRequired();

                entity.Property(e => e.MarketCap)
                    .IsRequired();

                entity.Property(e => e.IpoYear)
                    .IsRequired();

                entity.Property(e => e.Sector)
                    .IsRequired();

                entity.Property(e => e.Industry)
                    .IsRequired();

                entity.Property(e => e.SummaryQuote)
                    .IsRequired();

                entity.Property(e => e.Updated)
                    .IsRequired();
            });

            modelBuilder.Entity<PriceHistory>(entity =>
            {
                entity.HasKey(e => new { e.Symbol, e.DateTime });

                entity.HasAlternateKey(e => e.Id);

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .IsRequired();

                entity.Property(e => e.Symbol)
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(e => e.Close)
                    .IsRequired();

                entity.Property(e => e.High)
                    .IsRequired();

                entity.Property(e => e.Low)
                    .IsRequired();

                entity.Property(e => e.Open)
                    .IsRequired();

                entity.Property(e => e.DateTime)
                    .IsRequired();

                entity.Property(e => e.Updated)
                    .IsRequired();

                entity.HasOne(d => d.SymbolNavigation)
                    .WithMany(p => p.PriceHistory)
                    .HasForeignKey(d => d.Symbol)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_PriceHistory_Ticker")
                    .IsRequired();
            });

            modelBuilder.Entity<Authorization>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .IsRequired();

                entity.Property(e => e.AccessToken)
                    .IsRequired();

                entity.Property(e => e.RefreshToken)
                    .IsRequired();

                entity.Property(e => e.TokenType)
                    .IsRequired();

                entity.Property(e => e.ExpiresIn)
                    .IsRequired();

                entity.Property(e => e.Scope)
                    .IsRequired();

                entity.Property(e => e.RefreshTokenExpiresIn)
                    .IsRequired();

                entity.Property(e => e.Updated)
                    .IsRequired();
            });

            modelBuilder.Entity<SecuritiesAccount>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasAlternateKey(e => e.AccountId);

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .IsRequired();

                entity.Property(e => e.AccountId)
                    .IsRequired();

                entity.Property(e => e.Type)
                    .IsRequired();

                entity.Property(e => e.RoundTrips)
                    .IsRequired();

                entity.Property(e => e.IsDayTrader)
                    .IsRequired();

                entity.Property(e => e.IsClosingOnlyRestricted)
                    .IsRequired();

                entity.Property(e => e.Updated)
                    .IsRequired();
            });

            modelBuilder.Entity<Position>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .IsRequired();

                entity.Property(e => e.SecuritiesAccountId)
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(e => e.ShortQuantity)
                    .IsRequired();

                entity.Property(e => e.AveragePrice)
                    .IsRequired();

                entity.Property(e => e.CurrentDayProfitLoss)
                    .IsRequired();

                entity.Property(e => e.CurrentDayProfitLossPercentage)
                    .IsRequired();

                entity.Property(e => e.LongQuantity)
                    .IsRequired();

                entity.Property(e => e.SettledLongQuantity)
                    .IsRequired();

                entity.Property(e => e.SettledShortQuantity)
                    .IsRequired();

                entity.Property(e => e.MarketValue)
                    .IsRequired();

                entity.Property(e => e.Updated)
                    .IsRequired();

                entity.HasOne(d => d.SecuritiesAccountIdNavigation)
                    .WithMany(p => p.Positions)
                    .HasForeignKey(d => d.SecuritiesAccountId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Position_SecuritiesAccount")
                    .IsRequired();
            });

            modelBuilder.Entity<Instrument>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .IsRequired();

                entity.Property(e => e.AssetType)
                    .IsRequired();

                entity.Property(e => e.Cusip)
                    .IsRequired();

                entity.Property(e => e.Symbol)
                    .IsRequired();

                entity.Property(e => e.Updated)
                    .IsRequired();

                entity.HasOne(d => d.PositionIdNavigation)
                    .WithOne(p => p.Instrument)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Instrument_Position")
                    .IsRequired();
            });

            modelBuilder.Entity<CurrentBalance>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .IsRequired();

                entity.Property(e => e.SecuritiesAccountId)
                    .IsRequired();

                entity.Property(e => e.AccruedInterest)
                    .IsRequired();

                entity.Property(e => e.CashBalance)
                    .IsRequired();

                entity.Property(e => e.CashReceipts)
                    .IsRequired();

                entity.Property(e => e.LongOptionMarketValue)
                    .IsRequired();

                entity.Property(e => e.LiquidationValue)
                    .IsRequired();

                entity.Property(e => e.LongMarketValue)
                    .IsRequired();

                entity.Property(e => e.MoneyMarketFund)
                    .IsRequired();

                entity.Property(e => e.Savings)
                    .IsRequired();

                entity.Property(e => e.ShortMarketValue)
                    .IsRequired();

                entity.Property(e => e.PendingDeposits)
                    .IsRequired();

                entity.Property(e => e.CashAvailableForTrading)
                    .IsRequired();

                entity.Property(e => e.CashAvailableForWithdrawal)
                    .IsRequired();

                entity.Property(e => e.CashCall)
                    .IsRequired();

                entity.Property(e => e.LongNonMarginableMarketValue)
                    .IsRequired();

                entity.Property(e => e.TotalCash)
                    .IsRequired();

                entity.Property(e => e.ShortOptionMarketValue)
                    .IsRequired();

                entity.Property(e => e.BondValue)
                    .IsRequired();

                entity.Property(e => e.CashDebitCallValue)
                    .IsRequired();

                entity.Property(e => e.UnsettledCash)
                    .IsRequired();

                entity.Property(e => e.Updated)
                    .IsRequired();

                entity.HasOne(d => d.SecuritiesAccountIdNavigation)
                    .WithOne(p => p.CurrentBalances)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_CurrentBalance_SecuritiesAccount")
                    .IsRequired();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
