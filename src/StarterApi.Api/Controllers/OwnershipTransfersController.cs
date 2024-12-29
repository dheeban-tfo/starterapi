using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StarterApi.Application.Common.Models;
using StarterApi.Application.Modules.Owners.DTOs;
using StarterApi.Application.Modules.Owners.Interfaces;
using StarterApi.Domain.Constants;

namespace StarterApi.Api.Controllers
{
    [ApiController]
    [Route("api/ownership-transfers")]
    [Authorize]
    public class OwnershipTransfersController : ControllerBase
    {
        private readonly IOwnershipTransferService _transferService;
        private readonly ILogger<OwnershipTransfersController> _logger;

        public OwnershipTransfersController(
            IOwnershipTransferService transferService,
            ILogger<OwnershipTransfersController> logger)
        {
            _transferService = transferService;
            _logger = logger;
        }

        [HttpGet]
        [RequirePermission(Permissions.Owners.View)]
        public async Task<ActionResult<PagedResult<OwnershipTransferListDto>>> GetTransferRequests(
            [FromQuery] QueryParameters parameters)
        {
            try
            {
                var result = await _transferService.GetTransferRequestsAsync(parameters);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving transfer requests");
                return StatusCode(500, "An error occurred while retrieving transfer requests");
            }
        }

        [HttpGet("{id}")]
        [RequirePermission(Permissions.Owners.View)]
        public async Task<ActionResult<OwnershipTransferDto>> GetTransferRequest(Guid id)
        {
            try
            {
                var result = await _transferService.GetTransferRequestByIdAsync(id);
                if (result == null)
                    return NotFound();

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving transfer request with ID: {Id}", id);
                return StatusCode(500, "An error occurred while retrieving the transfer request");
            }
        }

        [HttpPost]
        [RequirePermission(Permissions.Owners.InitiateTransfer)]
        public async Task<ActionResult<OwnershipTransferDto>> CreateTransferRequest(
            CreateOwnershipTransferDto dto)
        {
            try
            {
                var result = await _transferService.CreateTransferRequestAsync(dto);
                return CreatedAtAction(nameof(GetTransferRequest), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating transfer request");
                return StatusCode(500, "An error occurred while creating the transfer request");
            }
        }

        [HttpPut("{id}/approve")]
        [RequirePermission(Permissions.Owners.ApproveTransfer)]
        public async Task<ActionResult<OwnershipTransferDto>> ApproveTransferRequest(Guid id)
        {
            try
            {
                var result = await _transferService.ApproveTransferRequestAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error approving transfer request with ID: {Id}", id);
                return StatusCode(500, "An error occurred while approving the transfer request");
            }
        }

        [HttpPut("{id}/reject")]
        [RequirePermission(Permissions.Owners.ApproveTransfer)]
        public async Task<ActionResult<OwnershipTransferDto>> RejectTransferRequest(
            Guid id,
            UpdateOwnershipTransferStatusDto dto)
        {
            try
            {
                var result = await _transferService.RejectTransferRequestAsync(id, dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error rejecting transfer request with ID: {Id}", id);
                return StatusCode(500, "An error occurred while rejecting the transfer request");
            }
        }
    }
} 