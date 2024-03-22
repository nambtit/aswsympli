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
            var utcNow = new DateTime(2024, 3, 22);

            migrationBuilder.InsertData(
                table: "SeoRankData",
                columns: new[] { "Ranks", "Keyword", "CompanyUrl", "Engine", "CreatedAtUtc" },
                values: new[] { "0,1", keyword, companyUrl, DbSearchEngineEnum.Bing.ToString(), utcNow.ToString() });

            migrationBuilder.InsertData(
                table: "SeoRankData",
                columns: new[] { "Ranks", "Keyword", "CompanyUrl", "Engine", "CreatedAtUtc" },
                values: new[] { "1,6", keyword, companyUrl, DbSearchEngineEnum.Google.ToString(), utcNow.ToString() });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
