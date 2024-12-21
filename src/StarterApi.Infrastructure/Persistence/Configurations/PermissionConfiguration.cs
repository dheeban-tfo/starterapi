using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StarterApi.Domain.Entities;

namespace StarterApi.Infrastructure.Persistence.Configurations
{
    public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.SystemName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.Description)
                .HasMaxLength(500);

            builder.Property(p => p.Group)
                .IsRequired()
                .HasMaxLength(50);

            // Ensure SystemName is unique
            builder.HasIndex(p => p.SystemName)
                .IsUnique();
        }
    }
} 