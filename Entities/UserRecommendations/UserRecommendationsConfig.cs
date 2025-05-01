using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodFlowSystem.Entities.UserRecommendations
{
    public class UserRecommendationsConfig : IEntityTypeConfiguration<UserRecommendationsEntity>
    {
        public void Configure(EntityTypeBuilder<UserRecommendationsEntity> builder)
        {
            builder.ToTable("UserRecommendations");
            builder.HasKey(x => x.ID);
            builder.Property(x => x.ID).ValueGeneratedOnAdd();

            builder.Property(x => x.UserId)
                .IsRequired();

            builder.Property(x => x.ProductId)
                .IsRequired();

            builder.Property(x => x.Score)
                .IsRequired();

            builder.HasOne(x => x.User)
                .WithMany(x => x.UserRecommendations)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Product)
                .WithMany(x => x.UserRecommendations)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
