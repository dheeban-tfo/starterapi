using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StarterApi.Application.Common.Exceptions;
using StarterApi.Application.Modules.Units.DTOs;
using StarterApi.Application.Modules.Units.Interfaces;
using StarterApi.Domain.Constants;

namespace StarterApi.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UnitsController : ControllerBase
    {
        private readonly IUnitService _unitService;
        private readonly ILogger<UnitsController> _logger;

        public UnitsController(
            IUnitService unitService,
            ILogger<UnitsController> logger)
        {
            _unitService = unitService;
            _logger = logger;
        }

        [HttpPost]
        [RequirePermission(Permissions.Societies.ManageBlocks)]
        public async Task<ActionResult<UnitDto>> CreateUnit(CreateUnitDto dto)
        {
            try
            {
                var unit = await _unitService.CreateUnitAsync(dto);
                return CreatedAtAction(nameof(GetUnit), new { id = unit.Id }, unit);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating unit");
                return StatusCode(500, "An error occurred while creating the unit");
            }
        }

        [HttpGet]
        [RequirePermission(Permissions.Societies.View)]
        public async Task<ActionResult<IEnumerable<UnitDto>>> GetUnits()
        {
            try
            {
                var units = await _unitService.GetAllUnitsAsync();
                return Ok(units);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving units");
                return StatusCode(500, "An error occurred while retrieving units");
            }
        }

        [HttpGet("floor/{floorId}")]
        [RequirePermission(Permissions.Societies.View)]
        public async Task<ActionResult<IEnumerable<UnitDto>>> GetUnitsByFloor(Guid floorId)
        {
            try
            {
                var units = await _unitService.GetUnitsByFloorAsync(floorId);
                return Ok(units);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving units for floor");
                return StatusCode(500, "An error occurred while retrieving units");
            }
        }

        [HttpGet("{id}")]
        [RequirePermission(Permissions.Societies.View)]
        public async Task<ActionResult<UnitDto>> GetUnit(Guid id)
        {
            try
            {
                var unit = await _unitService.GetUnitByIdAsync(id);
                return Ok(unit);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving unit");
                return StatusCode(500, "An error occurred while retrieving the unit");
            }
        }

        [HttpPut("{id}")]
        [RequirePermission(Permissions.Societies.ManageBlocks)]
        public async Task<ActionResult<UnitDto>> UpdateUnit(Guid id, UpdateUnitDto dto)
        {
            try
            {
                var unit = await _unitService.UpdateUnitAsync(id, dto);
                return Ok(unit);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating unit");
                return StatusCode(500, "An error occurred while updating the unit");
            }
        }

        [HttpDelete("{id}")]
        [RequirePermission(Permissions.Societies.ManageBlocks)]
        public async Task<ActionResult<bool>> DeleteUnit(Guid id)
        {
            try
            {
                var result = await _unitService.DeleteUnitAsync(id);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting unit");
                return StatusCode(500, "An error occurred while deleting the unit");
            }
        }
    }
} 