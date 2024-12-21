using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using StarterApi.Application.Common.Exceptions;
using StarterApi.Application.Interfaces;
using StarterApi.Domain.Entities;

using StarterApi.Infrastructure.Services;

namespace StarterApi.Infrastructure.Services
{
    public class TenantTokenService : ITenantTokenService
    {
        private readonly ITokenService _tokenService;
        private readonly IUserTenantRepository _userTenantRepository;
        private readonly ITenantRepository _tenantRepository;

        public TenantTokenService(
            ITokenService tokenService,
            IUserTenantRepository userTenantRepository,
            ITenantRepository tenantRepository)
        {
            _tokenService = tokenService;
            _userTenantRepository = userTenantRepository;
            _tenantRepository = tenantRepository;
        }

        public async Task<string> GenerateTenantTokenAsync(User user, Guid tenantId)
        {
            var userTenant = await _userTenantRepository.GetByUserAndTenantIdAsync(user.Id, tenantId);
            if (userTenant == null)
                throw new UnauthorizedException("User does not have access to this tenant");

            var tenant = await _tenantRepository.GetByIdAsync(tenantId);
            
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Email, user.Email),
                new("tenant_id", tenantId.ToString()),
                new("tenant_name", tenant.Name),
                new("role_id", userTenant.RoleId.ToString()),
                new("token_type", "tenant_token")
            };

            return _tokenService.GenerateToken(claims);
        }
    }
} 