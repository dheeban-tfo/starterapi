using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StarterApi.Domain.Entities;

namespace StarterApi.Infrastructure.Persistence.Configurations
{
    public class RoleConfiguration : BaseConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(r => r.Description)
                .HasMaxLength(500);

        

            // Ensure Role names are unique per tenant
            builder.HasIndex(r => new { r.TenantId, r.Name })
                .IsUnique();

            // Configure relationship with Tenant
            builder.HasOne(r => r.Tenant)
                .WithMany()
                .HasForeignKey(r => r.TenantId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
} 