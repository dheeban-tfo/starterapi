using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StarterApi.Domain.Entities;

namespace StarterApi.Infrastructure.Persistence.Configurations
{
    public class RentalContractConfiguration : BaseConfiguration<RentalContract>
    {
        public void Configure(EntityTypeBuilder<RentalContract> builder)
        {
            builder.ToTable("RentalContracts");
            
            builder.HasKey(rc => rc.Id);
            
            builder.Property(rc => rc.RentAmount)
                .HasPrecision(18, 2);
                
            builder.Property(rc => rc.SecurityDeposit)
                .HasPrecision(18, 2);
                
            builder.Property(rc => rc.PaymentFrequency)
                .IsRequired()
                .HasMaxLength(20);
                
            builder.Property(rc => rc.PaymentMode)
                .IsRequired()
                .HasMaxLength(20);
                
            builder.Property(rc => rc.Terms)
                .HasMaxLength(4000);

            builder.HasOne(rc => rc.Unit)
                .WithMany()
                .HasForeignKey(rc => rc.UnitId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(rc => rc.Tenant)
                .WithMany()
                .HasForeignKey(rc => rc.TenantId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
} 