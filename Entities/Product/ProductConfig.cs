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

            builder.Property(p => p.Price)
                .HasColumnType("decimal(10,2)")
                .IsRequired();

            builder.Property(p => p.Status)
                .HasMaxLength(30)
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
