using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using StarterApi.Application.Modules.Roles.Interfaces;
using StarterApi.Domain.Constants;

namespace StarterApi.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly ILogger<RolesController> _logger;

        public RolesController(IRoleService roleService, ILogger<RolesController> logger)
        {
            _roleService = roleService;
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
    }
}