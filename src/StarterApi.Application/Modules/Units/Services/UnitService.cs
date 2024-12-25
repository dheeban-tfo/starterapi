using AutoMapper;
using Microsoft.Extensions.Logging;
using StarterApi.Application.Common.Exceptions;
using StarterApi.Application.Common.Models;
using StarterApi.Application.Modules.Units.DTOs;
using StarterApi.Application.Modules.Units.Interfaces;
using StarterApi.Domain.Entities;

namespace StarterApi.Application.Modules.Units.Services
{
    public class UnitService : IUnitService
    {
        private readonly IUnitRepository _unitRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UnitService> _logger;
        private readonly IFloorRepository _floorRepository;
        private readonly IOwnerRepository _ownerRepository;

        public UnitService(
            IUnitRepository unitRepository,
            IMapper mapper,
            ILogger<UnitService> logger,
            IFloorRepository floorRepository,
            IOwnerRepository ownerRepository)
        {
            _unitRepository = unitRepository;
            _mapper = mapper;
            _logger = logger;
            _floorRepository = floorRepository;
            _ownerRepository = ownerRepository;
        }

        public async Task<UnitDto> CreateUnitAsync(CreateUnitDto dto)
        {
            // Validate floor exists
            var floor = await _floorRepository.GetByIdAsync(dto.FloorId);
            if (floor == null)
                throw new NotFoundException($"Floor with ID {dto.FloorId} not found");

            // Validate owner exists if provided
            if (dto.CurrentOwnerId.HasValue)
            {
                var owner = await _ownerRepository.GetByIdAsync(dto.CurrentOwnerId.Value);
                if (owner == null)
                    throw new NotFoundException($"Owner with ID {dto.CurrentOwnerId} not found");
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
            unit.Status = dto.Status;
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

        public async Task<PagedResult<UnitDto>> GetUnitsAsync(QueryParameters parameters)
        {
            var pagedUnits = await _unitRepository.GetPagedAsync(parameters);
            
            var unitDtos = _mapper.Map<IEnumerable<UnitDto>>(pagedUnits.Items);
            
            return new PagedResult<UnitDto>
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
    }
}
