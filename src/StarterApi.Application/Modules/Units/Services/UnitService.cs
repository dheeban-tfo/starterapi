using AutoMapper;
using Microsoft.Extensions.Logging;
using StarterApi.Application.Common.Exceptions;
using StarterApi.Application.Modules.Floors.Interfaces;
using StarterApi.Application.Modules.Units.DTOs;
using StarterApi.Application.Modules.Units.Interfaces;
using StarterApi.Domain.Entities;

namespace StarterApi.Application.Modules.Units.Services
{
    public class UnitService : IUnitService
    {
        private readonly IUnitRepository _unitRepository;
        private readonly IFloorRepository _floorRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UnitService> _logger;

        public UnitService(
            IUnitRepository unitRepository,
            IFloorRepository floorRepository,
            IMapper mapper,
            ILogger<UnitService> logger)
        {
            _unitRepository = unitRepository;
            _floorRepository = floorRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<UnitDto> CreateUnitAsync(CreateUnitDto dto)
        {
            var floor = await _floorRepository.GetByIdAsync(dto.FloorId);
            if (floor == null)
                throw new NotFoundException($"Floor with ID {dto.FloorId} not found");

            if (await _unitRepository.ExistsAsync(dto.FloorId, dto.UnitNumber))
                throw new InvalidOperationException($"Unit number {dto.UnitNumber} already exists on this floor");

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
                MonthlyMaintenanceFee = CalculateMaintenanceFee(dto.BuiltUpArea, floor.Block.MaintenanceChargePerSqft)
            };

            await _unitRepository.AddAsync(unit);
            await _unitRepository.SaveChangesAsync();

            // Update floor's total units count
            floor.TotalUnits = await _unitRepository.GetUnitCountByFloorAsync(floor.Id);
            await _floorRepository.UpdateAsync(floor);
            await _floorRepository.SaveChangesAsync();

            var unitDto = _mapper.Map<UnitDto>(unit);
            unitDto.FloorName = floor.FloorName;
            unitDto.BlockName = floor.Block?.Name;
            unitDto.BlockCode = floor.Block?.Code;
            return unitDto;
        }

        public async Task<UnitDto> GetUnitByIdAsync(Guid id)
        {
            var unit = await _unitRepository.GetByIdAsync(id);
            if (unit == null)
                throw new NotFoundException($"Unit with ID {id} not found");

            var unitDto = _mapper.Map<UnitDto>(unit);
            unitDto.FloorName = unit.Floor?.FloorName;
            unitDto.BlockName = unit.Floor?.Block?.Name;
            unitDto.BlockCode = unit.Floor?.Block?.Code;
            unitDto.CurrentOwnerName = unit.CurrentOwner?.Name;
            return unitDto;
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
            unit.Status = dto.Status;
            unit.CurrentOwnerId = dto.CurrentOwnerId;

            // Recalculate maintenance fee if built-up area changed
            if (unit.BuiltUpArea != dto.BuiltUpArea)
            {
                unit.MonthlyMaintenanceFee = CalculateMaintenanceFee(dto.BuiltUpArea, unit.Floor.Block.MaintenanceChargePerSqft);
            }

            await _unitRepository.UpdateAsync(unit);
            await _unitRepository.SaveChangesAsync();

            var unitDto = _mapper.Map<UnitDto>(unit);
            unitDto.FloorName = unit.Floor?.FloorName;
            unitDto.BlockName = unit.Floor?.Block?.Name;
            unitDto.BlockCode = unit.Floor?.Block?.Code;
            unitDto.CurrentOwnerName = unit.CurrentOwner?.Name;
            return unitDto;
        }

        public async Task<bool> DeleteUnitAsync(Guid id)
        {
            var unit = await _unitRepository.GetByIdAsync(id);
            if (unit == null)
                throw new NotFoundException($"Unit with ID {id} not found");

            // Add validation for active residents if needed
            unit.IsActive = false;
            await _unitRepository.UpdateAsync(unit);
            await _unitRepository.SaveChangesAsync();

            // Update floor's total units count
            var floor = await _floorRepository.GetByIdAsync(unit.FloorId);
            if (floor != null)
            {
                floor.TotalUnits = await _unitRepository.GetUnitCountByFloorAsync(floor.Id);
                await _floorRepository.UpdateAsync(floor);
                await _floorRepository.SaveChangesAsync();
            }

            return true;
        }

        public async Task<IEnumerable<UnitDto>> GetAllUnitsAsync()
        {
            var units = await _unitRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<UnitDto>>(units);
        }

        public async Task<IEnumerable<UnitDto>> GetUnitsByFloorAsync(Guid floorId)
        {
            var units = await _unitRepository.GetByFloorIdAsync(floorId);
            return _mapper.Map<IEnumerable<UnitDto>>(units);
        }

        public async Task<bool> ExistsByNumberAsync(Guid floorId, string unitNumber)
        {
            return await _unitRepository.ExistsAsync(floorId, unitNumber);
        }

        private decimal CalculateMaintenanceFee(decimal builtUpArea, decimal maintenanceChargePerSqft)
        {
            return builtUpArea * maintenanceChargePerSqft;
        }
    }
}
