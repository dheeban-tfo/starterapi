using StarterApi.Domain.Interfaces;
using StarterApi.Application.Common.Interfaces;


namespace StarterApi.Infrastructure.Services
{
    public class TenantInfo : ITenantInfo
    {
        private readonly ITenantProvider _tenantProvider;
        private readonly ITenantService _tenantService;
        private TenantInternalDto _currentTenant;

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
            if (_currentTenant != null)
                return _currentTenant;

            var tenantId = _tenantProvider.GetCurrentTenantId();
            if (!tenantId.HasValue)
                return null; // Return null instead of throwing

            var tenant = _tenantService.GetTenantByIdAsync(tenantId.Value).Result;
            if (tenant == null)
                return null; // Return null instead of throwing

            _currentTenant = new TenantInternalDto
            {
                Id = tenant.Id,
                Name = tenant.Name,
                DatabaseName = tenant.DatabaseName,
                Status = tenant.Status.ToString(),
                ConnectionString = tenant.ConnectionString,
                CreatedAt = tenant.CreatedAt,
                UpdatedAt = tenant.UpdatedAt
            };

            return _currentTenant;
        }
    }
} 