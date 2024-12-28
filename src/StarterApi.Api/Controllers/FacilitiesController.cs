using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StarterApi.Application.Common.Models;
using StarterApi.Application.Modules.Facilities.DTOs;
using StarterApi.Application.Modules.Facilities.Interfaces;
using StarterApi.Domain.Constants;

namespace StarterApi.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class FacilitiesController : ControllerBase
    {
        private readonly IFacilityService _facilityService;
        private readonly ILogger<FacilitiesController> _logger;

        public FacilitiesController(
            IFacilityService facilityService,
            ILogger<FacilitiesController> logger)
        {
            _facilityService = facilityService;
            _logger = logger;
        }

        /// <summary>
        /// Get a paged list of facilities
        /// </summary>
        [HttpGet]
        [RequirePermission(Permissions.Facilities.View)]
        public async Task<ActionResult<PagedResult<FacilityListDto>>> GetFacilities(
            [FromQuery] QueryParameters parameters)
        {
            try
            {
                var facilities = await _facilityService.GetFacilitiesAsync(parameters);
                return Ok(facilities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving facilities");
                return StatusCode(500, "An error occurred while retrieving facilities");
            }
        }

        /// <summary>
        /// Get facility by ID
        /// </summary>
        [HttpGet("{id}")]
        [RequirePermission(Permissions.Facilities.View)]
        public async Task<ActionResult<FacilityDto>> GetFacility(Guid id)
        {
            try
            {
                var facility = await _facilityService.GetByIdAsync(id);
                return Ok(facility);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving facility with ID {Id}", id);
                return StatusCode(500, "An error occurred while retrieving the facility");
            }
        }

        /// <summary>
        /// Create a new facility
        /// </summary>
        [HttpPost]
        [RequirePermission(Permissions.Facilities.Create)]
        public async Task<ActionResult<FacilityDto>> CreateFacility(CreateFacilityDto dto)
        {
            try
            {
                var facility = await _facilityService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetFacility), new { id = facility.Id }, facility);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating facility");
                return StatusCode(500, "An error occurred while creating the facility");
            }
        }

        /// <summary>
        /// Update an existing facility
        /// </summary>
        [HttpPut("{id}")]
        [RequirePermission(Permissions.Facilities.Edit)]
        public async Task<ActionResult<FacilityDto>> UpdateFacility(Guid id, UpdateFacilityDto dto)
        {
            try
            {
                var facility = await _facilityService.UpdateAsync(id, dto);
                return Ok(facility);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating facility with ID {Id}", id);
                return StatusCode(500, "An error occurred while updating the facility");
            }
        }

        /// <summary>
        /// Delete a facility
        /// </summary>
        [HttpDelete("{id}")]
        [RequirePermission(Permissions.Facilities.Delete)]
        public async Task<ActionResult> DeleteFacility(Guid id)
        {
            try
            {
                await _facilityService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting facility with ID {Id}", id);
                return StatusCode(500, "An error occurred while deleting the facility");
            }
        }

        /// <summary>
        /// Get facility types
        /// </summary>
        [HttpGet("types")]
        [RequirePermission(Permissions.Facilities.View)]
        public async Task<ActionResult<IEnumerable<FacilityTypeDto>>> GetFacilityTypes()
        {
            try
            {
                var types = await _facilityService.GetFacilityTypesAsync();
                return Ok(types);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving facility types");
                return StatusCode(500, "An error occurred while retrieving facility types");
            }
        }

        /// <summary>
        /// Get facility status types
        /// </summary>
        [HttpGet("status")]
        [RequirePermission(Permissions.Facilities.View)]
        public async Task<ActionResult<IEnumerable<FacilityStatusDto>>> GetFacilityStatusTypes()
        {
            try
            {
                var statuses = await _facilityService.GetFacilityStatusTypesAsync();
                return Ok(statuses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving facility status types");
                return StatusCode(500, "An error occurred while retrieving facility status types");
            }
        }

        /// <summary>
        /// Temporarily close a facility
        /// </summary>
        [HttpPost("{id}/close-temporary")]
        [RequirePermission(Permissions.Facilities.Edit)]
        public async Task<ActionResult> TemporaryCloseFacility(Guid id)
        {
            try
            {
                await _facilityService.TemporaryCloseAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error temporarily closing facility with ID {Id}", id);
                return StatusCode(500, "An error occurred while temporarily closing the facility");
            }
        }

        /// <summary>
        /// Reopen a facility
        /// </summary>
        [HttpPost("{id}/reopen")]
        [RequirePermission(Permissions.Facilities.Edit)]
        public async Task<ActionResult> ReopenFacility(Guid id)
        {
            try
            {
                await _facilityService.ReopenAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error reopening facility with ID {Id}", id);
                return StatusCode(500, "An error occurred while reopening the facility");
            }
        }

        /// <summary>
        /// Get facilities by society ID
        /// </summary>
        [HttpGet("society/{societyId}")]
        [RequirePermission(Permissions.Facilities.View)]
        public async Task<ActionResult<IEnumerable<FacilityListDto>>> GetFacilitiesBySociety(Guid societyId)
        {
            try
            {
                var facilities = await _facilityService.GetBySocietyIdAsync(societyId);
                return Ok(facilities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving facilities for society with ID {SocietyId}", societyId);
                return StatusCode(500, "An error occurred while retrieving facilities");
            }
        }
    }
} 