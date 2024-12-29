using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StarterApi.Domain.Common;
using StarterApi.Domain.Entities;

namespace StarterApi.Infrastructure.Persistence.Configurations
{
    public class OwnershipHistoryConfiguration : BaseConfiguration<OwnershipHistory>
    {
        public override void Configure(EntityTypeBuilder<OwnershipHistory> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.TransferType)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(e => e.TransferDate)
                .IsRequired();

            builder.Property(e => e.TransferReason)
                .HasMaxLength(500);

            builder.Property(e => e.PreviousOwnerName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.TransferDocumentNumber)
                .HasMaxLength(50);

            builder.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(20);

            // Relationships
            builder.HasOne(e => e.Unit)
                .WithMany()
                .HasForeignKey(e => e.UnitId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Owner)
                .WithMany(o => o.OwnershipHistory)
                .HasForeignKey(e => e.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(e => e.Documents)
                .WithMany()
                .UsingEntity(j => j.ToTable("OwnershipHistoryDocuments"));
        }
    }
} 