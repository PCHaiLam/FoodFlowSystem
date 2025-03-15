using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodFlowSystem.Entities.OAuth
{
    public class OAuthConfig : IEntityTypeConfiguration<OAuthEntity>
    {
        public void Configure(EntityTypeBuilder<OAuthEntity> builder)
        {
            builder.ToTable("OAuths");
            builder.HasKey(x => x.ID);
            builder.Property(x => x.ID).ValueGeneratedOnAdd();

            builder.Property(x => x.UserId)
                .IsRequired();

            builder.Property(x => x.Provider)
                .IsRequired();

            builder.Property(x => x.ProviderUserId)
                .IsRequired();

            builder.Property(x => x.Email)
                .IsRequired();

            builder.Property(x => x.LastLoginAt)
                .IsRequired();

            builder.HasOne(x => x.User)
                .WithMany(x => x.OAuths)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
