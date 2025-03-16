using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodFlowSystem.Entities.Table
{
    public class TableConfig : IEntityTypeConfiguration<TableEntity>
    {
        public void Configure(EntityTypeBuilder<TableEntity> builder)
        {
            builder.ToTable("Tables");
            builder.HasKey(t => t.ID);
            builder.Property(t => t.ID).ValueGeneratedOnAdd();

            builder.Property(t => t.Capacity)
                .IsRequired();

            builder.Property(t => t.Status)
                .HasMaxLength(30)
                .IsRequired();

            builder.Property(t => t.CreatedAt)
                .HasColumnType("datetime")
                .IsRequired();
        }
    }
}
