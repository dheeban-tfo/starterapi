using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using StarterApi.Infrastructure.Persistence.Contexts;

public class RootDbContextFactory : IDesignTimeDbContextFactory<RootDbContext>
{
    public RootDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<RootDbContext>();
        optionsBuilder.UseSqlServer("Server=localhost;Database=StarterApi_Root;User Id=sa;Password=MyPass@word;TrustServerCertificate=True;MultipleActiveResultSets=true;");

        return new RootDbContext(optionsBuilder.Options);
    }
} 