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
using StarterApi.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using System.Reflection;
using StarterApi.Application.Modules.Roles.Interfaces;
using StarterApi.Application.Modules.Roles.Services;


var builder = WebApplication.CreateBuilder(args);

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "StarterApi",
        Version = "v1",
        Description = "API for StarterApi with JWT Authentication"
    });

    // Add JWT Bearer Authentication
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer {token}'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    // Add Security Requirement
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });

    // Add XML comments if they exist
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
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

// Register services with correct lifetimes
builder.Services.AddSingleton<ITokenService, TokenService>();        // Handles token operations
builder.Services.AddSingleton<IJwtService, JwtService>();           // Handles base token and validation
builder.Services.AddScoped<ITenantTokenService, TenantTokenService>(); // Handles tenant-specific tokens
builder.Services.AddScoped<IAuthService, AuthService>();            // Handles authentication flow
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

// Add JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]))
    };
});

// Add Authorization with Policies
builder.Services.AddAuthorization(options =>
{
    // Add policies for each permission
    foreach (var property in typeof(Permissions).GetNestedTypes()
        .SelectMany(t => t.GetFields())
        .Where(f => f.IsStatic))
    {
        var permission = property.GetValue(null)?.ToString();
        if (!string.IsNullOrEmpty(permission))
        {
           // logger.LogInformation("Registering policy for permission: {Permission}", permission);
            options.AddPolicy($"Permission_{permission}", policy =>
                policy.Requirements.Add(new PermissionRequirement(permission)));
        }
    }
});

// Register the authorization handler
builder.Services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

// Add Role services
// builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IRoleService, RoleService>();

// Add HttpContextAccessor
builder.Services.AddHttpContextAccessor();

// Add CurrentUserService
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

// Register interfaces
builder.Services.AddScoped<IRootDbContext, RootDbContext>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Enable CORS
app.UseCors();

// First JWT authorization to validate token and set claims
app.UseMiddleware<JwtAuthorizationMiddleware>();

// Then tenant resolution using the claims
app.UseMiddleware<TenantResolutionMiddleware>();

// Then built-in auth middleware
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
