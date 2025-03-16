using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodFlowSystem.Entities.Order
{
    public class OrderConfig : IEntityTypeConfiguration<OrderEntity>
    {
        public void Configure(EntityTypeBuilder<OrderEntity> builder)
        {
            builder.ToTable("Orders");
            builder.HasKey(x => x.ID);
            builder.Property(x => x.ID).ValueGeneratedOnAdd();

            builder.Property(x => x.TotalAmount)
                .HasColumnType("decimal(10,2)")
                .IsRequired(false);

            builder.Property(x => x.Status)
                .HasMaxLength(30)
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .HasColumnType("datetime")
                .IsRequired();

            builder.Property(x => x.ReservationID)
                .IsRequired(false);

            builder.Property(x => x.TableID)
                .IsRequired(false);

            builder.Property(x => x.UserID)
                .IsRequired();

            builder.HasOne(u => u.User)
                .WithMany(o => o.Orders)
                .HasForeignKey(o => o.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(r => r.Reservation)
                .WithMany(o => o.Orders)
                .HasForeignKey(o => o.ReservationID)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(p => p.Table)
                .WithMany(o => o.Orders)
                .HasForeignKey(o => o.TableID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
