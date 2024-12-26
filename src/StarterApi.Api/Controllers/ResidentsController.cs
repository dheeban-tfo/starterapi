using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StarterApi.Application.Common.Models;
using StarterApi.Application.Modules.Residents.DTOs;
using StarterApi.Application.Modules.Residents.Interfaces;
using StarterApi.Domain.Constants;

namespace StarterApi.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ResidentsController : ControllerBase
    {
        private readonly IResidentService _residentService;
        private readonly ILogger<ResidentsController> _logger;

        public ResidentsController(
            IResidentService residentService,
            ILogger<ResidentsController> logger)
        {
            _residentService = residentService;
            _logger = logger;
        }

        /// <summary>
        /// Get all residents with pagination, sorting, and filtering
        /// </summary>
        [HttpGet]
        [RequirePermission(Permissions.Residents.View)]
        public async Task<ActionResult<PagedResult<ResidentDto>>> GetResidents([FromQuery] QueryParameters parameters)
        {
            try
            {
                var residents = await _residentService.GetResidentsAsync(parameters);
                return Ok(residents);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting residents");
                return StatusCode(500, "An error occurred while retrieving residents");
            }
        }

        /// <summary>
        /// Get a resident by ID
        /// </summary>
        [HttpGet("{id}")]
        [RequirePermission(Permissions.Residents.View)]
        public async Task<ActionResult<ResidentDto>> Get(Guid id)
        {
            try
            {
                var resident = await _residentService.GetByIdAsync(id);
                return Ok(resident);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting resident {ResidentId}", id);
                throw;
            }
        }

        /// <summary>
        /// Get residents by unit
        /// </summary>
        [HttpGet("unit/{unitId}")]
        [RequirePermission(Permissions.Residents.View)]
        public async Task<ActionResult<IEnumerable<ResidentDto>>> GetByUnit(Guid unitId)
        {
            try
            {
                var residents = await _residentService.GetByUnitIdAsync(unitId);
                return Ok(residents);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting residents for unit {UnitId}", unitId);
                throw;
            }
        }

        /// <summary>
        /// Get residents by individual ID
        /// </summary>
        [HttpGet("individual/{individualId}")]
        [RequirePermission(Permissions.Residents.View)]
        public async Task<ActionResult<IEnumerable<ResidentDto>>> GetByIndividualId(Guid individualId)
        {
            try
            {
                var residents = await _residentService.GetByIndividualIdAsync(individualId);
                return Ok(residents);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting residents by individual ID {IndividualId}", individualId);
                throw;
            }
        }

        /// <summary>
        /// Get resident by user ID
        /// </summary>
        [HttpGet("user/{userId}")]
        [RequirePermission(Permissions.Residents.View)]
        public async Task<ActionResult<ResidentDto>> GetByUserId(Guid userId)
        {
            try
            {
                var resident = await _residentService.GetByUserIdAsync(userId);
                return Ok(resident);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting resident by user ID {UserId}", userId);
                throw;
            }
        }

        /// <summary>
        /// Create a new resident
        /// </summary>
        [HttpPost]
        [RequirePermission(Permissions.Residents.Create)]
        public async Task<ActionResult<ResidentDto>> Create([FromBody] CreateResidentDto dto)
        {
            try
            {
                var resident = await _residentService.CreateAsync(dto);
                return CreatedAtAction(nameof(Get), new { id = resident.Id }, resident);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating resident");
                throw;
            }
        }

        /// <summary>
        /// Update a resident
        /// </summary>
        [HttpPut("{id}")]
        [RequirePermission(Permissions.Residents.Edit)]
        public async Task<ActionResult<ResidentDto>> Update(Guid id, [FromBody] UpdateResidentDto dto)
        {
            try
            {
                var resident = await _residentService.UpdateAsync(id, dto);
                return Ok(resident);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating resident {ResidentId}", id);
                throw;
            }
        }

        /// <summary>
        /// Delete a resident
        /// </summary>
        [HttpDelete("{id}")]
        [RequirePermission(Permissions.Residents.Delete)]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _residentService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting resident {ResidentId}", id);
                throw;
            }
        }

        /// <summary>
        /// Approve a resident
        /// </summary>
        [HttpPost("{id}/approve")]
        [RequirePermission(Permissions.Residents.Verify)]
        public async Task<ActionResult<ResidentDto>> Approve(Guid id)
        {
            try
            {
                var resident = await _residentService.ApproveAsync(id);
                return Ok(resident);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error approving resident {ResidentId}", id);
                throw;
            }
        }

        /// <summary>
        /// Reject a resident
        /// </summary>
        [HttpPost("{id}/reject")]
        [RequirePermission(Permissions.Residents.Verify)]
        public async Task<ActionResult<ResidentDto>> Reject(Guid id, [FromBody] string reason)
        {
            try
            {
                var resident = await _residentService.RejectAsync(id, reason);
                return Ok(resident);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error rejecting resident {ResidentId}", id);
                throw;
            }
        }

        /// <summary>
        /// Verify a resident
        /// </summary>
        [HttpPost("{id}/verify")]
        [RequirePermission(Permissions.Residents.Verify)]
        public async Task<ActionResult<ResidentDto>> Verify(Guid id)
        {
            try
            {
                var resident = await _residentService.VerifyResidentAsync(id);
                return Ok(resident);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error verifying resident {ResidentId}", id);
                throw;
            }
        }

        /// <summary>
        /// Get pending verifications
        /// </summary>
        [HttpGet("pending-verifications")]
        [RequirePermission(Permissions.Residents.Verify)]
        public async Task<ActionResult<IEnumerable<ResidentDto>>> GetPendingVerifications()
        {
            try
            {
                var residents = await _residentService.GetPendingVerificationsAsync();
                return Ok(residents);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting pending verifications");
                throw;
            }
        }

        /// <summary>
        /// Check if a unit is available for new residents
        /// </summary>
        [HttpGet("unit/{unitId}/available")]
        [RequirePermission(Permissions.Residents.View)]
        public async Task<ActionResult<bool>> IsUnitAvailable(Guid unitId)
        {
            try
            {
                var isAvailable = await _residentService.IsUnitAvailableForResidentAsync(unitId);
                return Ok(isAvailable);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking unit availability {UnitId}", unitId);
                throw;
            }
        }

        /// <summary>
        /// Check if an individual has active residency
        /// </summary>
        [HttpGet("individual/{individualId}/active-residency")]
        [RequirePermission(Permissions.Residents.View)]
        public async Task<ActionResult<bool>> HasActiveResidency(Guid individualId)
        {
            try
            {
                var hasActive = await _residentService.HasActiveResidencyAsync(individualId);
                return Ok(hasActive);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking active residency for individual {IndividualId}", individualId);
                throw;
            }
        }
    }
} 