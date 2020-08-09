using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MicroservicesSample.Notebooks.Api.Migrations
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "notebooks",
                columns: table => new
                {
                    id = table.Column<string>(nullable: false),
                    text = table.Column<string>(nullable: false),
                    sender_id = table.Column<string>(nullable: false),
                    sender_name = table.Column<string>(nullable: false),
                    created_at = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_notebooks", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "notebooks");
        }
    }
}
