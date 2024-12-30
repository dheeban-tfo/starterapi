using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StarterApi.Application.Common.Models;
using StarterApi.Application.Modules.Documents.DTOs;
using StarterApi.Application.Modules.Owners.DTOs;
using StarterApi.Application.Modules.Owners.Interfaces;
using StarterApi.Domain.Constants;

namespace StarterApi.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OwnersController : ControllerBase
    {
        private readonly IOwnerService _ownerService;
        private readonly ILogger<OwnersController> _logger;

        public OwnersController(IOwnerService ownerService, ILogger<OwnersController> logger)
        {
            _ownerService = ownerService;
            _logger = logger;
        }

        [HttpGet]
        [RequirePermission(Permissions.Owners.View)]
        public async Task<ActionResult<PagedResult<OwnerListDto>>> GetOwners([FromQuery] QueryParameters parameters)
        {
            try
            {
                var result = await _ownerService.GetOwnersAsync(parameters);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving owners");
                return StatusCode(500, "An error occurred while retrieving owners");
            }
        }

        [HttpGet("{id}")]
        [RequirePermission(Permissions.Owners.View)]
        public async Task<ActionResult<OwnerDto>> GetOwner(Guid id)
        {
            try
            {
                var result = await _ownerService.GetByIdAsync(id);
                if (result == null)
                    return NotFound();

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving owner with ID: {Id}", id);
                return StatusCode(500, "An error occurred while retrieving the owner");
            }
        }

        [HttpPost]
        [RequirePermission(Permissions.Owners.Create)]
        public async Task<ActionResult<OwnerDto>> CreateOwner(CreateOwnerDto dto)
        {
            try
            {
                var result = await _ownerService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetOwner), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating owner");
                return StatusCode(500, "An error occurred while creating the owner");
            }
        }

        [HttpPut("{id}")]
        [RequirePermission(Permissions.Owners.Edit)]
        public async Task<ActionResult<OwnerDto>> UpdateOwner(Guid id, UpdateOwnerDto dto)
        {
            try
            {
                var result = await _ownerService.UpdateAsync(id, dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating owner with ID: {Id}", id);
                return StatusCode(500, "An error occurred while updating the owner");
            }
        }

        [HttpDelete("{id}")]
        [RequirePermission(Permissions.Owners.Delete)]
        public async Task<ActionResult> DeleteOwner(Guid id)
        {
            try
            {
                var result = await _ownerService.DeleteAsync(id);
                if (!result)
                    return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting owner with ID: {Id}", id);
                return StatusCode(500, "An error occurred while deleting the owner");
            }
        }

        [HttpGet("{id}/history")]
        [RequirePermission(Permissions.Owners.ViewHistory)]
        public async Task<ActionResult<PagedResult<OwnershipHistoryListDto>>> GetOwnerHistory(Guid id, [FromQuery] QueryParameters parameters)
        {
            try
            {
                var result = await _ownerService.GetOwnerHistoryAsync(id, parameters);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving history for owner with ID: {Id}", id);
                return StatusCode(500, "An error occurred while retrieving owner history");
            }
        }

        [HttpGet("{id}/documents")]
        [RequirePermission(Permissions.Owners.View)]
        public async Task<ActionResult<List<DocumentDto>>> GetOwnerDocuments(Guid id)
        {
            try
            {
                var result = await _ownerService.GetOwnerDocumentsAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving documents for owner with ID: {Id}", id);
                return StatusCode(500, "An error occurred while retrieving owner documents");
            }
        }

        [HttpPost("{id}/documents")]
        [RequirePermission(Permissions.Owners.ManageDocuments)]
        public async Task<ActionResult<DocumentDto>> AddOwnerDocument(Guid id, [FromBody] Guid documentId)
        {
            try
            {
                var result = await _ownerService.AddOwnerDocumentAsync(id, documentId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding document to owner with ID: {Id}", id);
                return StatusCode(500, "An error occurred while adding the document");
            }
        }

        [HttpDelete("{id}/documents/{documentId}")]
        [RequirePermission(Permissions.Owners.ManageDocuments)]
        public async Task<ActionResult> RemoveOwnerDocument(Guid id, Guid documentId)
        {
            try
            {
                var result = await _ownerService.RemoveOwnerDocumentAsync(id, documentId);
                if (!result)
                    return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing document from owner with ID: {Id}", id);
                return StatusCode(500, "An error occurred while removing the document");
            }
        }

        [HttpGet("units/{unitId}/ownership-history")]
        [RequirePermission(Permissions.Owners.ViewHistory)]
        public async Task<ActionResult<PagedResult<OwnershipHistoryListDto>>> GetUnitOwnershipHistory(Guid unitId, [FromQuery] QueryParameters parameters)
        {
            try
            {
                var history = await _ownerService.GetUnitOwnershipHistoryAsync(unitId, parameters);
                return Ok(history);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving ownership history for unit {UnitId}", unitId);
                return StatusCode(500, "An error occurred while retrieving unit ownership history");
            }
        }
    }
} 