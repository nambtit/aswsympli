using Infrastructure.DB.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.DB.EntityTypeConfig
{
    internal class SeoRankDataETC : IEntityTypeConfiguration<SeoRankData>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<SeoRankData> builder)
        {
            builder.Property(e => e.CreatedAtUtc).IsRequired().HasDefaultValue(DateTime.UtcNow);
            builder.Property(e => e.CompanyUrl).IsRequired().HasMaxLength(256);
            builder.Property(e => e.Keyword).IsRequired().HasMaxLength(256);
            builder.Property(e => e.Engine).IsRequired().HasConversion<string>();

            var rankConverter = new ValueConverter<ICollection<int>, string>(
                v => string.Join(",", v),
                v => Array.ConvertAll(v.Split(",", StringSplitOptions.RemoveEmptyEntries), c => int.Parse(c))
            );

            builder.Property(e => e.Ranks).IsRequired().HasConversion(rankConverter);
        }
    }
}
