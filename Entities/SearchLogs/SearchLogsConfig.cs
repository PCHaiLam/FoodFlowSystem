using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodFlowSystem.Entities.SearchLogs
{
    public class SearchLogsConfig : IEntityTypeConfiguration<SearchLogsEntity>
    {
        public void Configure(EntityTypeBuilder<SearchLogsEntity> builder)
        {
            builder.ToTable("SearchLogs");
            builder.HasKey(r => r.ID);
            builder.Property(r => r.ID).ValueGeneratedOnAdd();

            builder.Property(x => x.UserId).IsRequired(false);

            builder.Property(x => x.Keyword).IsRequired();
        }
    }
}
