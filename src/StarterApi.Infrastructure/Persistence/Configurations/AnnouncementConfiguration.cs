using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StarterApi.Domain.Entities;

namespace StarterApi.Infrastructure.Persistence.Configurations
{
    public class AnnouncementConfiguration : BaseConfiguration<Announcement>
    {
        public void Configure(EntityTypeBuilder<Announcement> builder)
        {
            builder.ToTable("Announcements");
            
            builder.HasKey(a => a.Id);
            
            builder.Property(a => a.Title)
                .IsRequired()
                .HasMaxLength(200);
                
            builder.Property(a => a.Message)
                .IsRequired()
                .HasMaxLength(2000);
                
            builder.Property(a => a.Audience)
                .IsRequired()
                .HasMaxLength(50);
                
            builder.Property(a => a.Priority)
                .IsRequired()
                .HasMaxLength(20);

            builder.HasOne(a => a.Block)
                .WithMany()
                .HasForeignKey(a => a.BlockId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.Unit)
                .WithMany()
                .HasForeignKey(a => a.UnitId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
} 