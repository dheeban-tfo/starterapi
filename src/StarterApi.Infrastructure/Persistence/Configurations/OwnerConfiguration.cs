using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StarterApi.Domain.Entities;

namespace StarterApi.Infrastructure.Persistence.Configurations
{
    public class OwnerConfiguration : BaseConfiguration<Owner>
    {
        public override void Configure(EntityTypeBuilder<Owner> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.OwnershipType)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(e => e.OwnershipPercentage)
                .IsRequired()
                .HasPrecision(5, 2);

            builder.Property(e => e.OwnershipStartDate)
                .IsRequired();

            builder.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(e => e.OwnershipDocumentNumber)
                .HasMaxLength(50);

            // Relationships
            builder.HasOne(e => e.Individual)
                .WithMany(i => i.Owners)
                .HasForeignKey(e => e.IndividualId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(e => e.Units)
                .WithOne(u => u.CurrentOwner)
                .HasForeignKey(u => u.CurrentOwnerId)
                .OnDelete(DeleteBehavior.Restrict);

           
        }
    }
} 