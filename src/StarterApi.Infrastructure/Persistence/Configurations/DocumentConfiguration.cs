using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StarterApi.Domain.Entities;

namespace StarterApi.Infrastructure.Persistence.Configurations
{
    public class DocumentConfiguration : BaseConfiguration<Document>
    {
        public override void Configure(EntityTypeBuilder<Document> builder)
        {
            base.Configure(builder);

            builder.ToTable("Documents");

            builder.Property(d => d.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(d => d.Description)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(d => d.BlobUrl)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(d => d.BlobPath)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(d => d.FileType)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(d => d.ContentType)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(d => d.Size)
                .IsRequired();

            builder.Property(d => d.Category)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(d => d.CurrentVersion)
                .IsRequired()
                .HasDefaultValue(1);

            // Relationships
            builder.HasOne(d => d.DocumentCategory)
                .WithMany(c => c.Documents)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(d => d.Block)
                .WithMany()
                .HasForeignKey(d => d.BlockId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(d => d.Unit)
                .WithMany()
                .HasForeignKey(d => d.UnitId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
