using AutoMapper;
using Microsoft.Extensions.Logging;
using StarterApi.Application.Common.Exceptions;
using StarterApi.Application.Modules.Blocks.DTOs;
using StarterApi.Application.Modules.Blocks.Interfaces;
using StarterApi.Application.Modules.Societies.Interfaces;
using StarterApi.Domain.Entities;

namespace StarterApi.Application.Modules.Blocks.Services
{
    public class BlockService : IBlockService
    {
        private readonly IBlockRepository _blockRepository;
        private readonly ISocietyRepository _societyRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<BlockService> _logger;

        public BlockService(
            IBlockRepository blockRepository,
            ISocietyRepository societyRepository,
            IMapper mapper,
            ILogger<BlockService> logger)
        {
            _blockRepository = blockRepository;
            _societyRepository = societyRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BlockDto> CreateBlockAsync(CreateBlockDto dto)
        {
            var society = await _societyRepository.GetByIdAsync(dto.SocietyId);
            if (society == null)
                throw new NotFoundException($"Society with ID {dto.SocietyId} not found");

            if (await _blockRepository.ExistsAsync(dto.SocietyId, dto.Code))
                throw new InvalidOperationException($"Block with code {dto.Code} already exists in this society");

            var block = new Block
            {
                SocietyId = dto.SocietyId,
                Name = dto.Name,
                Code = dto.Code,
                MaintenanceChargePerSqft = dto.MaintenanceChargePerSqft,
                TotalFloors = 0
            };

            await _blockRepository.AddAsync(block);
            await _blockRepository.SaveChangesAsync();

            // Update society's total blocks count
            society.TotalBlocks = await _blockRepository.GetBlockCountBySocietyAsync(society.Id);
            await _societyRepository.UpdateAsync(society);
            await _societyRepository.SaveChangesAsync();

            var blockDto = _mapper.Map<BlockDto>(block);
            blockDto.SocietyName = society.Name;
            return blockDto;
        }

        public async Task<BlockDto> GetBlockByIdAsync(Guid id)
        {
            var block = await _blockRepository.GetByIdAsync(id);
            if (block == null)
                throw new NotFoundException($"Block with ID {id} not found");

            var blockDto = _mapper.Map<BlockDto>(block);
            blockDto.SocietyName = block.Society?.Name;
            return blockDto;
        }

        public async Task<BlockDto> UpdateBlockAsync(Guid id, UpdateBlockDto dto)
        {
            var block = await _blockRepository.GetByIdAsync(id);
            if (block == null)
                throw new NotFoundException($"Block with ID {id} not found");

            if (block.Code != dto.Code && await _blockRepository.ExistsAsync(block.SocietyId, dto.Code))
                throw new InvalidOperationException($"Block with code {dto.Code} already exists in this society");

            block.Name = dto.Name;
            block.Code = dto.Code;
            block.MaintenanceChargePerSqft = dto.MaintenanceChargePerSqft;

            await _blockRepository.UpdateAsync(block);
            await _blockRepository.SaveChangesAsync();

            var blockDto = _mapper.Map<BlockDto>(block);
            blockDto.SocietyName = block.Society?.Name;
            return blockDto;
        }

        public async Task<bool> DeleteBlockAsync(Guid id)
        {
            var block = await _blockRepository.GetByIdAsync(id);
            if (block == null)
                throw new NotFoundException($"Block with ID {id} not found");

            // Add validation for active floors/units if needed
            block.IsActive = false;
            await _blockRepository.UpdateAsync(block);
            await _blockRepository.SaveChangesAsync();

            // Update society's total blocks count
            var society = await _societyRepository.GetByIdAsync(block.SocietyId);
            if (society != null)
            {
                society.TotalBlocks = await _blockRepository.GetBlockCountBySocietyAsync(society.Id);
                await _societyRepository.UpdateAsync(society);
                await _societyRepository.SaveChangesAsync();
            }

            return true;
        }

        public async Task<IEnumerable<BlockDto>> GetAllBlocksAsync()
        {
            var blocks = await _blockRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<BlockDto>>(blocks);
        }

        public async Task<IEnumerable<BlockDto>> GetBlocksBySocietyAsync(Guid societyId)
        {
            var blocks = await _blockRepository.GetBySocietyIdAsync(societyId);
            return _mapper.Map<IEnumerable<BlockDto>>(blocks);
        }

        public async Task<bool> ExistsByCodeAsync(Guid societyId, string code)
        {
            return await _blockRepository.ExistsAsync(societyId, code);
        }
    }
}
