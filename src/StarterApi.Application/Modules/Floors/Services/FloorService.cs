using AutoMapper;
using Microsoft.Extensions.Logging;
using StarterApi.Application.Common.Exceptions;
using StarterApi.Application.Common.Models;
using StarterApi.Application.Modules.Floors.DTOs;
using StarterApi.Application.Modules.Floors.Interfaces;
using StarterApi.Domain.Entities;

namespace StarterApi.Application.Modules.Floors.Services
{
    public class FloorService : IFloorService
    {
        private readonly IFloorRepository _floorRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<FloorService> _logger;

        public FloorService(
            IFloorRepository floorRepository,
            IMapper mapper,
            ILogger<FloorService> logger)
        {
            _floorRepository = floorRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<FloorDto> CreateFloorAsync(CreateFloorDto dto)
        {
            if (await _floorRepository.ExistsAsync(dto.FloorNumber, dto.BlockId))
                throw new InvalidOperationException($"Floor number {dto.FloorNumber} already exists in this block");

            var floor = new Floor
            {
                FloorNumber = dto.FloorNumber,
                FloorName = dto.FloorName,
                BlockId = dto.BlockId,
                TotalUnits = 0
            };

            await _floorRepository.AddAsync(floor);
            await _floorRepository.SaveChangesAsync();

            return _mapper.Map<FloorDto>(floor);
        }

        public async Task<FloorDto> GetFloorByIdAsync(Guid id)
        {
            var floor = await _floorRepository.GetByIdAsync(id);
            if (floor == null)
                throw new NotFoundException($"Floor with ID {id} not found");

            return _mapper.Map<FloorDto>(floor);
        }

        public async Task<FloorDto> UpdateFloorAsync(Guid id, UpdateFloorDto dto)
        {
            var floor = await _floorRepository.GetByIdAsync(id);
            if (floor == null)
                throw new NotFoundException($"Floor with ID {id} not found");

            floor.FloorName = dto.FloorName;

            await _floorRepository.UpdateAsync(floor);
            await _floorRepository.SaveChangesAsync();

            return _mapper.Map<FloorDto>(floor);
        }

        public async Task<bool> DeleteFloorAsync(Guid id)
        {
            var floor = await _floorRepository.GetByIdAsync(id);
            if (floor == null)
                throw new NotFoundException($"Floor with ID {id} not found");

            floor.IsActive = false;
            await _floorRepository.UpdateAsync(floor);
            await _floorRepository.SaveChangesAsync();

            return true;
        }

        public async Task<PagedResult<FloorListDto>> GetFloorsAsync(QueryParameters parameters)
        {
            var pagedFloors = await _floorRepository.GetPagedAsync(parameters);
            
            var floorDtos = _mapper.Map<IEnumerable<FloorListDto>>(pagedFloors.Items);
            
            return new PagedResult<FloorListDto>
            {
                Items = floorDtos,
                TotalItems = pagedFloors.TotalItems,
                PageNumber = pagedFloors.PageNumber,
                PageSize = pagedFloors.PageSize,
                TotalPages = pagedFloors.TotalPages,
                HasNextPage = pagedFloors.HasNextPage,
                HasPreviousPage = pagedFloors.HasPreviousPage
            };
        }

        public async Task<bool> ExistsByNumberAsync(int number, Guid blockId)
        {
            return await _floorRepository.ExistsAsync(number, blockId);
        }
    }
}
