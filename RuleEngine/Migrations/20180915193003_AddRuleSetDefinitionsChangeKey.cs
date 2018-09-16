using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Crypto.RuleEngine.Migrations
{
    public partial class AddRuleSetDefinitionsChangeKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_RuleSetDefinitions",
                table: "RuleSetDefinitions");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "RuleSetDefinitions",
                nullable: false,
                oldClrType: typeof(int))
                .OldAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RuleSetDefinitions",
                table: "RuleSetDefinitions",
                columns: new[] { "Id", "RuleId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_RuleSetDefinitions",
                table: "RuleSetDefinitions");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "RuleSetDefinitions",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AddPrimaryKey(
                name: "PK_RuleSetDefinitions",
                table: "RuleSetDefinitions",
                column: "Id");
        }
    }
}
