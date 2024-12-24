using AutoMapper;
using Microsoft.Extensions.Logging;
using StarterApi.Application.Common.Exceptions;
using StarterApi.Application.Modules.Blocks.Interfaces;
using StarterApi.Application.Modules.Floors.DTOs;
using StarterApi.Application.Modules.Floors.Interfaces;
using StarterApi.Domain.Entities;

namespace StarterApi.Application.Modules.Floors.Services
{
    public class FloorService : IFloorService
    {
        private readonly IFloorRepository _floorRepository;
        private readonly IBlockRepository _blockRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<FloorService> _logger;

        public FloorService(
            IFloorRepository floorRepository,
            IBlockRepository blockRepository,
            IMapper mapper,
            ILogger<FloorService> logger)
        {
            _floorRepository = floorRepository;
            _blockRepository = blockRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<FloorDto> CreateFloorAsync(CreateFloorDto dto)
        {
            var block = await _blockRepository.GetByIdAsync(dto.BlockId);
            if (block == null)
                throw new NotFoundException($"Block with ID {dto.BlockId} not found");

            if (await _floorRepository.ExistsAsync(dto.BlockId, dto.FloorNumber))
                throw new InvalidOperationException($"Floor number {dto.FloorNumber} already exists in this block");

            var floor = new Floor
            {
                BlockId = dto.BlockId,
                FloorNumber = dto.FloorNumber,
                FloorName = dto.FloorName,
                TotalUnits = 0
            };

            await _floorRepository.AddAsync(floor);
            await _floorRepository.SaveChangesAsync();

            // Update block's total floors count
            block.TotalFloors = await _floorRepository.GetFloorCountByBlockAsync(block.Id);
            await _blockRepository.UpdateAsync(block);
            await _blockRepository.SaveChangesAsync();

            var floorDto = _mapper.Map<FloorDto>(floor);
            floorDto.BlockName = block.Name;
            floorDto.BlockCode = block.Code;
            return floorDto;
        }

        public async Task<FloorDto> GetFloorByIdAsync(Guid id)
        {
            var floor = await _floorRepository.GetByIdAsync(id);
            if (floor == null)
                throw new NotFoundException($"Floor with ID {id} not found");

            var floorDto = _mapper.Map<FloorDto>(floor);
            floorDto.BlockName = floor.Block?.Name;
            floorDto.BlockCode = floor.Block?.Code;
            return floorDto;
        }

        public async Task<FloorDto> UpdateFloorAsync(Guid id, UpdateFloorDto dto)
        {
            var floor = await _floorRepository.GetByIdAsync(id);
            if (floor == null)
                throw new NotFoundException($"Floor with ID {id} not found");

            floor.FloorName = dto.FloorName;

            await _floorRepository.UpdateAsync(floor);
            await _floorRepository.SaveChangesAsync();

            var floorDto = _mapper.Map<FloorDto>(floor);
            floorDto.BlockName = floor.Block?.Name;
            floorDto.BlockCode = floor.Block?.Code;
            return floorDto;
        }

        public async Task<bool> DeleteFloorAsync(Guid id)
        {
            var floor = await _floorRepository.GetByIdAsync(id);
            if (floor == null)
                throw new NotFoundException($"Floor with ID {id} not found");

            // Add validation for active units if needed
            floor.IsActive = false;
            await _floorRepository.UpdateAsync(floor);
            await _floorRepository.SaveChangesAsync();

            // Update block's total floors count
            var block = await _blockRepository.GetByIdAsync(floor.BlockId);
            if (block != null)
            {
                block.TotalFloors = await _floorRepository.GetFloorCountByBlockAsync(block.Id);
                await _blockRepository.UpdateAsync(block);
                await _blockRepository.SaveChangesAsync();
            }

            return true;
        }

        public async Task<IEnumerable<FloorDto>> GetAllFloorsAsync()
        {
            var floors = await _floorRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<FloorDto>>(floors);
        }

        public async Task<IEnumerable<FloorDto>> GetFloorsByBlockAsync(Guid blockId)
        {
            var floors = await _floorRepository.GetByBlockIdAsync(blockId);
            return _mapper.Map<IEnumerable<FloorDto>>(floors);
        }

        public async Task<bool> ExistsByNumberAsync(Guid blockId, int floorNumber)
        {
            return await _floorRepository.ExistsAsync(blockId, floorNumber);
        }
    }
}
