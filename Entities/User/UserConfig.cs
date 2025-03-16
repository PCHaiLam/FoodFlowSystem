using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodFlowSystem.Entities.User
{
    public class UserConfig : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(x => x.ID);
            builder.Property(x => x.ID).ValueGeneratedOnAdd();

            builder.Property(x => x.LastName)
                .HasMaxLength(50)
                .IsRequired(false);

            builder.Property(u => u.FirstName)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(u => u.Email)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(u => u.HashPassword)
                .HasMaxLength(255)
                .IsRequired(false);

            builder.Property(u => u.Phone)
                .HasMaxLength(15)
                .IsRequired(false);

            builder.Property(u => u.PhotoUrl)
                .IsRequired(false);

            builder.Property(u => u.CreatedAt)
                .HasColumnType("datetime")
                .IsRequired();

            builder.Property(u => u.RoleID)
                .IsRequired();

            builder.HasOne(r => r.Role)
                .WithMany(u => u.Users)
                .HasForeignKey(u => u.RoleID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
