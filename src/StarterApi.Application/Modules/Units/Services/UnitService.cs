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

        public UnitService(
            IUnitRepository unitRepository,
            IMapper mapper,
            ILogger<UnitService> logger)
        {
            _unitRepository = unitRepository;
            _mapper = mapper;
            _logger = logger;
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
            //unit.Status = dto.Status;
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
