using AutoMapper;
using Microsoft.Extensions.Logging;
using StarterApi.Application.Common.Exceptions;
using StarterApi.Application.Common.Models;
using StarterApi.Application.Modules.Blocks.DTOs;
using StarterApi.Application.Modules.Blocks.Interfaces;
using StarterApi.Domain.Entities;

namespace StarterApi.Application.Modules.Blocks.Services
{
    public class BlockService : IBlockService
    {
        private readonly IBlockRepository _blockRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<BlockService> _logger;

        public BlockService(
            IBlockRepository blockRepository,
            IMapper mapper,
            ILogger<BlockService> logger)
        {
            _blockRepository = blockRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BlockDto> CreateBlockAsync(CreateBlockDto dto)
        {
            if (await _blockRepository.ExistsAsync(dto.Code, dto.SocietyId))
                throw new InvalidOperationException($"Block with code {dto.Code} already exists in the society");

            var block = new Block
            {
                Code = dto.Code,
                Name = dto.Name,
                //Description = dto.Description,
                SocietyId = dto.SocietyId,
                TotalFloors = 0
            };

            await _blockRepository.AddAsync(block);
            await _blockRepository.SaveChangesAsync();

            return _mapper.Map<BlockDto>(block);
        }

        public async Task<BlockDto> GetBlockByIdAsync(Guid id)
        {
            var block = await _blockRepository.GetByIdAsync(id);
            if (block == null)
                throw new NotFoundException($"Block with ID {id} not found");

            return _mapper.Map<BlockDto>(block);
        }

        public async Task<BlockDto> UpdateBlockAsync(Guid id, UpdateBlockDto dto)
        {
            var block = await _blockRepository.GetByIdAsync(id);
            if (block == null)
                throw new NotFoundException($"Block with ID {id} not found");

            block.Name = dto.Name;
           // block.Description = dto.Description;

            await _blockRepository.UpdateAsync(block);
            await _blockRepository.SaveChangesAsync();

            return _mapper.Map<BlockDto>(block);
        }

        public async Task<bool> DeleteBlockAsync(Guid id)
        {
            var block = await _blockRepository.GetByIdAsync(id);
            if (block == null)
                throw new NotFoundException($"Block with ID {id} not found");

            block.IsActive = false;
            await _blockRepository.UpdateAsync(block);
            await _blockRepository.SaveChangesAsync();

            return true;
        }

        public async Task<PagedResult<BlockListDto>> GetBlocksAsync(QueryParameters parameters)
        {
            var pagedBlocks = await _blockRepository.GetPagedAsync(parameters);
            
            var blockDtos = _mapper.Map<IEnumerable<BlockListDto>>(pagedBlocks.Items);
            
            return new PagedResult<BlockListDto>
            {
                Items = blockDtos,
                TotalItems = pagedBlocks.TotalItems,
                PageNumber = pagedBlocks.PageNumber,
                PageSize = pagedBlocks.PageSize,
                TotalPages = pagedBlocks.TotalPages,
                HasNextPage = pagedBlocks.HasNextPage,
                HasPreviousPage = pagedBlocks.HasPreviousPage
            };
        }

        public async Task<BlockDto> GetBlockByCodeAsync(string code)
        {
            var block = await _blockRepository.GetByCodeAsync(code);
            if (block == null)
                throw new NotFoundException($"Block with code {code} not found");

            return _mapper.Map<BlockDto>(block);
        }

        public async Task<bool> ExistsByCodeAsync(string code, Guid societyId)
        {
            return await _blockRepository.ExistsAsync(code, societyId);
        }
    }
}
