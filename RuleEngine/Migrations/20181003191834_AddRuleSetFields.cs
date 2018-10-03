using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CryptoRobert.RuleEngine.Migrations
{
    public partial class AddRuleSetFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "RuleSets",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModified",
                table: "RuleSets",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "RuleSets",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PairToBuy",
                table: "RuleSets",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "RuleSets");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "RuleSets");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "RuleSets");

            migrationBuilder.DropColumn(
                name: "PairToBuy",
                table: "RuleSets");
        }
    }
}
