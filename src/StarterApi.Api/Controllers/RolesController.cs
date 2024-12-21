using Microsoft.AspNetCore.Mvc;
using StarterApi.Application.Common.Interfaces;

using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using StarterApi.Domain.Constants;
using StarterApi.Application.Common.Extensions;

namespace StarterApi.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RolesController : ControllerBase
    {
        private readonly IUserTenantRepository _userTenantRepository;
        private readonly ITenantProvider _tenantProvider;
        private readonly IMapper _mapper;

        public RolesController(
            IUserTenantRepository userTenantRepository,
            ITenantProvider tenantProvider,
            IMapper mapper)
        {
            _userTenantRepository = userTenantRepository;
            _tenantProvider = tenantProvider;
            _mapper = mapper;
        }

        [HttpGet]
        [RequirePermission(Permissions.Roles.View)]
        public async Task<ActionResult<List<RoleDto>>> GetRoles()
        {
            var tenantId = _tenantProvider.GetCurrentTenantId();
            if (!tenantId.HasValue)
                return BadRequest("Tenant not specified");

            var roles = await _userTenantRepository.GetTenantRolesAsync(tenantId.Value);
            return Ok(_mapper.Map<List<RoleDto>>(roles));
        }

        [HttpGet("permissions")]
        [RequirePermission(Permissions.Roles.View)]
        public async Task<ActionResult<List<PermissionDto>>> GetPermissions()
        {
            var tenantId = _tenantProvider.GetCurrentTenantId();
            if (!tenantId.HasValue)
                return BadRequest("Tenant not specified");

            var permissions = await _userTenantRepository.GetTenantPermissionsAsync(tenantId.Value);
            return Ok(_mapper.Map<List<PermissionDto>>(permissions));
        }

        [HttpGet("my-permissions")]
        [Authorize]
        public async Task<ActionResult<List<string>>> GetMyPermissions()
        {
            var userId = User.GetUserId();
            var tenantId = _tenantProvider.GetCurrentTenantId();
            
            if (!tenantId.HasValue)
                return BadRequest("Tenant not specified");

            var permissions = await _userTenantRepository.GetUserPermissionsAsync(userId, tenantId.Value);
            return Ok(permissions);
        }
    }
}