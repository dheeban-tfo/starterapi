using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StarterApi.Application.Modules.Facilities.DTOs;
using StarterApi.Application.Modules.Facilities.Interfaces;
using StarterApi.Domain.Constants;

namespace StarterApi.Api.Controllers
{
    [ApiController]
    [Route("api/facilities/{facilityId}/rules")]
    [Authorize]
    public class FacilityRulesController : ControllerBase
    {
        private readonly IFacilityBookingRuleService _ruleService;
        private readonly ILogger<FacilityRulesController> _logger;

        public FacilityRulesController(
            IFacilityBookingRuleService ruleService,
            ILogger<FacilityRulesController> logger)
        {
            _ruleService = ruleService;
            _logger = logger;
        }

        /// <summary>
        /// Get facility booking rules
        /// </summary>
        [HttpGet]
        [RequirePermission(Permissions.Facilities.View)]
        public async Task<ActionResult<FacilityBookingRuleDto>> GetRules(Guid facilityId)
        {
            try
            {
                var rules = await _ruleService.GetByFacilityIdAsync(facilityId);
                return Ok(rules);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving booking rules for facility {FacilityId}", facilityId);
                return StatusCode(500, "An error occurred while retrieving booking rules");
            }
        }

        /// <summary>
        /// Update facility booking rules
        /// </summary>
        [HttpPut]
        [RequirePermission(Permissions.Facilities.Edit)]
        public async Task<ActionResult<FacilityBookingRuleDto>> UpdateRules(Guid facilityId, UpdateFacilityBookingRuleDto dto)
        {
            try
            {
                var rules = await _ruleService.UpdateAsync(facilityId, dto);
                return Ok(rules);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating booking rules for facility {FacilityId}", facilityId);
                return StatusCode(500, "An error occurred while updating booking rules");
            }
        }

        /// <summary>
        /// Get available time slots for a specific date
        /// </summary>
        [HttpGet("available-slots")]
        [RequirePermission(Permissions.Facilities.View)]
        public async Task<ActionResult<IEnumerable<TimeSlotDto>>> GetAvailableSlots(
            Guid facilityId,
            [FromQuery] DateTime date)
        {
            try
            {
                var slots = await _ruleService.GetAvailableSlotsAsync(facilityId, date);
                return Ok(slots);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving available slots for facility {FacilityId} on date {Date}", facilityId, date);
                return StatusCode(500, "An error occurred while retrieving available slots");
            }
        }
    }
} 