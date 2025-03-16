using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodFlowSystem.Entities.Role
{
    public class RoleConfig : IEntityTypeConfiguration<RoleEntity>
    {
        public void Configure(EntityTypeBuilder<RoleEntity> builder)
        {
            builder.ToTable("Roles");
            builder.HasKey(r => r.ID);
            builder.Property(r => r.ID).ValueGeneratedOnAdd();

            builder.Property(r => r.RoleName)
                .HasMaxLength(30)
                .IsRequired();

            builder.Property(r => r.CreatedAt)
                .HasColumnType("datetime")
                .IsRequired();
        }
    }
}
