using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StarterApi.Domain.Common;
using StarterApi.Domain.Entities;

namespace StarterApi.Infrastructure.Persistence.Configurations
{
    public class OwnershipTransferRequestConfiguration : BaseConfiguration<OwnershipTransferRequest>
    {
        public override void Configure(EntityTypeBuilder<OwnershipTransferRequest> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.TransferType)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(e => e.RequestDate)
                .IsRequired();

            builder.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(e => e.Comments)
                .HasMaxLength(500);

            builder.Property(e => e.RejectionReason)
                .HasMaxLength(500);

            // Relationships
            builder.HasOne(e => e.Unit)
                .WithMany()
                .HasForeignKey(e => e.UnitId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.CurrentOwner)
                .WithMany()
                .HasForeignKey(e => e.CurrentOwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.NewOwner)
                .WithMany()
                .HasForeignKey(e => e.NewOwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.ApprovedByUser)
                .WithMany()
                .HasForeignKey(e => e.ApprovedBy)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(e => e.SupportingDocuments)
                .WithMany()
                .UsingEntity(j => j.ToTable("OwnershipTransferDocuments"));
        }
    }
} 