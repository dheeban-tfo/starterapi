using Microsoft.AspNetCore.Http;
using StarterApi.Application.Common.Interfaces;

namespace StarterApi.Infrastructure.Services
{
    public class TenantProvider : ITenantProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TenantProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid? GetCurrentTenantId()
        {
            return _httpContextAccessor.HttpContext?.Items["TenantId"] as Guid?;
        }

        public void SetCurrentTenantId(Guid tenantId)
        {
            if (_httpContextAccessor.HttpContext != null)
            {
                _httpContextAccessor.HttpContext.Items["TenantId"] = tenantId;
            }
        }
    }
} 