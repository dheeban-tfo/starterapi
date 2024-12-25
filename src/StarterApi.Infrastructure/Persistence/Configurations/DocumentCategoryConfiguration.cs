using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StarterApi.Domain.Entities;

namespace StarterApi.Infrastructure.Persistence.Configurations
{
    public class DocumentCategoryConfiguration : BaseConfiguration<DocumentCategory>
    {
        public override void Configure(EntityTypeBuilder<DocumentCategory> builder)
        {
            base.Configure(builder);

            builder.ToTable("DocumentCategories");

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.Description)
                .HasMaxLength(500);

            // Relationships
            builder.HasOne(c => c.ParentCategory)
                .WithMany(c => c.ChildCategories)
                .HasForeignKey(c => c.ParentCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Prevent circular references in hierarchical structure
            builder.HasCheckConstraint("CK_DocumentCategory_NoSelfReference", 
                "ParentCategoryId != Id OR ParentCategoryId IS NULL");
        }
    }
}
