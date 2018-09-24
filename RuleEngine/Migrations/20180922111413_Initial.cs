using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CryptoRobert.RuleEngine.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RuleDefinitions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Interval = table.Column<string>(nullable: true),
                    Key = table.Column<string>(nullable: true),
                    Operator = table.Column<int>(nullable: false),
                    Priority = table.Column<int>(nullable: false),
                    Retention = table.Column<int>(nullable: false),
                    RuleType = table.Column<string>(nullable: true),
                    State = table.Column<bool>(nullable: false),
                    Symbol = table.Column<string>(nullable: true),
                    Threshold = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RuleDefinitions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RuleSetDefinitions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    RuleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RuleSetDefinitions", x => new { x.Id, x.RuleId });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RuleDefinitions");

            migrationBuilder.DropTable(
                name: "RuleSetDefinitions");
        }
    }
}
