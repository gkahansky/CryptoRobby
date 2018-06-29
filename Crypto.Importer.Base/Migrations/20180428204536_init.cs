using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CryptoRobert.Importer.Cmc.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CmcCoins",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AvailableSupply = table.Column<decimal>(nullable: false),
                    ChangePct1Hr = table.Column<decimal>(nullable: false),
                    ChangePct24Hr = table.Column<decimal>(nullable: false),
                    ChangePct7d = table.Column<decimal>(nullable: false),
                    LastUpdate = table.Column<DateTime>(nullable: false),
                    MarketCapUsd = table.Column<decimal>(nullable: false),
                    MaxSupply = table.Column<decimal>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    PriceBtc = table.Column<decimal>(nullable: false),
                    PriceUsd = table.Column<decimal>(nullable: false),
                    Rank = table.Column<int>(nullable: false),
                    Symbol = table.Column<string>(nullable: true),
                    TotalSupply = table.Column<decimal>(nullable: false),
                    VolumeUsd = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CmcCoins", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CmcCoins");
        }
    }
}
