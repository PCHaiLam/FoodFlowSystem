using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodFlowSystem.Entities.Product
{
    public class ProductConfig : IEntityTypeConfiguration<ProductEntity>
    {
        public void Configure(EntityTypeBuilder<ProductEntity> builder)
        {
            builder.ToTable("Products");
            builder.HasKey(p => p.ID);
            builder.Property(p => p.ID).ValueGeneratedOnAdd();

            builder.Property(p => p.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder.HasIndex(p => p.Name)
                .IsUnique();

            builder.Property(p => p.Description)
                .IsRequired(false);

            builder.Property(p => p.Quantity)
                .IsRequired();

            builder.Property(p => p.ImageUrl)
                .HasMaxLength(255);

            builder.Property(p => p.CreatedAt)
                .HasColumnType("datetime")
                .IsRequired();

            builder.HasOne(c => c.Category)
                .WithMany(p => p.Products)
                .HasForeignKey(p => p.CategoryID)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
