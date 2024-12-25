using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StarterApi.Application.Common.Exceptions;
using StarterApi.Application.Common.Models;
using StarterApi.Application.Modules.Individuals.DTOs;
using StarterApi.Application.Modules.Individuals.Interfaces;
using StarterApi.Domain.Constants;

namespace StarterApi.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class IndividualsController : ControllerBase
    {
        private readonly IIndividualService _individualService;
        private readonly ILogger<IndividualsController> _logger;

        public IndividualsController(
            IIndividualService individualService,
            ILogger<IndividualsController> logger)
        {
            _individualService = individualService;
            _logger = logger;
        }

        [HttpPost]
        [RequirePermission(Permissions.Individuals.Create)]
        public async Task<ActionResult<IndividualDto>> CreateIndividual(CreateIndividualDto dto)
        {
            try
            {
                var individual = await _individualService.CreateIndividualAsync(dto);
                return CreatedAtAction(nameof(GetIndividual), new { id = individual.Id }, individual);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Conflict while creating individual");
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating individual");
                return StatusCode(500, "An error occurred while creating the individual");
            }
        }

        [HttpGet]
        [RequirePermission(Permissions.Individuals.View)]
        public async Task<ActionResult<PagedResult<IndividualDto>>> GetIndividuals([FromQuery] QueryParameters parameters)
        {
            try
            {
                var individuals = await _individualService.GetIndividualsAsync(parameters);
                return Ok(individuals);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving individuals");
                return StatusCode(500, "An error occurred while retrieving individuals");
            }
        }

        [HttpGet("{id}")]
        [RequirePermission(Permissions.Individuals.View)]
        public async Task<ActionResult<IndividualDto>> GetIndividual(Guid id)
        {
            try
            {
                var individual = await _individualService.GetIndividualByIdAsync(id);
                return Ok(individual);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving individual");
                return StatusCode(500, "An error occurred while retrieving the individual");
            }
        }

        [HttpPut("{id}")]
        [RequirePermission(Permissions.Individuals.Edit)]
        public async Task<ActionResult<IndividualDto>> UpdateIndividual(Guid id, UpdateIndividualDto dto)
        {
            try
            {
                var individual = await _individualService.UpdateIndividualAsync(id, dto);
                return Ok(individual);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Conflict while updating individual");
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating individual");
                return StatusCode(500, "An error occurred while updating the individual");
            }
        }

        [HttpPut("{id}/verify")]
        [RequirePermission(Permissions.Individuals.Verify)]
        public async Task<ActionResult<IndividualDto>> VerifyIndividual(Guid id, VerifyIndividualDto dto)
        {
            try
            {
                var individual = await _individualService.VerifyIndividualAsync(id, dto);
                return Ok(individual);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error verifying individual");
                return StatusCode(500, "An error occurred while verifying the individual");
            }
        }

        [HttpDelete("{id}")]
        [RequirePermission(Permissions.Individuals.Delete)]
        public async Task<ActionResult<bool>> DeleteIndividual(Guid id)
        {
            try
            {
                var result = await _individualService.DeleteIndividualAsync(id);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting individual");
                return StatusCode(500, "An error occurred while deleting the individual");
            }
        }

        [HttpGet("exists/email")]
        [RequirePermission(Permissions.Individuals.View)]
        public async Task<ActionResult<bool>> ExistsByEmail([FromQuery] string email)
        {
            try
            {
                var exists = await _individualService.ExistsByEmailAsync(email);
                return Ok(exists);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking individual existence by email");
                return StatusCode(500, "An error occurred while checking individual existence");
            }
        }

        [HttpGet("exists/phone")]
        [RequirePermission(Permissions.Individuals.View)]
        public async Task<ActionResult<bool>> ExistsByPhoneNumber([FromQuery] string phoneNumber)
        {
            try
            {
                var exists = await _individualService.ExistsByPhoneNumberAsync(phoneNumber);
                return Ok(exists);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking individual existence by phone number");
                return StatusCode(500, "An error occurred while checking individual existence");
            }
        }

        [HttpGet("exists/idproof")]
        [RequirePermission(Permissions.Individuals.View)]
        public async Task<ActionResult<bool>> ExistsByIdProof([FromQuery] string idProofType, [FromQuery] string idProofNumber)
        {
            try
            {
                var exists = await _individualService.ExistsByIdProofAsync(idProofType, idProofNumber);
                return Ok(exists);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking individual existence by ID proof");
                return StatusCode(500, "An error occurred while checking individual existence");
            }
        }
    }
}
