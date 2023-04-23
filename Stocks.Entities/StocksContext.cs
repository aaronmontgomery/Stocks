using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Stocks.Entities
{
    public partial class StocksDbContext : DbContext
    {
        public StocksDbContext(DbContextOptions<StocksDbContext> options)
            : base(options)
        {
        }
        
        public virtual DbSet<Setting> Setting { get; set; }

        public virtual DbSet<Authorization> Authorization { get; set; }

        public virtual DbSet<SecuritiesAccount> SecuritiesAccount { get; set; }

        public virtual DbSet<Position> Position { get; set; }

        public virtual DbSet<Instrument> Instrument { get; set; }

        public virtual DbSet<CurrentBalances> CurrentBalance { get; set; }

        public virtual DbSet<PriceHistory> PriceHistory { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder
                .UseLazyLoadingProxies()
                .UseSqlServer("Server=localhost;Database=Stocks;Trusted_Connection=True;");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Setting>(entity =>
            {
                entity.HasKey(e => new { e.Type, e.Key });
                
                entity.Property(e => e.Type)
                    .IsRequired();

                entity.Property(e => e.Key)
                    .IsRequired();

                entity.Property(e => e.Value)
                    .IsRequired();

                entity.Property(e => e.Updated)
                    .HasDefaultValueSql("GETUTCDATE()")
                    //.ValueGeneratedOnAddOrUpdate()
                    //.HasValueGenerator<ValueGenerators.DateTimeUtcGenerator>()
                    .IsRequired();

                entity.Property(e => e.Id)
                    .HasDefaultValueSql("NEWSEQUENTIALID()")
                    //.ValueGeneratedOnAdd()
                    //.HasValueGenerator<SequentialGuidValueGenerator>()
                    .IsRequired();
            });

            modelBuilder.Entity<Authorization>(entity =>
            {
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
                    //.HasDefaultValueSql("GETUTCDATE()")
                    //.ValueGeneratedOnAddOrUpdate()
                    //.HasValueGenerator<ValueGenerators.DateTimeUtcGenerator>()
                    .IsRequired();

                entity.Property(e => e.Id)
                    .HasDefaultValueSql("NEWSEQUENTIALID()")
                    //.ValueGeneratedOnAdd()
                    //.HasValueGenerator<SequentialGuidValueGenerator>()
                    .IsRequired();
            });

            modelBuilder.Entity<SecuritiesAccount>(entity =>
            {
                entity.HasKey(e => e.AccountId);

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
                    //.HasDefaultValueSql("GETUTCDATE()")
                    //.ValueGeneratedOnAddOrUpdate()
                    //.HasValueGenerator<ValueGenerators.DateTimeUtcGenerator>()
                    .IsRequired();

                entity.Property(e => e.Id)
                    .HasDefaultValueSql("NEWSEQUENTIALID()")
                    //.ValueGeneratedOnAdd()
                    //.HasValueGenerator<SequentialGuidValueGenerator>()
                    .IsRequired();

                entity.HasOne(d => d.CurrentBalances)
                    .WithOne(p => p.SecuritiesAccount)
                    .HasForeignKey<SecuritiesAccount>("AccountId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                entity.HasMany(d => d.Positions)
                    .WithOne(p => p.SecuritiesAccount)
                    .HasForeignKey(new string[] { "AccountId" })
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

            modelBuilder.Entity<CurrentBalances>(entity =>
            {
                entity.HasKey(e => e.AccountId);

                entity.Property(e => e.AccountId)
                    .IsRequired();

                entity.Property(e => e.AccruedInterest)
                    .HasColumnType("decimal(18, 5)")
                    .IsRequired();

                entity.Property(e => e.CashBalance)
                    .HasColumnType("decimal(18, 5)")
                    .IsRequired();

                entity.Property(e => e.CashReceipts)
                    .HasColumnType("decimal(18, 5)")
                    .IsRequired();

                entity.Property(e => e.LongOptionMarketValue)
                    .HasColumnType("decimal(18, 5)")
                    .IsRequired();

                entity.Property(e => e.LiquidationValue)
                    .HasColumnType("decimal(18, 5)")
                    .IsRequired();

                entity.Property(e => e.LongMarketValue)
                    .HasColumnType("decimal(18, 5)")
                    .IsRequired();

                entity.Property(e => e.MoneyMarketFund)
                    .HasColumnType("decimal(18, 5)")
                    .IsRequired();

                entity.Property(e => e.Savings)
                    .HasColumnType("decimal(18, 5)")
                    .IsRequired();

                entity.Property(e => e.ShortMarketValue)
                    .HasColumnType("decimal(18, 5)")
                    .IsRequired();

                entity.Property(e => e.PendingDeposits)
                    .HasColumnType("decimal(18, 5)")
                    .IsRequired();

                entity.Property(e => e.CashAvailableForTrading)
                    .HasColumnType("decimal(18, 5)")
                    .IsRequired();

                entity.Property(e => e.CashAvailableForWithdrawal)
                    .HasColumnType("decimal(18, 5)")
                    .IsRequired();

                entity.Property(e => e.CashCall)
                    .HasColumnType("decimal(18, 5)")
                    .IsRequired();

                entity.Property(e => e.LongNonMarginableMarketValue)
                    .HasColumnType("decimal(18, 5)")
                    .IsRequired();

                entity.Property(e => e.TotalCash)
                    .HasColumnType("decimal(18, 5)")
                    .IsRequired();

                entity.Property(e => e.ShortOptionMarketValue)
                    .HasColumnType("decimal(18, 5)")
                    .IsRequired();

                entity.Property(e => e.BondValue)
                    .HasColumnType("decimal(18, 5)")
                    .IsRequired();

                entity.Property(e => e.CashDebitCallValue)
                    .HasColumnType("decimal(18, 5)")
                    .IsRequired();

                entity.Property(e => e.UnsettledCash)
                    .HasColumnType("decimal(18, 5)")
                    .IsRequired();

                entity.Property(e => e.Updated)
                    //.HasDefaultValueSql("GETUTCDATE()")
                    //.ValueGeneratedOnAddOrUpdate()
                    //.HasValueGenerator<ValueGenerators.DateTimeUtcGenerator>()
                    .IsRequired();

                entity.Property(e => e.Id)
                    .HasDefaultValueSql("NEWSEQUENTIALID()")
                    //.ValueGeneratedOnAdd()
                    //.HasValueGenerator<SequentialGuidValueGenerator>()
                    .IsRequired();

                entity.HasOne(d => d.SecuritiesAccount)
                    .WithOne(p => p.CurrentBalances)
                    .HasForeignKey<CurrentBalances>(new string[] { "AccountId" })
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });
            
            modelBuilder.Entity<Position>(entity =>
            {
                entity.HasKey(e => new { e.AccountId, e.Exchange, e.Symbol });

                entity.Property(e => e.AccountId)
                    .IsRequired();

                entity.Property(e => e.Exchange)
                    .IsRequired();

                entity.Property(e => e.Symbol)
                    .IsRequired();

                entity.Property(e => e.ShortQuantity)
                    .IsRequired();

                entity.Property(e => e.AveragePrice)
                    .HasColumnType("decimal(18, 5)")
                    .IsRequired();

                entity.Property(e => e.CurrentDayProfitLoss)
                    .HasColumnType("decimal(18, 5)")
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
                    //.HasDefaultValueSql("GETUTCDATE()")
                    //.ValueGeneratedOnAddOrUpdate()
                    //.HasValueGenerator<ValueGenerators.DateTimeUtcGenerator>()
                    .IsRequired();

                entity.Property(e => e.Id)
                    .HasDefaultValueSql("NEWSEQUENTIALID()")
                    //.ValueGeneratedOnAdd()
                    //.HasValueGenerator<SequentialGuidValueGenerator>()
                    .IsRequired();

                entity.HasOne(d => d.SecuritiesAccount)
                    .WithMany(p => p.Positions)
                    .HasForeignKey(new string[] { "AccountId" })
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                entity.HasOne(d => d.Instrument)
                    .WithMany(p => p.Positions)
                    .HasForeignKey(new string[] { "Exchange", "Symbol" })
                    .OnDelete(DeleteBehavior.NoAction)
                    .IsRequired();
            });

            modelBuilder.Entity<Instrument>(entity =>
            {
                entity.HasKey(e => new { e.Exchange, e.Symbol });

                entity.Property(e => e.Exchange)
                    .IsRequired();

                entity.Property(e => e.Symbol)
                    .IsRequired();

                entity.Property(e => e.Cusip);

                entity.Property(e => e.AssetType);

                entity.Property(e => e.Description);
                
                entity.Property(e => e.Created)
                    .IsRequired();

                entity.Property(e => e.Updated)
                    //.HasDefaultValueSql("GETUTCDATE()")
                    //.ValueGeneratedOnAddOrUpdate()
                    //.HasValueGenerator<ValueGenerators.DateTimeUtcGenerator>()
                    .IsRequired();

                entity.Property(e => e.Id)
                    .HasDefaultValueSql("NEWSEQUENTIALID()")
                    //.ValueGeneratedOnAdd()
                    //.HasValueGenerator<SequentialGuidValueGenerator>()
                    .IsRequired();

                entity.HasMany(d => d.Positions)
                    .WithOne(p => p.Instrument)
                    .HasForeignKey(new string[] { "Exchange", "Symbol" })
                    .OnDelete(DeleteBehavior.NoAction)
                    .IsRequired();

                entity.HasMany(d => d.PriceHistories)
                    .WithOne(p => p.Instrument)
                    .HasForeignKey(new string[] { "Exchange", "Symbol" })
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

            modelBuilder.Entity<PriceHistory>(entity =>
            {
                entity.HasKey(e => new { e.Exchange, e.Symbol, e.DateTime });

                entity.Property(e => e.Symbol)
                    .IsRequired();

                entity.Property(e => e.DateTime)
                    .IsRequired();

                entity.Property(e => e.Exchange)
                    .IsRequired();

                entity.Property(e => e.Close)
                    .HasColumnType("decimal(18, 5)")
                    .IsRequired();

                entity.Property(e => e.High)
                    .HasColumnType("decimal(18, 5)")
                    .IsRequired();

                entity.Property(e => e.Low)
                    .HasColumnType("decimal(18, 5)")
                    .IsRequired();

                entity.Property(e => e.Open)
                    .HasColumnType("decimal(18, 5)")
                    .IsRequired();

                entity.Property(e => e.Volume)
                    .IsRequired();

                entity.Property(e => e.Updated)
                    //.HasDefaultValueSql("GETUTCDATE()")
                    //.ValueGeneratedOnAddOrUpdate()
                    //.HasValueGenerator<ValueGenerators.DateTimeUtcGenerator>()
                    .IsRequired();

                entity.Property(e => e.Id)
                    .HasDefaultValueSql("NEWSEQUENTIALID()")
                    //.ValueGeneratedOnAdd()
                    //.HasValueGenerator<SequentialGuidValueGenerator>()
                    .IsRequired();

                entity.HasOne(d => d.Instrument)
                    .WithMany(p => p.PriceHistories)
                    .HasForeignKey(new string[] { "Exchange", "Symbol" })
                    .OnDelete(DeleteBehavior.NoAction)
                    .IsRequired();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
