using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using StarterApi.Application.Modules.Roles.Interfaces;
using StarterApi.Domain.Constants;
using StarterApi.Application.Common.Interfaces;

namespace StarterApi.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly ILogger<RolesController> _logger;
        private readonly ICurrentUserService _currentUserService;
        private readonly ITenantProvider _tenantProvider;
        private readonly IUserTenantRepository _userTenantRepository;

        public RolesController(
            IRoleService roleService, 
            ICurrentUserService currentUserService,
            ITenantProvider tenantProvider,
            IUserTenantRepository userTenantRepository,
            ILogger<RolesController> logger)
        {
            _roleService = roleService;
            _currentUserService = currentUserService;
            _tenantProvider = tenantProvider;
            _userTenantRepository = userTenantRepository;
            _logger = logger;
        }

        [HttpGet]
        [RequirePermission(Permissions.Roles.View)]
        public async Task<ActionResult<List<RoleDto>>> GetRoles()
        {
            return Ok(await _roleService.GetRolesAsync());
        }

        [HttpGet("{id}")]
        [RequirePermission(Permissions.Roles.View)]
        public async Task<ActionResult<RoleDto>> GetRole(Guid id)
        {
            return Ok(await _roleService.GetRoleByIdAsync(id));
        }

        [HttpPost]
        [RequirePermission(Permissions.Roles.Create)]
        public async Task<ActionResult<RoleDto>> CreateRole(CreateRoleDto dto)
        {
            var role = await _roleService.CreateRoleAsync(dto);
            return CreatedAtAction(nameof(GetRole), new { id = role.Id }, role);
        }

        [HttpPut("{id}")]
        [RequirePermission(Permissions.Roles.Edit)]
        public async Task<ActionResult<RoleDto>> UpdateRole(Guid id, UpdateRoleDto dto)
        {
            return Ok(await _roleService.UpdateRoleAsync(id, dto));
        }

        [HttpDelete("{id}")]
        [RequirePermission(Permissions.Roles.Delete)]
        public async Task<ActionResult> DeleteRole(Guid id)
        {
            await _roleService.DeleteRoleAsync(id);
            return NoContent();
        }

        [HttpGet("permissions")]
        [RequirePermission(Permissions.Roles.View)]
        public async Task<ActionResult<List<PermissionDto>>> GetAllPermissions()
        {
            return Ok(await _roleService.GetAllPermissionsAsync());
        }

        [HttpGet("{roleId}/permissions")]
        [RequirePermission(Permissions.Roles.View)]
        public async Task<ActionResult<List<PermissionDto>>> GetRolePermissions(Guid roleId)
        {
            return Ok(await _roleService.GetRolePermissionsAsync(roleId));
        }

        [HttpPut("{roleId}/permissions")]
        [RequirePermission(Permissions.Roles.ManagePermissions)]
        public async Task<ActionResult> UpdateRolePermissions(Guid roleId, RolePermissionUpdateDto dto)
        {
            await _roleService.UpdateRolePermissionsAsync(roleId, dto);
            return NoContent();
        }

        [HttpGet("my-permissions")]
        [Authorize]
        public async Task<ActionResult<List<string>>> GetMyPermissions()
        {
            var userId = _currentUserService.UserId;
            if (userId == null)
            {
                _logger.LogWarning("No user ID found in claims");
                return Unauthorized();
            }

            // First check if user is root admin
            var rootPermissions = await _roleService.GetUserPermissionsAsync(userId.Value);
            if (rootPermissions.Any())
            {
                _logger.LogInformation("Returning root admin permissions for user: {UserId}", userId);
                return Ok(rootPermissions.Select(p => p.SystemName));
            }

            // If not root admin, get tenant permissions
            var tenantId = _tenantProvider.GetCurrentTenantId();
            if (!tenantId.HasValue)
            {
                return BadRequest("Tenant not specified");
            }

            var permissions = await _userTenantRepository.GetUserPermissionsAsync(userId.Value, tenantId.Value);
            return Ok(permissions);
        }
    }
}