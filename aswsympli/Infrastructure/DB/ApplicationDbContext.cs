using Application.Abstraction;
using Application.Features.SEORank.Enums;
using Application.Models;
using Infrastructure.DB.Entities;
using Infrastructure.DB.EntityTypeConfig;
using Infrastructure.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DB
{
    public class ApplicationDbContext : DbContext, IApplicationDb
    {
        private readonly IApplicationConfig _applicationConfig;

        public ApplicationDbContext(IApplicationConfig applicationConfig)
        {
            _applicationConfig = applicationConfig;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(_applicationConfig.DbConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            new SeoRankDataETC().Configure(modelBuilder.Entity<SeoRankData>());
            base.OnModelCreating(modelBuilder);
        }

        public async Task<SearchRankData> GetRankDataByEngineAsync(AppSearchEngineEnum fromEngine)
        {
            var engine = fromEngine == AppSearchEngineEnum.Google ? DbSearchEngineEnum.Google : DbSearchEngineEnum.Bing;
            var googleRank = await SeoRankData.Where(e => e.Engine == engine).OrderByDescending(e => e.Id).FirstOrDefaultAsync();

            return new SearchRankData(fromEngine, googleRank.Keyword, googleRank.CompanyUrl)
            {
                Ranks = googleRank.Ranks.Order(),
                RecordedAtUTC = googleRank.CreatedAtUtc
            };
        }

        public async Task UpdateRankDataByEngineAsync(AppSearchEngineEnum engine, SearchRankData data)
        {
            var entry = new SeoRankData()
            {
                CreatedAtUtc = DateTime.UtcNow,
                Engine = data.Engine == AppSearchEngineEnum.Google ? Enums.DbSearchEngineEnum.Google : Enums.DbSearchEngineEnum.Bing,
                Ranks = data.Ranks.ToArray(),
                Keyword = data.Keyword,
                CompanyUrl = data.CompanyUrl,
            };

            await SeoRankData.AddAsync(entry);
            await SaveChangesAsync();
        }

        public async Task<IEnumerable<SearchRankData>> GetLatestKeywordsRankDataByEngineAsync(AppSearchEngineEnum fromEngine)
        {
            var engine = fromEngine == AppSearchEngineEnum.Google ? DbSearchEngineEnum.Google : DbSearchEngineEnum.Bing;
            var googleRank = await SeoRankData.Where(e => e.Engine == engine)
                .GroupBy(e => e.Keyword)
                .Select(e => e.OrderByDescending(i => i.Id).First())
                .ToArrayAsync();

            if (googleRank == null || !googleRank.Any())
            {
                return Enumerable.Empty<SearchRankData>();
            }

            return googleRank.Select(e => new SearchRankData(fromEngine, e.Keyword, e.CompanyUrl)
            {
                RecordedAtUTC = e.CreatedAtUtc,
                Ranks = e.Ranks.Order()
            });
        }

        public virtual DbSet<SeoRankData> SeoRankData { get; set; }
    }
}
