using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StarterApi.Domain.Entities;

namespace StarterApi.Infrastructure.Persistence.Configurations
{
    public class NotificationConfiguration : BaseConfiguration<Notification>
    {
        public override void Configure(EntityTypeBuilder<Notification> builder)
        {
            base.Configure(builder);
            
            builder.ToTable("Notifications");
            
            builder.HasKey(n => n.Id);
            
            builder.Property(n => n.Message)
                .IsRequired()
                .HasMaxLength(1000);
                
            builder.Property(n => n.Type)
                .IsRequired()
                .HasMaxLength(50);
                
            builder.Property(n => n.Status)
                .IsRequired()
                .HasMaxLength(20);
                
            builder.Property(n => n.NotificationType)
                .IsRequired()
                .HasMaxLength(50);
                
            builder.Property(n => n.ReferenceId)
                .HasMaxLength(100);
                
            builder.Property(n => n.ReferenceType)
                .HasMaxLength(50);

            builder.HasOne(n => n.Recipient)
                .WithMany()
                .HasForeignKey(n => n.RecipientId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
} 