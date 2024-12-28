using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StarterApi.Application.Common.Models;
using StarterApi.Application.Modules.Facilities.DTOs;
using StarterApi.Application.Modules.Facilities.Interfaces;
using StarterApi.Domain.Constants;

namespace StarterApi.Api.Controllers
{
    [ApiController]
    [Route("api/facilities/{facilityId}/blackout-dates")]
    [Authorize]
    public class FacilityBlackoutDatesController : ControllerBase
    {
        private readonly IFacilityBlackoutDateService _blackoutDateService;
        private readonly ILogger<FacilityBlackoutDatesController> _logger;

        public FacilityBlackoutDatesController(
            IFacilityBlackoutDateService blackoutDateService,
            ILogger<FacilityBlackoutDatesController> logger)
        {
            _blackoutDateService = blackoutDateService;
            _logger = logger;
        }

        /// <summary>
        /// Get blackout dates for a facility
        /// </summary>
        [HttpGet]
        [RequirePermission(Permissions.Facilities.View)]
        public async Task<ActionResult<PagedResult<FacilityBlackoutDateDto>>> GetBlackoutDates(
            Guid facilityId,
            [FromQuery] QueryParameters parameters)
        {
            try
            {
                var blackoutDates = await _blackoutDateService.GetPagedAsync(facilityId, parameters);
                return Ok(blackoutDates);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving blackout dates for facility {FacilityId}", facilityId);
                return StatusCode(500, "An error occurred while retrieving blackout dates");
            }
        }

        /// <summary>
        /// Get blackout date by ID
        /// </summary>
        [HttpGet("{id}")]
        [RequirePermission(Permissions.Facilities.View)]
        public async Task<ActionResult<FacilityBlackoutDateDto>> GetBlackoutDate(Guid facilityId, Guid id)
        {
            try
            {
                var blackoutDate = await _blackoutDateService.GetByIdAsync(id);
                return Ok(blackoutDate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving blackout date {Id} for facility {FacilityId}", id, facilityId);
                return StatusCode(500, "An error occurred while retrieving blackout date");
            }
        }

        /// <summary>
        /// Get blackout dates for a date range
        /// </summary>
        [HttpGet("range")]
        [RequirePermission(Permissions.Facilities.View)]
        public async Task<ActionResult<IEnumerable<FacilityBlackoutDateDto>>> GetBlackoutDateRange(
            Guid facilityId,
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            try
            {
                var blackoutDates = await _blackoutDateService.GetByDateRangeAsync(facilityId, startDate, endDate);
                return Ok(blackoutDates);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving blackout dates for facility {FacilityId} between {StartDate} and {EndDate}", 
                    facilityId, startDate, endDate);
                return StatusCode(500, "An error occurred while retrieving blackout dates");
            }
        }

        /// <summary>
        /// Update blackout date
        /// </summary>
        [HttpPut("{id}")]
        [RequirePermission(Permissions.Facilities.Edit)]
        public async Task<ActionResult<FacilityBlackoutDateDto>> UpdateBlackoutDate(
            Guid facilityId,
            Guid id,
            UpdateFacilityBlackoutDateDto dto)
        {
            try
            {
                var blackoutDate = await _blackoutDateService.UpdateAsync(id, dto);
                return Ok(blackoutDate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating blackout date {Id} for facility {FacilityId}", id, facilityId);
                return StatusCode(500, "An error occurred while updating blackout date");
            }
        }

        /// <summary>
        /// Delete blackout date
        /// </summary>
        [HttpDelete("{id}")]
        [RequirePermission(Permissions.Facilities.Edit)]
        public async Task<ActionResult> DeleteBlackoutDate(Guid facilityId, Guid id)
        {
            try
            {
                await _blackoutDateService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting blackout date {Id} for facility {FacilityId}", id, facilityId);
                return StatusCode(500, "An error occurred while deleting blackout date");
            }
        }
    }
} 