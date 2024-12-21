using Microsoft.EntityFrameworkCore;
using StarterApi.Application.Common.Interfaces;
using StarterApi.Domain.Entities;

namespace StarterApi.Infrastructure.Persistence.Contexts
{
    public class TenantDbContext : DbContext, ITenantDbContext
    {
        private readonly string _connectionString;

        public DbSet<TenantUser> Users { get; set; }
        public DbSet<TenantRole> Roles { get; set; }

        public TenantDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TenantUser>(entity =>
            {
                entity.ToTable("Users");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FirstName).IsRequired();
                entity.Property(e => e.LastName).IsRequired();
                entity.Property(e => e.Email).IsRequired();
                entity.Property(e => e.MobileNumber).IsRequired();
            });

            modelBuilder.Entity<TenantRole>(entity =>
            {
                entity.ToTable("Roles");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired();
            });
        }
    }
}