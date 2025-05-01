using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodFlowSystem.Entities.EmailTemplate
{
    public class EmailTemplatesConfig : IEntityTypeConfiguration<EmailTemplatesEntity>
    {
        public void Configure(EntityTypeBuilder<EmailTemplatesEntity> builder)
        {
            builder.ToTable("EmailTemplates");
            builder.HasKey(x => x.ID);
            builder.Property(x => x.ID).ValueGeneratedOnAdd();

            builder.Property(x => x.TemplateName)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.Subject)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(x => x.Body)
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .HasColumnType("datetime")
                .IsRequired();

            builder.Property(x => x.UpdatedAt)
                .HasColumnType("datetime")
                .IsRequired();
        }
    }
}
