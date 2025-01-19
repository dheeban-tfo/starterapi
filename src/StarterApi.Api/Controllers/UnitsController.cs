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
using StarterApi.Application.Modules.Blocks.Interfaces;
using StarterApi.Application.Modules.Floors.Interfaces;
using StarterApi.Application.Modules.Blocks.DTOs;
using StarterApi.Application.Modules.Floors.DTOs;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using StarterApi.Application.Modules.Units.Mappings;

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
        private readonly IBlockService _blockService;
        private readonly IFloorService _floorService;
        private readonly ITenantProvider _tenantProvider;

        public UnitsController(
            IUnitService unitService,
            ILogger<UnitsController> logger,
            IResidentService residentService,
            IDocumentService documentService,
            IBlockService blockService,
            IFloorService floorService,
            ITenantProvider tenantProvider)
        {
            _unitService = unitService;
            _logger = logger;
            _residentService = residentService;
            _documentService = documentService;
            _blockService = blockService;
            _floorService = floorService;
            _tenantProvider = tenantProvider;
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
                var tenantId = _tenantProvider.GetCurrentTenantId() ?? 
                    throw new InvalidOperationException("No tenant ID found in context");

                using (var reader = new StreamReader(file.OpenReadStream(), Encoding.UTF8))
                {
                    var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                    {
                        HasHeaderRecord = true,
                        Delimiter = ",",
                        TrimOptions = TrimOptions.Trim,
                        MissingFieldFound = null,
                        Mode = CsvMode.RFC4180,
                        BadDataFound = null,
                        HeaderValidated = null,
                        IgnoreBlankLines = true
                    };

                    using (var csv = new CsvReader(reader, config))
                    {
                        csv.Context.RegisterClassMap<UnitBulkImportMap>();
                        
                        // Read and validate headers first
                        csv.Read();
                        csv.ReadHeader();
                        csv.ValidateHeader<UnitBulkImportDto>();
                        
                        // Then read records
                        units = csv.GetRecords<UnitBulkImportDto>().ToList();
                    }
                }

                if (!units.Any())
                    return BadRequest("No records found in the CSV file");

                var defaultSocietyId = Guid.Parse("8050505b-3f56-47ae-a8dc-763a0e4266c1");
                var validUnits = new List<UnitBulkImportDto>();
                var result = new UnitBulkImportResultDto();

                // Validate society IDs first
                foreach (var unit in units)
                {
                    if (unit.SocietyId == Guid.Empty)
                    {
                        unit.SocietyId = defaultSocietyId;
                        validUnits.Add(unit);
                    }
                    else if (unit.SocietyId == defaultSocietyId)
                    {
                        validUnits.Add(unit);
                    }
                    else
                    {
                        _logger.LogWarning("Skipping unit {UnitNumber} due to invalid society ID. Expected: {ExpectedId}, Found: {FoundId}", 
                            unit.UnitNumber, defaultSocietyId, unit.SocietyId);
                        continue;
                    }
                }

                // Process blocks and floors first
                var createdEntities = new HashSet<string>(); // Track what we've created
                foreach (var unit in validUnits)
                {
                    _logger.LogInformation("Processing unit {UnitNumber} in block {BlockCode}, floor {FloorNumber}", 
                        unit.UnitNumber, unit.BlockCode, unit.FloorNumber);

                    // Check if block exists by code
                    var blockExists = await _blockService.ExistsByCodeAsync(unit.BlockCode, unit.SocietyId);
                    _logger.LogInformation("Block {BlockCode} exists: {Exists}", unit.BlockCode, blockExists);
                    BlockDto block;
                    
                    if (!blockExists)
                    {
                        var blockKey = $"block_{unit.BlockCode}";
                        if (!createdEntities.Contains(blockKey))
                        {
                            _logger.LogInformation("Creating new block {BlockCode}", unit.BlockCode);
                            var createBlockDto = new CreateBlockDto
                            {
                                Code = unit.BlockCode,
                                Name = unit.BlockName,
                                SocietyId = unit.SocietyId,
                                MaintenanceChargePerSqft = 0
                            };
                            block = await _blockService.CreateBlockAsync(createBlockDto);
                            createdEntities.Add(blockKey);
                            result.Warnings.Add($"Block with code {unit.BlockCode} not found. Created new block.");
                            _logger.LogInformation("Created new block {BlockCode} with ID {BlockId}", unit.BlockCode, block.Id);
                        }
                    }
                    
                    // Get the block (whether it existed or we just created it)
                    block = await _blockService.GetBlockByCodeAsync(unit.BlockCode);
                    _logger.LogInformation("Retrieved block {BlockCode} with ID {BlockId}", unit.BlockCode, block.Id);

                    // Check if floor exists
                    var floorExists = await _floorService.ExistsByNumberAsync(unit.FloorNumber, block.Id);
                    _logger.LogInformation("Floor {FloorNumber} in block {BlockCode} exists: {Exists}", 
                        unit.FloorNumber, unit.BlockCode, floorExists);

                    if (!floorExists)
                    {
                        // Create new floor only if we haven't created it yet
                        var floorKey = $"floor_{unit.BlockCode}_{unit.FloorNumber}";
                        _logger.LogInformation("Checking if floor was already created in this session. Key: {FloorKey}, Exists: {Exists}", 
                            floorKey, createdEntities.Contains(floorKey));

                        if (!createdEntities.Contains(floorKey))
                        {
                            _logger.LogInformation("Creating new floor {FloorNumber} in block {BlockCode}", 
                                unit.FloorNumber, unit.BlockCode);

                            var createFloorDto = new CreateFloorDto
                            {
                                BlockId = block.Id,
                                FloorNumber = unit.FloorNumber,
                                FloorName = unit.FloorName
                            };
                            var floor = await _floorService.CreateFloorAsync(createFloorDto);
                            createdEntities.Add(floorKey);
                            result.Warnings.Add($"Floor number {unit.FloorNumber} in block {unit.BlockCode} not found. Created new floor.");
                            _logger.LogInformation("Created new floor {FloorNumber} in block {BlockCode}", 
                                unit.FloorNumber, unit.BlockCode);
                        }
                        else
                        {
                            _logger.LogInformation("Skipping floor creation as it was already created in this session. Key: {FloorKey}", 
                                floorKey);
                        }
                    }
                }

                _logger.LogInformation("Created entities during this session: {CreatedEntities}", 
                    string.Join(", ", createdEntities));

                // Process units after blocks and floors are created
                var importResult = await _unitService.BulkImportAsync(validUnits);
                
                // Combine results
                result.TotalProcessed = importResult.TotalProcessed;
                result.SuccessCount = importResult.SuccessCount;
                result.FailureCount = importResult.FailureCount;
                result.Errors.AddRange(importResult.Errors);

                // Only include warnings if there were failures
                if (result.FailureCount > 0)
                {
                    result.Warnings.AddRange(importResult.Warnings);
                }
                else
                {
                    result.Warnings.Clear(); // Clear warnings if everything was successful
                }

                if (result.FailureCount > 0)
                {
                    // Return 207 Multi-Status when there are partial failures
                    return StatusCode(207, result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during bulk import of units: {Message}", ex.Message);
                return StatusCode(500, new { message = "An error occurred while processing the bulk import", details = ex.Message });
            }
        }
    }
} 