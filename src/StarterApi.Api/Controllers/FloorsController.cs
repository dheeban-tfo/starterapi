using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StarterApi.Application.Common.Exceptions;
using StarterApi.Application.Common.Models;
using StarterApi.Application.Modules.Floors.DTOs;
using StarterApi.Application.Modules.Floors.Interfaces;
using StarterApi.Domain.Constants;

namespace StarterApi.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class FloorsController : ControllerBase
    {
        private readonly IFloorService _floorService;
        private readonly ILogger<FloorsController> _logger;

        public FloorsController(
            IFloorService floorService,
            ILogger<FloorsController> logger)
        {
            _floorService = floorService;
            _logger = logger;
        }

        [HttpPost]
        [RequirePermission(Permissions.Floors.Create)]
        public async Task<ActionResult<FloorDto>> CreateFloor(CreateFloorDto dto)
        {
            try
            {
                var floor = await _floorService.CreateFloorAsync(dto);
                return CreatedAtAction(nameof(GetFloor), new { id = floor.Id }, floor);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating floor");
                return StatusCode(500, "An error occurred while creating the floor");
            }
        }

        [HttpGet]
        [RequirePermission(Permissions.Floors.View)]
        public async Task<ActionResult<PagedResult<FloorDto>>> GetFloors([FromQuery] QueryParameters parameters)
        {
            try
            {
                var floors = await _floorService.GetFloorsAsync(parameters);
                return Ok(floors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving floors");
                return StatusCode(500, "An error occurred while retrieving floors");
            }
        }

        [HttpGet("{id}")]
        [RequirePermission(Permissions.Floors.View)]
        public async Task<ActionResult<FloorDto>> GetFloor(Guid id)
        {
            try
            {
                var floor = await _floorService.GetFloorByIdAsync(id);
                return Ok(floor);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving floor");
                return StatusCode(500, "An error occurred while retrieving the floor");
            }
        }

        [HttpPut("{id}")]
        [RequirePermission(Permissions.Floors.Edit)]
        public async Task<ActionResult<FloorDto>> UpdateFloor(Guid id, UpdateFloorDto dto)
        {
            try
            {
                var floor = await _floorService.UpdateFloorAsync(id, dto);
                return Ok(floor);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating floor");
                return StatusCode(500, "An error occurred while updating the floor");
            }
        }

        [HttpDelete("{id}")]
        [RequirePermission(Permissions.Floors.Delete)]
        public async Task<ActionResult<bool>> DeleteFloor(Guid id)
        {
            try
            {
                var result = await _floorService.DeleteFloorAsync(id);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting floor");
                return StatusCode(500, "An error occurred while deleting the floor");
            }
        }
    }
} 