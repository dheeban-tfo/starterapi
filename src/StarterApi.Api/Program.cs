using Microsoft.EntityFrameworkCore;
using StarterApi.Application.Modules.Tenants.Services;
using StarterApi.Infrastructure.Persistence.Contexts;
using StarterApi.Infrastructure.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Add DbContext
builder.Services.AddDbContext<RootDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("RootDatabase")));

// Register Services and Repositories
builder.Services.AddScoped<ITenantRepository, TenantRepository>();
builder.Services.AddScoped<ITenantService, TenantService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserTenantRepository, UserTenantRepository>();

// Register Services
builder.Services.AddScoped<TenantDbMigrationService>();
builder.Services.AddScoped<RootDataSeeder>();
builder.Services.AddScoped<ITenantDbMigrationService, TenantDbMigrationService>();

// Add HttpContextAccessor
builder.Services.AddHttpContextAccessor();

// Register tenant services
builder.Services.AddScoped<ITenantProvider, TenantProvider>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Add tenant resolution middleware before authorization
app.UseTenantResolution();

app.UseAuthorization();
app.MapControllers();

// Apply migrations and seed data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<RootDbContext>();
        await context.Database.MigrateAsync();

        var seeder = services.GetRequiredService<RootDataSeeder>();
        await seeder.SeedAsync();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating or seeding the database");
        throw;
    }
}

app.Run();
