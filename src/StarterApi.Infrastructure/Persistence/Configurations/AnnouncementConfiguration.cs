using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StarterApi.Domain.Common;
using StarterApi.Domain.Entities;

namespace StarterApi.Infrastructure.Persistence.Configurations
{
    public class AnnouncementConfiguration : BaseConfiguration<Announcement>
    {
        public override void Configure(EntityTypeBuilder<Announcement> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(e => e.Message)
                .IsRequired()
                .HasMaxLength(2000);

            builder.Property(e => e.Audience)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(e => e.Priority)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(e => e.PostedAt)
                .IsRequired();

            builder.Property(e => e.ExpirationDate);

            // Relationships
            builder.HasOne(e => e.Block)
                .WithMany()
                .HasForeignKey(e => e.BlockId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Unit)
                .WithMany()
                .HasForeignKey(e => e.UnitId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
} 