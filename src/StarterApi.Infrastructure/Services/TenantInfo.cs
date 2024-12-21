using StarterApi.Domain.Interfaces;
using StarterApi.Application.Common.Interfaces;


namespace StarterApi.Infrastructure.Services
{
    public class TenantInfo : ITenantInfo
    {
        private readonly ITenantProvider _tenantProvider;
        private readonly ITenantService _tenantService;

        public TenantInfo(
            ITenantProvider tenantProvider,
            ITenantService tenantService)
        {
            _tenantProvider = tenantProvider;
            _tenantService = tenantService;
        }

        public Guid Id => _tenantProvider.GetCurrentTenantId() ?? 
            throw new InvalidOperationException("No tenant ID found in context");

        public string Name => GetCurrentTenant().Name;

        public string ConnectionString => GetCurrentTenant().ConnectionString;

        private TenantInternalDto GetCurrentTenant()
        {
            var tenantId = _tenantProvider.GetCurrentTenantId() ?? 
                throw new InvalidOperationException("No tenant ID found in context");
            
            var tenant = _tenantService.GetTenantByIdAsync(tenantId).Result;
            if (tenant == null)
                throw new InvalidOperationException($"Tenant with ID {tenantId} not found");

            // Manual mapping
            return new TenantInternalDto
            {
                Id = tenant.Id,
                Name = tenant.Name,
                DatabaseName = tenant.DatabaseName,
                Status = tenant.Status.ToString(),
                ConnectionString = tenant.ConnectionString,
                CreatedAt = tenant.CreatedAt,
                UpdatedAt = tenant.UpdatedAt
            };
        }
    }
} 