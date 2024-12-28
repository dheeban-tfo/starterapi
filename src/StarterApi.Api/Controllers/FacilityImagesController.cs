using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StarterApi.Application.Modules.Facilities.DTOs;
using StarterApi.Application.Modules.Facilities.Interfaces;
using StarterApi.Domain.Constants;

namespace StarterApi.Api.Controllers
{
    [ApiController]
    [Route("api/facilities/{facilityId}/images")]
    [Authorize]
    public class FacilityImagesController : ControllerBase
    {
        private readonly IFacilityImageService _imageService;
        private readonly ILogger<FacilityImagesController> _logger;

        public FacilityImagesController(
            IFacilityImageService imageService,
            ILogger<FacilityImagesController> logger)
        {
            _imageService = imageService;
            _logger = logger;
        }

        /// <summary>
        /// Get facility images
        /// </summary>
        [HttpGet]
        [RequirePermission(Permissions.Facilities.View)]
        public async Task<ActionResult<IEnumerable<FacilityImageDto>>> GetImages(Guid facilityId)
        {
            try
            {
                var images = await _imageService.GetByFacilityIdAsync(facilityId);
                return Ok(images);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving images for facility {FacilityId}", facilityId);
                return StatusCode(500, "An error occurred while retrieving facility images");
            }
        }

        /// <summary>
        /// Upload facility image
        /// </summary>
        [HttpPost]
        [RequirePermission(Permissions.Facilities.Edit)]
        public async Task<ActionResult<FacilityImageDto>> UploadImage(
            Guid facilityId,
            [FromForm] IFormFile file,
            [FromForm] CreateFacilityImageDto dto)
        {
            try
            {
                var image = await _imageService.UploadAsync(facilityId, file, dto);
                return Ok(image);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading image for facility {FacilityId}", facilityId);
                return StatusCode(500, "An error occurred while uploading facility image");
            }
        }

        /// <summary>
        /// Update facility image
        /// </summary>
        [HttpPut("{id}")]
        [RequirePermission(Permissions.Facilities.Edit)]
        public async Task<ActionResult<FacilityImageDto>> UpdateImage(
            Guid facilityId,
            Guid id,
            UpdateFacilityImageDto dto)
        {
            try
            {
                var image = await _imageService.UpdateAsync(id, dto);
                return Ok(image);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating image {Id} for facility {FacilityId}", id, facilityId);
                return StatusCode(500, "An error occurred while updating facility image");
            }
        }

        /// <summary>
        /// Delete facility image
        /// </summary>
        [HttpDelete("{id}")]
        [RequirePermission(Permissions.Facilities.Edit)]
        public async Task<ActionResult> DeleteImage(Guid facilityId, Guid id)
        {
            try
            {
                await _imageService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting image {Id} for facility {FacilityId}", id, facilityId);
                return StatusCode(500, "An error occurred while deleting facility image");
            }
        }

        /// <summary>
        /// Set primary image
        /// </summary>
        [HttpPost("{id}/set-primary")]
        [RequirePermission(Permissions.Facilities.Edit)]
        public async Task<ActionResult<FacilityImageDto>> SetPrimaryImage(Guid facilityId, Guid id)
        {
            try
            {
                var image = await _imageService.SetPrimaryImageAsync(facilityId, id);
                return Ok(image);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error setting primary image {Id} for facility {FacilityId}", id, facilityId);
                return StatusCode(500, "An error occurred while setting primary image");
            }
        }

        /// <summary>
        /// Reorder images
        /// </summary>
        [HttpPost("reorder")]
        [RequirePermission(Permissions.Facilities.Edit)]
        public async Task<ActionResult> ReorderImages(Guid facilityId, [FromBody] IEnumerable<Guid> imageIds)
        {
            try
            {
                await _imageService.ReorderImagesAsync(facilityId, imageIds);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error reordering images for facility {FacilityId}", facilityId);
                return StatusCode(500, "An error occurred while reordering facility images");
            }
        }
    }
} 