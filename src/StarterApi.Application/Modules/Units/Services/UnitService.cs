using AutoMapper;
using Microsoft.Extensions.Logging;
using StarterApi.Application.Common.Exceptions;
using StarterApi.Application.Common.Models;
using StarterApi.Application.Modules.Units.DTOs;
using StarterApi.Application.Modules.Units.Interfaces;
using StarterApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using StarterApi.Application.Common.Interfaces;
using StarterApi.Application.Modules.Owners.DTOs;
using StarterApi.Application.Modules.Blocks.Interfaces;
using StarterApi.Application.Modules.Floors.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace StarterApi.Application.Modules.Units.Services
{
    public class UnitService : IUnitService
    {
        private readonly IUnitRepository _unitRepository;
        private readonly ITenantDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<UnitService> _logger;
        private readonly IBlockRepository _blockRepository;
        private readonly IFloorRepository _floorRepository;

        public UnitService(
            IUnitRepository unitRepository,
            ITenantDbContext context,
            IMapper mapper,
            ILogger<UnitService> logger,
            IBlockRepository blockRepository,
            IFloorRepository floorRepository)
        {
            _unitRepository = unitRepository;
            _context = context;
            _mapper = mapper;
            _logger = logger;
            _blockRepository = blockRepository;
            _floorRepository = floorRepository;
        }

        public async Task<UnitDto> CreateUnitAsync(CreateUnitDto dto)
        {
            if (await _unitRepository.ExistsAsync(dto.UnitNumber, dto.FloorId))
                throw new InvalidOperationException($"Unit with number {dto.UnitNumber} already exists on this floor");

            if (dto.CurrentOwnerId.HasValue)
            {
                var individual = await _unitRepository.GetIndividualByIdAsync(dto.CurrentOwnerId.Value);
                if (individual == null)
                    throw new NotFoundException($"Individual with ID {dto.CurrentOwnerId} not found");

                // Get or create owner for this individual
                var owner = await _unitRepository.GetOwnerByIndividualIdAsync(dto.CurrentOwnerId.Value);
                if (owner == null)
                {
                    owner = new Owner
                    {
                        IndividualId = dto.CurrentOwnerId.Value,
                        OwnershipType = "Primary", // Default value
                        OwnershipStartDate = DateTime.UtcNow
                    };
                    await _unitRepository.AddOwnerAsync(owner);
                    await _unitRepository.SaveChangesAsync();
                }
                
                dto.CurrentOwnerId = owner.Id;
            }

            var unit = new Unit
            {
                FloorId = dto.FloorId,
                UnitNumber = dto.UnitNumber,
                Type = dto.Type,
                BuiltUpArea = dto.BuiltUpArea,
                CarpetArea = dto.CarpetArea,
                FurnishingStatus = dto.FurnishingStatus,
                Status = dto.Status,
                CurrentOwnerId = dto.CurrentOwnerId,
                MonthlyMaintenanceFee = dto.MonthlyMaintenanceFee
            };

            await _unitRepository.AddAsync(unit);
            await _unitRepository.SaveChangesAsync();

            return _mapper.Map<UnitDto>(unit);
        }

        public async Task<UnitDto> GetUnitByIdAsync(Guid id)
        {
            var unit = await _unitRepository.GetByIdAsync(id);
            if (unit == null)
                throw new NotFoundException($"Unit with ID {id} not found");

            return _mapper.Map<UnitDto>(unit);
        }

        public async Task<UnitDto> UpdateUnitAsync(Guid id, UpdateUnitDto dto)
        {
            var unit = await _unitRepository.GetByIdAsync(id);
            if (unit == null)
                throw new NotFoundException($"Unit with ID {id} not found");

            unit.Type = dto.Type;
            unit.BuiltUpArea = dto.BuiltUpArea;
            unit.CarpetArea = dto.CarpetArea;
            unit.FurnishingStatus = dto.FurnishingStatus;
            unit.CurrentOwnerId = dto.CurrentOwnerId;
            unit.MonthlyMaintenanceFee = dto.MonthlyMaintenanceFee;

            await _unitRepository.UpdateAsync(unit);
            await _unitRepository.SaveChangesAsync();

            return _mapper.Map<UnitDto>(unit);
        }

        public async Task<bool> DeleteUnitAsync(Guid id)
        {
            var unit = await _unitRepository.GetByIdAsync(id);
            if (unit == null)
                throw new NotFoundException($"Unit with ID {id} not found");

            unit.IsActive = false;
            await _unitRepository.UpdateAsync(unit);
            await _unitRepository.SaveChangesAsync();

            return true;
        }

        public async Task<PagedResult<UnitListDto>> GetUnitsAsync(QueryParameters parameters)
        {
            var pagedUnits = await _unitRepository.GetPagedAsync(parameters);
            
            var unitDtos = _mapper.Map<IEnumerable<UnitListDto>>(pagedUnits.Items);
            
            return new PagedResult<UnitListDto>
            {
                Items = unitDtos,
                TotalItems = pagedUnits.TotalItems,
                PageNumber = pagedUnits.PageNumber,
                PageSize = pagedUnits.PageSize,
                TotalPages = pagedUnits.TotalPages,
                HasNextPage = pagedUnits.HasNextPage,
                HasPreviousPage = pagedUnits.HasPreviousPage
            };
        }

        public async Task<bool> ExistsByNumberAsync(string number, Guid floorId)
        {
            return await _unitRepository.ExistsAsync(number, floorId);
        }

        public async Task<PagedResult<OwnershipHistoryListDto>> GetUnitOwnershipHistoryAsync(Guid unitId, QueryParameters parameters)
        {
            var query = _context.OwnershipHistories
                .Include(oh => oh.Owner)
                    .ThenInclude(o => o.Individual)
                .Include(oh => oh.Unit)
                .Where(oh => oh.UnitId == unitId)
                .OrderByDescending(oh => oh.TransferDate)
                .AsNoTracking();

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .Select(oh => new OwnershipHistoryListDto
                {
                    Id = oh.Id,
                    UnitNumber = oh.Unit.UnitNumber,
                    OwnerName = $"{oh.Owner.Individual.FirstName} {oh.Owner.Individual.LastName}",
                    TransferType = oh.TransferType,
                    TransferDate = oh.TransferDate,
                    Status = oh.Status,
                    PreviousOwnerName = oh.PreviousOwnerName
                })
                .ToListAsync();

            return new PagedResult<OwnershipHistoryListDto>
            {
                Items = items,
                TotalItems = totalCount,
                PageNumber = parameters.PageNumber,
                PageSize = parameters.PageSize,
                TotalPages = (int)Math.Ceiling(totalCount / (double)parameters.PageSize),
                HasNextPage = parameters.PageNumber < (int)Math.Ceiling(totalCount / (double)parameters.PageSize),
                HasPreviousPage = parameters.PageNumber > 1
            };
        }

        public async Task<UnitBulkImportResultDto> BulkImportAsync(IEnumerable<UnitBulkImportDto> units)
        {
            var result = new UnitBulkImportResultDto();
            var processedUnits = new List<Unit>();

            foreach (var importUnit in units)
            {
                try
                {
                    result.TotalProcessed++;
                    _logger.LogInformation("UnitService: Processing unit {UnitNumber} in block {BlockCode}, floor {FloorNumber}", 
                        importUnit.UnitNumber, importUnit.BlockCode, importUnit.FloorNumber);

                    // Get block and floor
                    var block = await _blockRepository.GetByCodeAsync(importUnit.BlockCode);
                    if (block == null)
                    {
                        _logger.LogError("UnitService: Block {BlockCode} not found", importUnit.BlockCode);
                        result.Errors.Add($"Block with code {importUnit.BlockCode} not found.");
                        result.FailureCount++;
                        continue;
                    }

                    var floor = await _floorRepository.GetByNumberAndBlockAsync(importUnit.FloorNumber, block.Id);
                    if (floor == null)
                    {
                        _logger.LogError("UnitService: Floor {FloorNumber} in block {BlockCode} not found", 
                            importUnit.FloorNumber, importUnit.BlockCode);
                        result.Errors.Add($"Floor number {importUnit.FloorNumber} in block {importUnit.BlockCode} not found.");
                        result.FailureCount++;
                        continue;
                    }

                    // Check if unit already exists
                    var existingUnit = await _unitRepository.GetByNumberAndFloorAsync(importUnit.UnitNumber, floor.Id);
                    if (existingUnit != null)
                    {
                        _logger.LogWarning("UnitService: Unit {UnitNumber} already exists in floor {FloorNumber}, block {BlockCode}", 
                            importUnit.UnitNumber, importUnit.FloorNumber, importUnit.BlockCode);
                        result.Warnings.Add($"Unit {importUnit.UnitNumber} already exists. Skipping.");
                        continue;
                    }

                    _logger.LogInformation("UnitService: Creating new unit {UnitNumber} in floor {FloorNumber}, block {BlockCode}", 
                        importUnit.UnitNumber, importUnit.FloorNumber, importUnit.BlockCode);

                    // Create new unit
                    var unit = new Unit
                    {
                        FloorId = floor.Id,
                        UnitNumber = importUnit.UnitNumber,
                        Type = importUnit.Type,
                        BuiltUpArea = importUnit.BuiltUpArea,
                        CarpetArea = importUnit.CarpetArea,
                        FurnishingStatus = importUnit.FurnishingStatus,
                        Status = importUnit.Status,
                        MonthlyMaintenanceFee = importUnit.MonthlyMaintenanceFee
                    };

                    await _unitRepository.AddAsync(unit);
                    processedUnits.Add(unit);
                    result.SuccessCount++;
                    _logger.LogInformation("UnitService: Successfully created unit {UnitNumber}", importUnit.UnitNumber);
                }
                catch (Exception ex)
                {
                    result.FailureCount++;
                    result.Errors.Add($"Failed to process unit {importUnit.UnitNumber}: {ex.Message}");
                    _logger.LogError(ex, "UnitService: Error processing unit {UnitNumber} during bulk import", importUnit.UnitNumber);
                }
            }

            // Save all changes at once
            await _unitRepository.SaveChangesAsync();
            _logger.LogInformation("UnitService: Saved {Count} new units", processedUnits.Count);

            // Update total units count for floors
            var affectedFloors = processedUnits.Select(u => u.FloorId).Distinct();
            foreach (var floorId in affectedFloors)
            {
                var floor = await _floorRepository.GetByIdAsync(floorId);
                if (floor != null)
                {
                    var totalUnits = await _unitRepository.GetCountByFloorAsync(floorId);
                    _logger.LogInformation("UnitService: Updating floor {FloorId} total units to {TotalUnits}", floorId, totalUnits);
                    floor.TotalUnits = totalUnits;
                    await _floorRepository.UpdateAsync(floor);
                }
            }

            await _floorRepository.SaveChangesAsync();
            _logger.LogInformation("UnitService: Updated unit counts for {Count} floors", affectedFloors.Count());

            return result;
        }
    }
}
