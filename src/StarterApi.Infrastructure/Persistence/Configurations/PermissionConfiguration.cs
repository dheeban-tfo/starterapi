using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StarterApi.Domain.Common;
using StarterApi.Domain.Entities;

namespace StarterApi.Infrastructure.Persistence.Configurations
{
    public class PermissionConfiguration : BaseConfiguration<Permission>
    {
        public override void Configure(EntityTypeBuilder<Permission> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.SystemName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.Group)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(e => e.Description)
                .HasMaxLength(500);

            // Relationships
            builder.HasMany(e => e.RolePermissions)
                .WithOne(rp => rp.Permission)
                .HasForeignKey(rp => rp.PermissionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
} 