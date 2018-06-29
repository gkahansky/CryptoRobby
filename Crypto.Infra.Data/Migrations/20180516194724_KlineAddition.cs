using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CryptoRobert.Infra.Data.Migrations
{
    public partial class KlineAddition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Wallets");

            migrationBuilder.DropColumn(
                name: "Coin1Id",
                table: "CoinPairs");

            migrationBuilder.DropColumn(
                name: "Coin2Id",
                table: "CoinPairs");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "CoinPairs",
                newName: "Value");

            migrationBuilder.CreateTable(
                name: "Klines",
                columns: table => new
                {
                    Symbol = table.Column<string>(nullable: false),
                    Interval = table.Column<int>(nullable: false),
                    OpenTime = table.Column<long>(nullable: false),
                    Close = table.Column<decimal>(nullable: false),
                    CloseTime = table.Column<long>(nullable: false),
                    High = table.Column<decimal>(nullable: false),
                    Low = table.Column<decimal>(nullable: false),
                    Open = table.Column<decimal>(nullable: false),
                    Volume = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Klines", x => new { x.Symbol, x.Interval, x.OpenTime });
                });

            migrationBuilder.CreateTable(
                name: "MarketData",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ActiveAssets = table.Column<int>(nullable: false),
                    ActiveCurrencies = table.Column<int>(nullable: false),
                    ActiveMarkets = table.Column<int>(nullable: false),
                    BitcoinDominancePct = table.Column<decimal>(nullable: false),
                    MarketDataUsd = table.Column<decimal>(nullable: false),
                    Volume24Hours = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarketData", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Klines");

            migrationBuilder.DropTable(
                name: "MarketData");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "CoinPairs",
                newName: "Price");

            migrationBuilder.AddColumn<int>(
                name: "Coin1Id",
                table: "CoinPairs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Coin2Id",
                table: "CoinPairs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Wallets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CoinId = table.Column<int>(nullable: false),
                    Exchange = table.Column<int>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wallets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Wallets_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Wallets_UserId",
                table: "Wallets",
                column: "UserId");
        }
    }
}
