using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StarterApi.Application.Common.Exceptions;
using StarterApi.Application.Modules.Facilities.DTOs;
using StarterApi.Application.Modules.Facilities.Interfaces;
using StarterApi.Domain.Constants;

namespace StarterApi.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class FacilityImagesController : ControllerBase
    {
        private readonly IFacilityImageService _facilityImageService;
        private readonly ILogger<FacilityImagesController> _logger;

        public FacilityImagesController(
            IFacilityImageService facilityImageService,
            ILogger<FacilityImagesController> logger)
        {
            _facilityImageService = facilityImageService;
            _logger = logger;
        }

        [HttpGet("facility/{facilityId}")]
        [RequirePermission(Permissions.Facilities.View)]
        public async Task<ActionResult<IEnumerable<FacilityImageDto>>> GetImages(Guid facilityId)
        {
            try
            {
                var images = await _facilityImageService.GetByFacilityIdAsync(facilityId);
                return Ok(images);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving facility images");
                return StatusCode(500, "An error occurred while retrieving facility images");
            }
        }

        [HttpPost]
        [RequirePermission(Permissions.Facilities.Edit)]
        public async Task<ActionResult<FacilityImageDto>> UploadImage([FromForm] CreateFacilityImageDto dto)
        {
            if (dto.File == null || dto.File.Length == 0)
                return BadRequest("No file uploaded");

            try
            {
                var image = await _facilityImageService.UploadImageAsync(dto);
                return CreatedAtAction(nameof(GetImages), new { facilityId = image.FacilityId }, image);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading facility image");
                return StatusCode(500, "An error occurred while uploading the facility image");
            }
        }

        [HttpPut("{id}")]
        [RequirePermission(Permissions.Facilities.Edit)]
        public async Task<ActionResult<FacilityImageDto>> UpdateImage(Guid id, [FromForm] UpdateFacilityImageDto dto)
        {
            try
            {
                var image = await _facilityImageService.UpdateImageAsync(id, dto);
                return Ok(image);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating facility image");
                return StatusCode(500, "An error occurred while updating the facility image");
            }
        }

        [HttpDelete("{id}")]
        [RequirePermission(Permissions.Facilities.Delete)]
        public async Task<IActionResult> DeleteImage(Guid id)
        {
            try
            {
                var result = await _facilityImageService.DeleteImageAsync(id);
                if (!result)
                    return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting facility image");
                return StatusCode(500, "An error occurred while deleting the facility image");
            }
        }
    }
} 