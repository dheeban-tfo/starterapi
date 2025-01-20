using Microsoft.Extensions.DependencyInjection;
using StarterApi.Application.Modules.Visitors.Interfaces;
using StarterApi.Application.Modules.Visitors.Services;
using StarterApi.Application.Modules.Visitors.Mappings;
using StarterApi.Infrastructure.Persistence.Repositories;

namespace StarterApi.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Register AutoMapper
            services.AddAutoMapper(typeof(VisitorMappingProfile).Assembly);

            // Register Services
            services.AddScoped<IVisitorService, VisitorService>();

            // Register Repositories
            services.AddScoped<IVisitorRepository, VisitorRepository>();

            // ... existing registrations ...

            return services;
        }
    }
} 