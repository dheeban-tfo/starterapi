using Microsoft.EntityFrameworkCore;
using StarterApi.Application.Common.Interfaces;
using StarterApi.Application.Modules.Tenants.Services;
using StarterApi.Application.Modules.Users.Services;
using StarterApi.Infrastructure.Persistence.Contexts;
using StarterApi.Infrastructure.Persistence.Repositories;

using StarterApi.Domain.Interfaces;
using Microsoft.OpenApi.Models;
using StarterApi.Infrastructure.Services;
using StarterApi.Application.Modules.Auth.Services;

using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using StarterApi.Application.Interfaces;
using StarterApi.Domain.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "StarterApi", Version = "v1" });
    
    // Add X-TenantId header parameter globally
    c.AddSecurityDefinition("TenantId", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.ApiKey,
        In = ParameterLocation.Header,
        Name = "X-TenantId",
        Description = "Tenant ID for multi-tenancy",
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "TenantId"
                }
            },
            new string[] {}
        }
    });
});

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Add DbContext
builder.Services.AddDbContext<RootDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("RootDatabase")));

// Add these services in order
builder.Services.AddHttpContextAccessor();

// Register repositories
builder.Services.AddScoped<ITenantRepository, TenantRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserTenantRepository, UserTenantRepository>();

// Register services
builder.Services.AddScoped<ITenantDbMigrationService, TenantDbMigrationService>();
builder.Services.AddScoped<ITenantService, TenantService>();
builder.Services.AddScoped<ITenantUserService, TenantUserService>();

// Register tenant-related services
builder.Services.AddScoped<ITenantProvider, TenantProvider>();
builder.Services.AddScoped<ITenantInfo, TenantInfo>();

// Add DbContext configurations
builder.Services.AddDbContext<RootDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("RootDatabase")));

// Add Scoped TenantDbContext
builder.Services.AddScoped<ITenantDbContext>(serviceProvider =>
{
    var tenantInfo = serviceProvider.GetRequiredService<ITenantInfo>();
    return new TenantDbContext(tenantInfo.ConnectionString);
});

// Add Data Seeders
builder.Services.AddScoped<RootDataSeeder>();

// Register auth services
builder.Services.AddScoped<IOtpRepository, OtpRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Add JWT Configuration
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

// Add Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]))
        };
    });

// Register JWT Service
builder.Services.AddScoped<IJwtService, JwtService>();

// Register RefreshToken Repository
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

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

app.UseAuthentication();
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
