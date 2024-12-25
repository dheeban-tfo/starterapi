using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using StarterApi.Application.Common.Interfaces;
using StarterApi.Domain.Entities;
using StarterApi.Domain.Constants;
using StarterApi.Application.Common.Exceptions;

namespace StarterApi.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DocumentsController : ControllerBase
    {
        private readonly IDocumentService _documentService;
        private readonly ILogger<DocumentsController> _logger;

        public DocumentsController(
            IDocumentService documentService,
            ILogger<DocumentsController> logger)
        {
            _documentService = documentService;
            _logger = logger;
        }

        [HttpGet]
        [RequirePermission(Permissions.Documents.View)]
        public async Task<ActionResult<IEnumerable<Document>>> GetAllDocuments()
        {
            try
            {
                var documents = await _documentService.GetAllDocumentsAsync();
                return Ok(documents);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving documents");
                return StatusCode(500, "An error occurred while retrieving documents");
            }
        }

        [HttpGet("{id}")]
        [RequirePermission(Permissions.Documents.View)]
        public async Task<ActionResult<Document>> GetDocument(Guid id)
        {
            try
            {
                var document = await _documentService.GetDocumentByIdAsync(id);
                return Ok(document);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving document");
                return StatusCode(500, "An error occurred while retrieving the document");
            }
        }

        [HttpGet("unit/{unitId}")]
        [RequirePermission(Permissions.Documents.View)]
        public async Task<ActionResult<IEnumerable<Document>>> GetDocumentsByUnit(Guid unitId)
        {
            try
            {
                var documents = await _documentService.GetDocumentsByUnitAsync(unitId);
                return Ok(documents);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving documents for unit");
                return StatusCode(500, "An error occurred while retrieving documents");
            }
        }

        [HttpGet("block/{blockId}")]
        [RequirePermission(Permissions.Documents.View)]
        public async Task<ActionResult<IEnumerable<Document>>> GetDocumentsByBlock(Guid blockId)
        {
            try
            {
                var documents = await _documentService.GetDocumentsByBlockAsync(blockId);
                return Ok(documents);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving documents for block");
                return StatusCode(500, "An error occurred while retrieving documents");
            }
        }

        [HttpGet("category/{categoryId}")]
        [RequirePermission(Permissions.Documents.View)]
        public async Task<ActionResult<IEnumerable<Document>>> GetDocumentsByCategory(Guid categoryId)
        {
            try
            {
                var documents = await _documentService.GetDocumentsByCategoryAsync(categoryId);
                return Ok(documents);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving documents for category");
                return StatusCode(500, "An error occurred while retrieving documents");
            }
        }

        [HttpPost]
        [RequirePermission(Permissions.Documents.Create)]
        public async Task<ActionResult<Document>> UploadDocument(
            IFormFile file,
            [FromQuery] Guid? categoryId = null,
            [FromQuery] Guid? unitId = null,
            [FromQuery] Guid? blockId = null)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded");

            try
            {
                using var stream = file.OpenReadStream();
                var document = await _documentService.UploadDocumentAsync(
                    file.FileName,
                    stream,
                    file.ContentType,
                    categoryId,
                    unitId,
                    blockId);

                return CreatedAtAction(nameof(GetDocument), new { id = document.Id }, document);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading document");
                return StatusCode(500, "An error occurred while uploading the document");
            }
        }

        [HttpPut("{id}")]
        [RequirePermission(Permissions.Documents.Edit)]
        public async Task<ActionResult<Document>> UpdateDocument(Guid id, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded");

            try
            {
                using var stream = file.OpenReadStream();
                var document = await _documentService.UpdateDocumentAsync(
                    id,
                    file.FileName,
                    stream,
                    file.ContentType);

                return Ok(document);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating document");
                return StatusCode(500, "An error occurred while updating the document");
            }
        }

        [HttpPost("{id}/versions")]
        [RequirePermission(Permissions.Documents.Edit)]
        public async Task<ActionResult<DocumentVersion>> AddVersion(
            Guid id,
            IFormFile file,
            [FromForm] string changeDescription)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded");

            try
            {
                using var stream = file.OpenReadStream();
                var version = await _documentService.AddVersionAsync(
                    id,
                    file.FileName,
                    stream,
                    file.ContentType,
                    changeDescription);

                return Ok(version);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding document version");
                return StatusCode(500, "An error occurred while adding the document version");
            }
        }

        [HttpGet("{id}/versions")]
        [RequirePermission(Permissions.Documents.View)]
        public async Task<ActionResult<IEnumerable<DocumentVersion>>> GetVersions(Guid id)
        {
            try
            {
                var versions = await _documentService.GetVersionsAsync(id);
                return Ok(versions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving document versions");
                return StatusCode(500, "An error occurred while retrieving document versions");
            }
        }

        [HttpGet("{id}/download")]
        [RequirePermission(Permissions.Documents.View)]
        public async Task<IActionResult> DownloadDocument(Guid id, [FromQuery] int? version = null)
        {
            try
            {
                var document = await _documentService.GetDocumentByIdAsync(id);
                var stream = await _documentService.DownloadDocumentAsync(id, version);
                return File(stream, document.ContentType, document.Name);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error downloading document");
                return StatusCode(500, "An error occurred while downloading the document");
            }
        }

        [HttpDelete("{id}")]
        [RequirePermission(Permissions.Documents.Delete)]
        public async Task<IActionResult> DeleteDocument(Guid id)
        {
            try
            {
                await _documentService.DeleteDocumentAsync(id);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting document");
                return StatusCode(500, "An error occurred while deleting the document");
            }
        }

        [HttpPost("{id}/access")]
        [RequirePermission(Permissions.Documents.Edit)]
        public async Task<ActionResult<DocumentAccess>> GrantAccess(
            Guid id,
            [FromQuery] Guid userId,
            [FromQuery] string accessLevel)
        {
            try
            {
                var access = await _documentService.GrantAccessAsync(id, userId, accessLevel);
                return Ok(access);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error granting document access");
                return StatusCode(500, "An error occurred while granting document access");
            }
        }

        [HttpDelete("{id}/access/{userId}")]
        [RequirePermission(Permissions.Documents.Edit)]
        public async Task<IActionResult> RevokeAccess(Guid id, Guid userId)
        {
            try
            {
                await _documentService.RevokeAccessAsync(id, userId);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error revoking document access");
                return StatusCode(500, "An error occurred while revoking document access");
            }
        }

        [HttpGet("{id}/access/{userId}")]
        [RequirePermission(Permissions.Documents.View)]
        public async Task<ActionResult<bool>> CheckAccess(Guid id, Guid userId)
        {
            try
            {
                var hasAccess = await _documentService.HasAccessAsync(id, userId);
                return Ok(hasAccess);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking document access");
                return StatusCode(500, "An error occurred while checking document access");
            }
        }
    }
}
