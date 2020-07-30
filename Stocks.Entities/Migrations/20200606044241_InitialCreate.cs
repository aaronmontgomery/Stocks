using System;
using Microsoft.EntityFrameworkCore.Migrations;

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
                    Id = table.Column<Guid>(nullable: false),
                    AccessToken = table.Column<string>(nullable: false),
                    RefreshToken = table.Column<string>(nullable: false),
                    TokenType = table.Column<string>(nullable: false),
                    ExpiresIn = table.Column<int>(nullable: false),
                    Scope = table.Column<string>(nullable: false),
                    RefreshTokenExpiresIn = table.Column<int>(nullable: false),
                    Updated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authorization", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SecuritiesAccount",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AccountId = table.Column<string>(nullable: false),
                    Type = table.Column<string>(nullable: false),
                    RoundTrips = table.Column<int>(nullable: false),
                    IsDayTrader = table.Column<bool>(nullable: false),
                    IsClosingOnlyRestricted = table.Column<bool>(nullable: false),
                    Updated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SecuritiesAccount", x => x.Id);
                    table.UniqueConstraint("AK_SecuritiesAccount_AccountId", x => x.AccountId);
                });

            migrationBuilder.CreateTable(
                name: "Setting",
                columns: table => new
                {
                    Key = table.Column<string>(maxLength: 50, nullable: false),
                    Type = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: false),
                    Updated = table.Column<DateTime>(nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Setting", x => x.Key);
                });

            migrationBuilder.CreateTable(
                name: "Ticker",
                columns: table => new
                {
                    Symbol = table.Column<string>(maxLength: 50, nullable: false),
                    Name = table.Column<string>(nullable: false),
                    LastSale = table.Column<string>(nullable: false),
                    MarketCap = table.Column<string>(nullable: false),
                    IpoYear = table.Column<string>(nullable: false),
                    Sector = table.Column<string>(nullable: false),
                    Industry = table.Column<string>(nullable: false),
                    SummaryQuote = table.Column<string>(nullable: false),
                    Updated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ticker", x => x.Symbol);
                });

            migrationBuilder.CreateTable(
                name: "CurrentBalance",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AccruedInterest = table.Column<decimal>(nullable: false),
                    CashBalance = table.Column<decimal>(nullable: false),
                    CashReceipts = table.Column<decimal>(nullable: false),
                    LongOptionMarketValue = table.Column<decimal>(nullable: false),
                    LiquidationValue = table.Column<decimal>(nullable: false),
                    LongMarketValue = table.Column<decimal>(nullable: false),
                    MoneyMarketFund = table.Column<decimal>(nullable: false),
                    Savings = table.Column<decimal>(nullable: false),
                    ShortMarketValue = table.Column<decimal>(nullable: false),
                    PendingDeposits = table.Column<decimal>(nullable: false),
                    CashAvailableForTrading = table.Column<decimal>(nullable: false),
                    CashAvailableForWithdrawal = table.Column<decimal>(nullable: false),
                    CashCall = table.Column<decimal>(nullable: false),
                    LongNonMarginableMarketValue = table.Column<decimal>(nullable: false),
                    TotalCash = table.Column<decimal>(nullable: false),
                    ShortOptionMarketValue = table.Column<decimal>(nullable: false),
                    BondValue = table.Column<decimal>(nullable: false),
                    CashDebitCallValue = table.Column<decimal>(nullable: false),
                    UnsettledCash = table.Column<decimal>(nullable: false),
                    SecuritiesAccountId = table.Column<Guid>(nullable: false),
                    Updated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrentBalance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CurrentBalance_SecuritiesAccount",
                        column: x => x.SecuritiesAccountId,
                        principalTable: "SecuritiesAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Position",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ShortQuantity = table.Column<double>(nullable: false),
                    AveragePrice = table.Column<decimal>(nullable: false),
                    CurrentDayProfitLoss = table.Column<decimal>(nullable: false),
                    CurrentDayProfitLossPercentage = table.Column<double>(nullable: false),
                    LongQuantity = table.Column<double>(nullable: false),
                    SettledLongQuantity = table.Column<double>(nullable: false),
                    SettledShortQuantity = table.Column<double>(nullable: false),
                    MarketValue = table.Column<double>(nullable: false),
                    SecuritiesAccountId = table.Column<Guid>(maxLength: 50, nullable: false),
                    Updated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Position", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Position_SecuritiesAccount",
                        column: x => x.SecuritiesAccountId,
                        principalTable: "SecuritiesAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PriceHistory",
                columns: table => new
                {
                    Symbol = table.Column<string>(maxLength: 50, nullable: false),
                    DateTime = table.Column<DateTime>(nullable: false),
                    Id = table.Column<Guid>(nullable: false),
                    Open = table.Column<decimal>(nullable: false),
                    High = table.Column<decimal>(nullable: false),
                    Low = table.Column<decimal>(nullable: false),
                    Close = table.Column<decimal>(nullable: false),
                    Volume = table.Column<long>(nullable: false),
                    Updated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceHistory", x => new { x.Symbol, x.DateTime });
                    table.UniqueConstraint("AK_PriceHistory_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PriceHistory_Ticker",
                        column: x => x.Symbol,
                        principalTable: "Ticker",
                        principalColumn: "Symbol",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Instrument",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AssetType = table.Column<string>(nullable: false),
                    Cusip = table.Column<string>(nullable: false),
                    Symbol = table.Column<string>(nullable: false),
                    PositionId = table.Column<Guid>(nullable: false),
                    Updated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Instrument", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Instrument_Position",
                        column: x => x.PositionId,
                        principalTable: "Position",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CurrentBalance_SecuritiesAccountId",
                table: "CurrentBalance",
                column: "SecuritiesAccountId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Instrument_PositionId",
                table: "Instrument",
                column: "PositionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Position_SecuritiesAccountId",
                table: "Position",
                column: "SecuritiesAccountId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Authorization");

            migrationBuilder.DropTable(
                name: "CurrentBalance");

            migrationBuilder.DropTable(
                name: "Instrument");

            migrationBuilder.DropTable(
                name: "PriceHistory");

            migrationBuilder.DropTable(
                name: "Setting");

            migrationBuilder.DropTable(
                name: "Position");

            migrationBuilder.DropTable(
                name: "Ticker");

            migrationBuilder.DropTable(
                name: "SecuritiesAccount");
        }
    }
}
