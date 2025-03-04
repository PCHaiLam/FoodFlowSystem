using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodFlowSystem.Entities.Reservation
{
    public class ReservationConfig : IEntityTypeConfiguration<ReservationEntity>
    {
        public void Configure(EntityTypeBuilder<ReservationEntity> builder)
        {
            builder.ToTable("Reservations");
            builder.HasKey(r => r.ID);
            builder.Property(r => r.ID).ValueGeneratedOnAdd();

            builder.Property(r => r.NumberOfGuests)
                .IsRequired();

            builder.Property(r => r.Status)
                .HasMaxLength(30)
                .IsRequired();

            builder.Property(r => r.CreatedAt)
                .HasColumnType("datetime")
                .IsRequired();

            builder.Property(r => r.TableID)
                .IsRequired();

            builder.HasOne(u => u.User)
                .WithMany(r => r.Reservations)
                .HasForeignKey(r => r.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(t => t.Table)
                .WithMany(r => r.Reservations)
                .HasForeignKey(r => r.TableID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
