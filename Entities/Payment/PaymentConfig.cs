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

            builder.Property(p => p.PaymentType)
                .HasMaxLength(30)
                .IsRequired();

            builder.Property(p => p.PaymentMethod)
                .HasMaxLength(30)
                .IsRequired();

            builder.Property(p => p.CreatedAt)
                .HasColumnType("datetime")
                .IsRequired();

            builder.Property(p => p.Status)
                .HasMaxLength(30)
                .HasDefaultValue("Pending")
                .IsRequired();

            builder.Property(p => p.IsDeposit)
                .HasDefaultValue(false)
                .IsRequired();

            builder.Property(p => p.InvoiceId)
                .IsRequired();

            builder.HasOne(i => i.Invoice)
                .WithMany(p => p.Payments)
                .HasForeignKey(p => p.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
