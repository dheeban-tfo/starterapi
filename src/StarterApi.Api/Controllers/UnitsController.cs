using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StarterApi.Application.Common.Exceptions;
using StarterApi.Application.Common.Models;
using StarterApi.Application.Modules.Units.DTOs;
using StarterApi.Application.Modules.Units.Interfaces;
using StarterApi.Domain.Constants;
using StarterApi.Application.Common.Interfaces;
using StarterApi.Domain.Entities;
using StarterApi.Application.Modules.Residents.DTOs;
using StarterApi.Application.Modules.Residents.Interfaces;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace StarterApi.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UnitsController : ControllerBase
    {
        private readonly IUnitService _unitService;
        private readonly ILogger<UnitsController> _logger;
        private readonly IResidentService _residentService;
        private readonly IDocumentService _documentService;

        public UnitsController(
            IUnitService unitService,
            ILogger<UnitsController> logger,
            IResidentService residentService,
            IDocumentService documentService)
        {
            _unitService = unitService;
            _logger = logger;
            _residentService = residentService;
            _documentService = documentService;
        }

        [HttpPost]
        [RequirePermission(Permissions.Units.Create)]
        public async Task<ActionResult<UnitDto>> CreateUnit(CreateUnitDto dto)
        {
            try
            {
                var unit = await _unitService.CreateUnitAsync(dto);
                return CreatedAtAction(nameof(GetUnit), new { id = unit.Id }, unit);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating unit");
                return StatusCode(500, "An error occurred while creating the unit");
            }
        }

        [HttpGet]
        [RequirePermission(Permissions.Units.View)]
        public async Task<ActionResult<PagedResult<UnitListDto>>> GetUnits([FromQuery] QueryParameters parameters)
        {
            try
            {
                var units = await _unitService.GetUnitsAsync(parameters);
                return Ok(units);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving units");
                return StatusCode(500, "An error occurred while retrieving units");
            }
        }

        [HttpGet("{id}")]
        [RequirePermission(Permissions.Units.View)]
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
        [RequirePermission(Permissions.Units.Edit)]
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
        [RequirePermission(Permissions.Units.Delete)]
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

        [HttpGet("{id}/residents")]
        [RequirePermission(Permissions.Residents.View)]
        public async Task<ActionResult<IEnumerable<ResidentDto>>> GetResidents(Guid id)
        {
            try
            {
                // First check if the unit exists
                var unit = await _unitService.GetUnitByIdAsync(id);
                if (unit == null)
                {
                    return Ok(Array.Empty<ResidentDto>());
                }

                var residents = await _residentService.GetByUnitIdAsync(id);
                return Ok(residents);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving residents for unit {UnitId}", id);
                return StatusCode(500, "An error occurred while retrieving residents");
            }
        }

        [HttpGet("{id}/documents")]
        [RequirePermission(Permissions.Documents.View)]
        public async Task<ActionResult<IEnumerable<Document>>> GetDocuments(Guid id)
        {
            try
            {
                // First check if the unit exists
                var unit = await _unitService.GetUnitByIdAsync(id);
                if (unit == null)
                {
                    return Ok(Array.Empty<Document>());
                }

                var documents = await _documentService.GetDocumentsByUnitAsync(id);
                return Ok(documents);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving documents for unit {UnitId}", id);
                return StatusCode(500, "An error occurred while retrieving documents");
            }
        }

        [HttpPost("bulk-import")]
        [RequirePermission(Permissions.Units.BulkImport)]
        public async Task<ActionResult<UnitBulkImportResultDto>> BulkImport(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return BadRequest("No file uploaded");

                if (!file.FileName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
                    return BadRequest("Only CSV files are supported");

                var units = new List<UnitBulkImportDto>();

                using (var reader = new StreamReader(file.OpenReadStream()))
                {
                    var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                    {
                        HasHeaderRecord = true,
                        Delimiter = ",",
                        TrimOptions = TrimOptions.Trim,
                        MissingFieldFound = null
                    };

                    using (var csv = new CsvReader(reader, config))
                    {
                        units = csv.GetRecords<UnitBulkImportDto>().ToList();
                    }
                }

                if (!units.Any())
                    return BadRequest("No records found in the CSV file");

                var result = await _unitService.BulkImportAsync(units);

                if (result.FailureCount > 0)
                {
                    // Return 207 Multi-Status when there are partial failures
                    return StatusCode(207, result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during bulk import of units");
                return StatusCode(500, new { message = "An error occurred while processing the bulk import" });
            }
        }
    }
} 