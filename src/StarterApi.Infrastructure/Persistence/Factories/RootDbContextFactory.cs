using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using StarterApi.Infrastructure.Persistence.Contexts;

public class RootDbContextFactory : IDesignTimeDbContextFactory<RootDbContext>
{
    public RootDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<RootDbContext>();
        optionsBuilder.UseSqlServer("Server=localhost;Database=StarterApi_Root;Trusted_Connection=True;TrustServerCertificate=True;");

        return new RootDbContext(optionsBuilder.Options);
    }
} 