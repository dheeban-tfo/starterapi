using StarterApi.Application.Modules.Blocks.DTOs;

namespace StarterApi.Application.Modules.Blocks.Interfaces
{
    public interface IBlockService
    {
        Task<BlockDto> CreateBlockAsync(CreateBlockDto dto);
        Task<BlockDto> UpdateBlockAsync(Guid id, UpdateBlockDto dto);
        Task<bool> DeleteBlockAsync(Guid id);
        Task<BlockDto> GetBlockByIdAsync(Guid id);
        Task<IEnumerable<BlockDto>> GetAllBlocksAsync();
        Task<IEnumerable<BlockDto>> GetBlocksBySocietyAsync(Guid societyId);
        Task<bool> ExistsByCodeAsync(Guid societyId, string code);
    }
}
