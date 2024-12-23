using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StarterApi.Domain.Entities;

namespace StarterApi.Infrastructure.Persistence.Configurations
{
    public class BlockConfiguration : BaseConfiguration<Block>
    {
        public override void Configure(EntityTypeBuilder<Block> builder)
        {
            base.Configure(builder);
            
            builder.ToTable("Blocks");
            
            builder.Property(b => b.Name)
                .IsRequired()
                .HasMaxLength(50);
                
            builder.Property(b => b.Code)
                .IsRequired()
                .HasMaxLength(10);
                
            builder.Property(b => b.MaintenanceChargePerSqft)
                .HasPrecision(18, 2);
                
            builder.HasMany(b => b.Floors)
                .WithOne(f => f.Block)
                .HasForeignKey(f => f.BlockId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
} 