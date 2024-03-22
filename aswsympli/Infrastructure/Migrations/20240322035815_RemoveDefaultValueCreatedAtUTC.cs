using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveDefaultValueCreatedAtUTC : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAtUtc",
                table: "SeoRankData",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2024, 3, 22, 3, 26, 43, 961, DateTimeKind.Utc).AddTicks(4588));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAtUtc",
                table: "SeoRankData",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2024, 3, 22, 3, 26, 43, 961, DateTimeKind.Utc).AddTicks(4588),
                oldClrType: typeof(DateTime),
                oldType: "TEXT");
        }
    }
}
