using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodFlowSystem.Entities.Invoice
{
    public class InvoiceConfig : IEntityTypeConfiguration<InvoiceEntity>
    {
        public void Configure(EntityTypeBuilder<InvoiceEntity> builder)
        {
            builder.ToTable("Invoices");
            builder.HasKey(i => i.ID);
            builder.Property(i => i.ID).ValueGeneratedOnAdd();

            builder.Property(i => i.Discount)
                .HasColumnType("decimal(10,2)")
                .IsRequired();

            builder.Property(i => i.TotalAmount)
                .HasColumnType("decimal(10,2)")
                .IsRequired();

            builder.Property(i => i.OrderID)
                .IsRequired();

            builder.Property(i => i.CreatedAt)
                .HasColumnType("datetime")
                .IsRequired();

            builder.HasOne(o => o.Order)
                .WithMany(i => i.Invoices)
                .HasForeignKey(i => i.OrderID)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
