using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodFlowSystem.Entities.Report
{
    public class ReportConfig : IEntityTypeConfiguration<ReportEntity>
    {
        public void Configure(EntityTypeBuilder<ReportEntity> builder)
        {
            builder.ToTable("Reports");
            builder.HasKey(r => r.ID);
            builder.Property(r => r.ID).ValueGeneratedOnAdd();

            builder.Property(r => r.Title)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(r => r.ReportType)
                .HasMaxLength(30)
                .IsRequired();

            builder.Property(r => r.ReportContent)
                .IsRequired();

            builder.Property(r => r.StartDate)
                .HasColumnType("datetime")
                .IsRequired();

            builder.Property(r => r.EndDate)
                .HasColumnType("datetime")
                .IsRequired();

            builder.Property(r => r.CreatedAt)
                .HasColumnType("datetime")
                .IsRequired();

            builder.HasOne(u => u.User)
                .WithMany(r => r.Reports)
                .HasForeignKey(r => r.UserID)
                .OnDelete(DeleteBehavior.SetNull);  
        }
    }
}
