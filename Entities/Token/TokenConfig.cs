using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodFlowSystem.Entities.Token
{
    public class TokenConfig : IEntityTypeConfiguration<TokenEntity>
    {
        public void Configure(EntityTypeBuilder<TokenEntity> builder)
        {
            builder.ToTable("Tokens");
            builder.HasKey(x => x.ID);
            builder.Property(x => x.ID).ValueGeneratedOnAdd();

            builder.Property(x => x.AccessToken)
                .IsRequired();

            builder.Property(x => x.RefreshToken)
                .IsRequired();

            builder.Property(x => x.ExpiresAt)
                .HasColumnType("datetime")
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .HasColumnType("datetime")
                .IsRequired();

            builder.Property(x => x.UpdatedAt)
                .HasColumnType("datetime")
                .IsRequired();

            builder.Property(x => x.IsRevoked)
                .HasDefaultValue(false)
                .IsRequired();

            builder.HasOne(t => t.User)
                .WithMany()
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
} 