using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodFlowSystem.Entities.Payment
{
    public class PaymentConfig : IEntityTypeConfiguration<PaymentEntity>
    {
        public void Configure(EntityTypeBuilder<PaymentEntity> builder)
        {
            builder.ToTable("Payments");
            builder.HasKey(p => p.ID);
            builder.Property(p => p.ID).ValueGeneratedOnAdd();

            builder.Property(p => p.Amount)
                .HasColumnType("decimal(10,2)")
                .IsRequired();

            builder.Property(p => p.PaymentMethod)
                .HasMaxLength(30)
                .IsRequired();

            builder.Property(p => p.CreatedAt)
                .HasColumnType("datetime")
                .IsRequired();

            builder.Property(p => p.OrderID)
                .IsRequired();

            builder.HasOne(o => o.Order)
                .WithMany(p => p.Payments)
                .HasForeignKey(p => p.OrderID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
