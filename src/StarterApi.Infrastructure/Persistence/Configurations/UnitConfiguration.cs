using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StarterApi.Domain.Entities;

namespace StarterApi.Infrastructure.Persistence.Configurations
{
    public class UnitConfiguration : BaseConfiguration<Unit>
    {
        public override void Configure(EntityTypeBuilder<Unit> builder)
        {
            base.Configure(builder);
            
            builder.ToTable("Units");
            
            builder.Property(u => u.UnitNumber)
                .IsRequired()
                .HasMaxLength(10);
                
            builder.Property(u => u.Type)
                .IsRequired()
                .HasMaxLength(20);
                
            builder.Property(u => u.BuiltUpArea)
                .HasPrecision(18, 2);
                
            builder.Property(u => u.CarpetArea)
                .HasPrecision(18, 2);
                
            builder.Property(u => u.MonthlyMaintenanceFee)
                .HasPrecision(18, 2);
                
            builder.Property(u => u.FurnishingStatus)
                .IsRequired()
                .HasMaxLength(20);
                
            builder.Property(u => u.Status)
                .IsRequired()
                .HasMaxLength(20);
                
            builder.HasOne(u => u.Floor)
                .WithMany(f => f.Units)
                .HasForeignKey(u => u.FloorId)
                .OnDelete(DeleteBehavior.Restrict);
                
            builder.HasOne(u => u.CurrentOwner)
                .WithMany()
                .HasForeignKey(u => u.CurrentOwnerId)
                .OnDelete(DeleteBehavior.Restrict);
                
            builder.HasMany(u => u.Residents)
                .WithOne(r => r.Unit)
                .HasForeignKey(r => r.UnitId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
} 