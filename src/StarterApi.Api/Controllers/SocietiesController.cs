using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StarterApi.Application.Common.Exceptions;
using StarterApi.Application.Common.Models;
using StarterApi.Application.Modules.Societies.DTOs;
using StarterApi.Application.Modules.Societies.Interfaces;
using StarterApi.Domain.Constants;

namespace StarterApi.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SocietiesController : ControllerBase
    {
        private readonly ISocietyService _societyService;
        private readonly ILogger<SocietiesController> _logger;

        public SocietiesController(
            ISocietyService societyService,
            ILogger<SocietiesController> logger)
        {
            _societyService = societyService;
            _logger = logger;
        }

        [HttpPost]
        [RequirePermission(Permissions.Societies.Create)]
        public async Task<ActionResult<SocietyDto>> CreateSociety(CreateSocietyDto dto)
        {
            try
            {
                var society = await _societyService.CreateSocietyAsync(dto);
                return CreatedAtAction(nameof(GetSociety), new { id = society.Id }, society);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating society");
                return StatusCode(500, "An error occurred while creating the society");
            }
        }

        [HttpGet]
        [RequirePermission(Permissions.Societies.View)]
        public async Task<ActionResult<PagedResult<SocietyDto>>> GetSocieties([FromQuery] QueryParameters parameters)
        {
            try
            {
                var societies = await _societyService.GetSocietiesAsync(parameters);
                return Ok(societies);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving societies");
                return StatusCode(500, "An error occurred while retrieving societies");
            }
        }

        [HttpGet("{id}")]
        [RequirePermission(Permissions.Societies.View)]
        public async Task<ActionResult<SocietyDto>> GetSociety(Guid id)
        {
            try
            {
                var society = await _societyService.GetSocietyByIdAsync(id);
                return Ok(society);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving society");
                return StatusCode(500, "An error occurred while retrieving the society");
            }
        }

        [HttpPut("{id}")]
        [RequirePermission(Permissions.Societies.Edit)]
        public async Task<ActionResult<SocietyDto>> UpdateSociety(Guid id, UpdateSocietyDto dto)
        {
            try
            {
                var society = await _societyService.UpdateSocietyAsync(id, dto);
                return Ok(society);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating society");
                return StatusCode(500, "An error occurred while updating the society");
            }
        }

        [HttpDelete("{id}")]
        [RequirePermission(Permissions.Societies.Delete)]
        public async Task<ActionResult<bool>> DeleteSociety(Guid id)
        {
            try
            {
                var result = await _societyService.DeleteSocietyAsync(id);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting society");
                return StatusCode(500, "An error occurred while deleting the society");
            }
        }
    }
} 