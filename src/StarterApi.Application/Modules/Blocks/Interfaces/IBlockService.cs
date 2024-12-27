using StarterApi.Application.Common.Models;
using StarterApi.Application.Modules.Blocks.DTOs;

namespace StarterApi.Application.Modules.Blocks.Interfaces
{
    public interface IBlockService
    {
        Task<BlockDto> CreateBlockAsync(CreateBlockDto dto);
        Task<BlockDto> UpdateBlockAsync(Guid id, UpdateBlockDto dto);
        Task<bool> DeleteBlockAsync(Guid id);
        Task<BlockDto> GetBlockByIdAsync(Guid id);
        Task<PagedResult<BlockListDto>> GetBlocksAsync(QueryParameters parameters);
        Task<bool> ExistsByCodeAsync(string code, Guid societyId);
    }
}
