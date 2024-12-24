using StarterApi.Domain.Entities;

namespace StarterApi.Application.Modules.Blocks.Interfaces
{
    public interface IBlockRepository
    {
        Task<Block> GetByIdAsync(Guid id);
        Task<IEnumerable<Block>> GetAllAsync();
        Task<IEnumerable<Block>> GetBySocietyIdAsync(Guid societyId);
        Task<Block> AddAsync(Block block);
        Task<Block> UpdateAsync(Block block);
        Task SaveChangesAsync();
        Task<bool> ExistsAsync(Guid societyId, string code);
        Task<int> GetBlockCountBySocietyAsync(Guid societyId);
    }
}
