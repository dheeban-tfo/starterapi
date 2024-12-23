using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StarterApi.Domain.Entities;

namespace StarterApi.Infrastructure.Persistence.Configurations
{
    public class RolePermissionConfiguration : BaseConfiguration<RolePermission>
    {
        public override void Configure(EntityTypeBuilder<RolePermission> builder)
        {
            base.Configure(builder);
            
            builder.HasKey(rp => new { rp.RoleId, rp.PermissionId });
            
            // Configure relationship with Role
            builder.HasOne(rp => rp.Role)
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(rp => rp.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure relationship with Permission
            builder.HasOne(rp => rp.Permission)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(rp => rp.PermissionId)
                .OnDelete(DeleteBehavior.Restrict);

            // Add created/updated audit fields
            builder.Property(rp => rp.CreatedAt)
                .IsRequired();

            builder.Property(rp => rp.UpdatedAt)
                .IsRequired(false);
        }
    }
} 