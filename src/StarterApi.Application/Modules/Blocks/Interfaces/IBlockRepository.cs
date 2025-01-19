using StarterApi.Application.Common.Models;
using StarterApi.Domain.Entities;

namespace StarterApi.Application.Modules.Blocks.Interfaces
{
    public interface IBlockRepository
    {
        Task<Block> GetByIdAsync(Guid id);
        Task<PagedResult<Block>> GetPagedAsync(QueryParameters parameters);
        Task<Block> AddAsync(Block block);
        Task<Block> UpdateAsync(Block block);
        Task SaveChangesAsync();
        Task<Block> GetByCodeAsync(string code);
        Task<bool> ExistsAsync(string code, Guid societyId);
        Task<int> GetBlockCountBySocietyAsync(Guid societyId);
    }
}
