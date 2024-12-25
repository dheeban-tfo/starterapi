using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StarterApi.Domain.Entities;

namespace StarterApi.Infrastructure.Persistence.Configurations
{
    public class DocumentAccessConfiguration : BaseConfiguration<DocumentAccess>
    {
        public override void Configure(EntityTypeBuilder<DocumentAccess> builder)
        {
            base.Configure(builder);

            builder.ToTable("DocumentAccesses");

            builder.Property(a => a.AccessLevel)
                .IsRequired()
                .HasMaxLength(20);

            // Relationships
            builder.HasOne(a => a.Document)
                .WithMany(d => d.AccessControls)
                .HasForeignKey(a => a.DocumentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.User)
                .WithMany()
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Unique constraint for Document + User
            builder.HasIndex(a => new { a.DocumentId, a.UserId })
                .IsUnique();
        }
    }
}
