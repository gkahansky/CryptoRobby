using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Crypto.RuleEngine.Migrations
{
    public partial class AddRuleSetDefinitions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RuleSetDefinitions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Priority = table.Column<int>(nullable: false),
                    RuleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RuleSetDefinitions", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RuleSetDefinitions");
        }
    }
}
