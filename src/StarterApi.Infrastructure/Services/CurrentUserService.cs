using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using StarterApi.Application.Common.Interfaces;

namespace StarterApi.Infrastructure.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid? UserId
        {
            get
            {
                var userId = _httpContextAccessor.HttpContext?.User?.Claims
                    .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                return userId != null ? Guid.Parse(userId) : null;
            }
        }

        public string? Email => _httpContextAccessor.HttpContext?.User?.Claims
            .FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

        public Guid? TenantId
        {
            get
            {
                var tenantId = _httpContextAccessor.HttpContext?.User?.Claims
                    .FirstOrDefault(c => c.Type == "tenant_id")?.Value;
                return tenantId != null ? Guid.Parse(tenantId) : null;
            }
        }

        public string? TenantName => _httpContextAccessor.HttpContext?.User?.Claims
            .FirstOrDefault(c => c.Type == "tenant_name")?.Value;

        public Guid? RoleId
        {
            get
            {
                var roleId = _httpContextAccessor.HttpContext?.User?.Claims
                    .FirstOrDefault(c => c.Type == "role_id")?.Value;
                return roleId != null ? Guid.Parse(roleId) : null;
            }
        }
    }
} 