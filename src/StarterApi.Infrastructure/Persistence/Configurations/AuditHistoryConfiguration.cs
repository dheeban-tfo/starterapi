using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StarterApi.Domain.Entities;

namespace StarterApi.Infrastructure.Persistence.Configurations
{
    public class AuditHistoryConfiguration : BaseConfiguration<AuditHistory>
    {
        public override void Configure(EntityTypeBuilder<AuditHistory> builder)
        {
            base.Configure(builder);
            
            builder.ToTable("AuditHistory");
            
            builder.HasKey(ah => ah.Id);
            
            builder.Property(ah => ah.TableName)
                .IsRequired()
                .HasMaxLength(100);
                
            builder.Property(ah => ah.RecordId)
                .IsRequired()
                .HasMaxLength(100);
                
            builder.Property(ah => ah.Action)
                .IsRequired()
                .HasMaxLength(20);
                
            builder.Property(ah => ah.OldValue)
                .HasMaxLength(4000);
                
            builder.Property(ah => ah.NewValue)
                .HasMaxLength(4000);
                
            builder.Property(ah => ah.IpAddress)
                .HasMaxLength(50);
                
            builder.Property(ah => ah.UserAgent)
                .HasMaxLength(500);
        }
    }
} 