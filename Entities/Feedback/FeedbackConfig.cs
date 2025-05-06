using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodFlowSystem.Entities.Feedback
{
    public class FeedbackConfig : IEntityTypeConfiguration<FeedbackEntity>
    {
        public void Configure(EntityTypeBuilder<FeedbackEntity> builder)
        {
            builder.ToTable("Feedbacks");
            builder.HasKey(f => f.ID);
            builder.Property(f => f.ID).ValueGeneratedOnAdd();

            builder.Property(f => f.Rating)
                .IsRequired();

            builder.Property(f => f.Comment);

            builder.Property(f => f.CreatedAt)
                .HasColumnType("datetime")
                .IsRequired();

            builder.Property(f => f.ProductID)
                .IsRequired();

            builder.Property(f => f.UserID)
                .IsRequired();

            builder.Property(f => f.OrderID)
                .IsRequired(false);

            builder.HasOne(o => o.Product)
                .WithMany(f => f.Feedbacks)
                .HasForeignKey(f => f.ProductID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(u => u.User)
                .WithMany(f => f.Feedbacks)
                .HasForeignKey(f => f.UserID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
