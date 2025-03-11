using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodFlowSystem.Entities.AuditLog
{
    public class AuditLogConfig : IEntityTypeConfiguration<AuditLogEntity>
    {
        public void Configure(EntityTypeBuilder<AuditLogEntity> builder)
        {
            builder.ToTable("AuditLogs");
            builder.HasKey(x => x.ID);
            builder.Property(x => x.ID).ValueGeneratedOnAdd();

            builder.Property(x => x.Action)
                .HasMaxLength(30)
                .IsRequired();

            builder.Property(x => x.TableName)
                .HasMaxLength(30)
                .IsRequired();

            builder.Property(x => x.RecordID)
                .HasMaxLength(30)
                .IsRequired(false);

            builder.Property(x => x.OldValue)
                .IsRequired(false);

            builder.Property(x => x.NewValue)
                .IsRequired(false);

            builder.Property(x => x.IPAddress)
                .HasMaxLength(50)
                .IsRequired(false);

            builder.Property(x => x.UserAgent)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .HasColumnType("datetime")
                .IsRequired();

            builder.HasOne(u => u.User)
                .WithMany(a => a.AuditLogs)
                .HasForeignKey(a => a.UserID)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
