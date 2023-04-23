using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stocks.Entities.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Authorization",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    AccessToken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TokenType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpiresIn = table.Column<int>(type: "int", nullable: false),
                    Scope = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RefreshTokenExpiresIn = table.Column<int>(type: "int", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authorization", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Instrument",
                columns: table => new
                {
                    Exchange = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Symbol = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AssetType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cusip = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Instrument", x => new { x.Exchange, x.Symbol });
                });

            migrationBuilder.CreateTable(
                name: "SecuritiesAccount",
                columns: table => new
                {
                    AccountId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoundTrips = table.Column<int>(type: "int", nullable: false),
                    IsDayTrader = table.Column<bool>(type: "bit", nullable: false),
                    IsClosingOnlyRestricted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SecuritiesAccount", x => x.AccountId);
                });

            migrationBuilder.CreateTable(
                name: "Setting",
                columns: table => new
                {
                    Key = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Setting", x => new { x.Type, x.Key });
                });

            migrationBuilder.CreateTable(
                name: "PriceHistory",
                columns: table => new
                {
                    Exchange = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Symbol = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Open = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    High = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    Low = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    Close = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    Volume = table.Column<long>(type: "bigint", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceHistory", x => new { x.Exchange, x.Symbol, x.DateTime });
                    table.ForeignKey(
                        name: "FK_PriceHistory_Instrument_Exchange_Symbol",
                        columns: x => new { x.Exchange, x.Symbol },
                        principalTable: "Instrument",
                        principalColumns: new[] { "Exchange", "Symbol" });
                });

            migrationBuilder.CreateTable(
                name: "CurrentBalance",
                columns: table => new
                {
                    AccountId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    AccruedInterest = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    CashBalance = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    CashReceipts = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    LongOptionMarketValue = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    LiquidationValue = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    LongMarketValue = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    MoneyMarketFund = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    Savings = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    ShortMarketValue = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    PendingDeposits = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    CashAvailableForTrading = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    CashAvailableForWithdrawal = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    CashCall = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    LongNonMarginableMarketValue = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    TotalCash = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    ShortOptionMarketValue = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    BondValue = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    CashDebitCallValue = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    UnsettledCash = table.Column<decimal>(type: "decimal(18,5)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrentBalance", x => x.AccountId);
                    table.ForeignKey(
                        name: "FK_CurrentBalance_SecuritiesAccount_AccountId",
                        column: x => x.AccountId,
                        principalTable: "SecuritiesAccount",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Position",
                columns: table => new
                {
                    AccountId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Exchange = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Symbol = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ShortQuantity = table.Column<double>(type: "float", nullable: false),
                    AveragePrice = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    CurrentDayProfitLoss = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    CurrentDayProfitLossPercentage = table.Column<double>(type: "float", nullable: false),
                    LongQuantity = table.Column<double>(type: "float", nullable: false),
                    SettledLongQuantity = table.Column<double>(type: "float", nullable: false),
                    SettledShortQuantity = table.Column<double>(type: "float", nullable: false),
                    MarketValue = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Position", x => new { x.AccountId, x.Exchange, x.Symbol });
                    table.ForeignKey(
                        name: "FK_Position_Instrument_Exchange_Symbol",
                        columns: x => new { x.Exchange, x.Symbol },
                        principalTable: "Instrument",
                        principalColumns: new[] { "Exchange", "Symbol" });
                    table.ForeignKey(
                        name: "FK_Position_SecuritiesAccount_AccountId",
                        column: x => x.AccountId,
                        principalTable: "SecuritiesAccount",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Position_Exchange_Symbol",
                table: "Position",
                columns: new[] { "Exchange", "Symbol" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Authorization");

            migrationBuilder.DropTable(
                name: "CurrentBalance");

            migrationBuilder.DropTable(
                name: "Position");

            migrationBuilder.DropTable(
                name: "PriceHistory");

            migrationBuilder.DropTable(
                name: "Setting");

            migrationBuilder.DropTable(
                name: "SecuritiesAccount");

            migrationBuilder.DropTable(
                name: "Instrument");
        }
    }
}
