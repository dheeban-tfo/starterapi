using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StarterApi.Domain.Entities;

namespace StarterApi.Infrastructure.Persistence.Configurations
{
    public class DocumentVersionConfiguration : BaseConfiguration<DocumentVersion>
    {
        public override void Configure(EntityTypeBuilder<DocumentVersion> builder)
        {
            base.Configure(builder);

            builder.ToTable("DocumentVersions");

            builder.Property(dv => dv.Version)
                .IsRequired();

            builder.Property(dv => dv.BlobUrl)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(dv => dv.BlobPath)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(dv => dv.ContentType)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(dv => dv.Size)
                .IsRequired();

            builder.Property(dv => dv.ChangeDescription)
                .HasMaxLength(500);

            // Relationships
            builder.HasOne(dv => dv.Document)
                .WithMany(d => d.Versions)
                .HasForeignKey(dv => dv.DocumentId)
                .OnDelete(DeleteBehavior.Cascade);

            // Unique constraint for DocumentId and Version
            builder.HasIndex(dv => new { dv.DocumentId, dv.Version })
                .IsUnique();
        }
    }
}
