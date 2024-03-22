using Application.Abstraction;
using Application.Features.SEORank.Enums;
using Application.Models;
using Infrastructure.DB.Entities;
using Infrastructure.DB.EntityTypeConfig;
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

        public Task<SearchRankData> GetRankDataByEngineAsync(AppSearchEngineEnum fromEngine)
        {
            throw new NotImplementedException();
        }

        public Task UpdateRankDataByEngineAsync(AppSearchEngineEnum engine, SearchRankData data)
        {
            throw new NotImplementedException();
        }

        public virtual DbSet<SeoRankData> SeoRankData { get; set; }
    }
}
