using Infrastructure.Enums;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedRankData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            const string keyword = "\"e-settlements\"";
            const string companyUrl = "https://www.sympli.com.au";
            var utcNow = DateTime.UtcNow;

            migrationBuilder.InsertData(
                table: "SeoRankData",
                columns: new[] { "Ranks", "Keyword", "CompanyUrl", "Engine", "CreatedAtUtc" },
                values: new[] { "0,1", keyword, companyUrl, SearchEngineDbEnum.Bing.ToString(), utcNow.ToString() });

            migrationBuilder.InsertData(
                table: "SeoRankData",
                columns: new[] { "Ranks", "Keyword", "CompanyUrl", "Engine", "CreatedAtUtc" },
                values: new[] { "1,6", keyword, companyUrl, SearchEngineDbEnum.Google.ToString(), utcNow.ToString() });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
