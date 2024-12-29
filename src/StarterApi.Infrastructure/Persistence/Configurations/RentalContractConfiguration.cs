using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StarterApi.Domain.Common;
using StarterApi.Domain.Entities;

namespace StarterApi.Infrastructure.Persistence.Configurations
{
    public class RentalContractConfiguration : BaseConfiguration<RentalContract>
    {
        public override void Configure(EntityTypeBuilder<RentalContract> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.StartDate)
                .IsRequired();

            builder.Property(e => e.EndDate)
                .IsRequired();

            builder.Property(e => e.RentAmount)
                .IsRequired()
                .HasPrecision(18, 2);

            builder.Property(e => e.SecurityDeposit)
                .IsRequired()
                .HasPrecision(18, 2);

            builder.Property(e => e.PaymentFrequency)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(e => e.PaymentMode)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(e => e.Terms)
                .HasMaxLength(4000);

            // Relationships
            builder.HasOne(e => e.Unit)
                .WithMany()
                .HasForeignKey(e => e.UnitId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Tenant)
                .WithMany()
                .HasForeignKey(e => e.TenantId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
} 