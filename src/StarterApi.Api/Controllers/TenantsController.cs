using Microsoft.AspNetCore.Mvc;
using StarterApi.Application.Common.Exceptions;


namespace StarterApi.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TenantsController : ControllerBase
    {
        private readonly ITenantService _tenantService;
        private readonly ILogger<TenantsController> _logger;

        public TenantsController(ITenantService tenantService, ILogger<TenantsController> logger)
        {
            _tenantService = tenantService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TenantDto>>> GetAll()
        {
            try
            {
                var tenants = await _tenantService.GetAllTenantsAsync();
                return Ok(tenants);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all tenants");
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TenantDto>> GetById(Guid id)
        {
            try
            {
                var tenant = await _tenantService.GetTenantByIdAsync(id);
                return Ok(tenant);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting tenant {TenantId}", id);
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        [HttpPost]
        public async Task<ActionResult<TenantDto>> Create(CreateTenantDto dto)
        {
            try
            {
                var tenant = await _tenantService.CreateTenantAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = tenant.Id }, tenant);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating tenant");
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        [HttpPut("{id}/deactivate")]
        public async Task<IActionResult> Deactivate(Guid id)
        {
            try
            {
                await _tenantService.DeactivateTenantAsync(id);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deactivating tenant {TenantId}", id);
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        [HttpGet("current")]
       // [RequireTenant]
        public async Task<ActionResult<TenantDto>> GetCurrent([FromServices] ITenantProvider tenantProvider)
        {
            var tenantId = tenantProvider.GetCurrentTenantId();
            return await GetById(tenantId.Value);
        }
    }
} 