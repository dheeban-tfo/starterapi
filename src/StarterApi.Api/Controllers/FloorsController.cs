using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StarterApi.Application.Common.Exceptions;
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
        [RequirePermission(Permissions.Societies.ManageBlocks)]
        public async Task<ActionResult<FloorDto>> CreateFloor(CreateFloorDto dto)
        {
            try
            {
                var floor = await _floorService.CreateFloorAsync(dto);
                return CreatedAtAction(nameof(GetFloor), new { id = floor.Id }, floor);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating floor");
                return StatusCode(500, "An error occurred while creating the floor");
            }
        }

        [HttpGet]
        [RequirePermission(Permissions.Societies.View)]
        public async Task<ActionResult<IEnumerable<FloorDto>>> GetFloors()
        {
            try
            {
                var floors = await _floorService.GetAllFloorsAsync();
                return Ok(floors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving floors");
                return StatusCode(500, "An error occurred while retrieving floors");
            }
        }

        [HttpGet("block/{blockId}")]
        [RequirePermission(Permissions.Societies.View)]
        public async Task<ActionResult<IEnumerable<FloorDto>>> GetFloorsByBlock(Guid blockId)
        {
            try
            {
                var floors = await _floorService.GetFloorsByBlockAsync(blockId);
                return Ok(floors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving floors for block");
                return StatusCode(500, "An error occurred while retrieving floors");
            }
        }

        [HttpGet("{id}")]
        [RequirePermission(Permissions.Societies.View)]
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
        [RequirePermission(Permissions.Societies.ManageBlocks)]
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
        [RequirePermission(Permissions.Societies.ManageBlocks)]
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