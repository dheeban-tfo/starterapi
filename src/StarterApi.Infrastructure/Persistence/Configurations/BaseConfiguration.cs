using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace StarterApi.Infrastructure.Persistence.Configurations
{
    public abstract class BaseConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> 
        where TEntity : BaseEntity
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.CreatedBy)
                .IsRequired();

            builder.Property(e => e.CreatedAt)
                .IsRequired();

            builder.Property(e => e.UpdatedBy)
                .IsRequired(false);

            builder.Property(e => e.UpdatedAt)
                .IsRequired(false);

            builder.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValue(true);
        }
    }
} 