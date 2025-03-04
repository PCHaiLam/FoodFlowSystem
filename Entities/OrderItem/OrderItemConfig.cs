using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodFlowSystem.Entities.OrderItem
{
    public class OrderItemConfig : IEntityTypeConfiguration<OrderItemEntity>
    {
        public void Configure(EntityTypeBuilder<OrderItemEntity> builder)
        {
            builder.ToTable("OrderItems");
            builder.HasKey(oi => oi.ID);
            builder.Property(oi => oi.ID).ValueGeneratedOnAdd();

            builder.Property(oi => oi.Quantity)
                .IsRequired();

            builder.Property(oi => oi.Price)
                .HasColumnType("decimal(10,2)")
                .IsRequired();

            builder.Property(oi => oi.CreatedAt)
                .HasColumnType("datetime")
                .IsRequired();

            builder.Property(oi => oi.OrderID)
                .IsRequired();

            builder.Property(oi => oi.ProductID)
                .IsRequired();

            builder.HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(oi => oi.Product)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(oi => oi.ProductID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
