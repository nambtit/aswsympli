using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SeoRankData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Ranks = table.Column<string>(type: "TEXT", nullable: false),
                    Keyword = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    CompanyUrl = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    Engine = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValue: new DateTime(2024, 3, 22, 3, 26, 43, 961, DateTimeKind.Utc).AddTicks(4588))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeoRankData", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SeoRankData");
        }
    }
}
