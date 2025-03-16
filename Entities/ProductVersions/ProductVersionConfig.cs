using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodFlowSystem.Entities.ProductVersions
{
    public class ProductVersionConfig : IEntityTypeConfiguration<ProductVersionEntity>
    {
        public void Configure(EntityTypeBuilder<ProductVersionEntity> builder)
        {
            builder.ToTable("ProductVersions");
            builder.HasKey(x => x.ID);
            builder.Property(x => x.ID).ValueGeneratedOnAdd();

            builder.Property(x => x.Price)
                .HasColumnType("decimal(10,2)")
                .IsRequired();

            builder.Property(x => x.EffectiveDate)
                .HasColumnType("datetime")
                .IsRequired();

            builder.Property(x => x.IsActive)
                .IsRequired();

            builder.Property(x => x.ProductID)
                .IsRequired();

            builder.HasOne(x => x.Product)
                .WithMany(x => x.ProductVersions)
                .HasForeignKey(x => x.ProductID)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
