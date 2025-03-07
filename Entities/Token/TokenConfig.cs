using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodFlowSystem.Entities.Token
{
    public class TokenConfig : IEntityTypeConfiguration<TokenEntity>
    {
        public void Configure(EntityTypeBuilder<TokenEntity> builder)
        {
            builder.ToTable("Tokens");
            builder.HasKey(t => t.ID);
            builder.Property(t => t.ID).ValueGeneratedOnAdd();

            builder.Property(t => t.AccessToken)
                .IsRequired();

            builder.Property(t => t.RefreshToken)
                .IsRequired();

            builder.Property(t => t.ExpireAt)
                .HasColumnType("datetime")
                .IsRequired();

            builder.Property(t => t.CreatedAt)
                .HasColumnType("datetime")
                .IsRequired();

            builder.Property(t => t.UserID)
                .IsRequired();

            builder.HasOne(u => u.User)
                .WithMany(t => t.Tokens)
                .HasForeignKey(t => t.UserID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
