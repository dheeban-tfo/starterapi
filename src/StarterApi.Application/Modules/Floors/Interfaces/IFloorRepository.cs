using StarterApi.Domain.Entities;

namespace StarterApi.Application.Modules.Floors.Interfaces
{
    public interface IFloorRepository
    {
        Task<Floor> GetByIdAsync(Guid id);
        Task<IEnumerable<Floor>> GetAllAsync();
        Task<IEnumerable<Floor>> GetByBlockIdAsync(Guid blockId);
        Task<Floor> AddAsync(Floor floor);
        Task<Floor> UpdateAsync(Floor floor);
        Task SaveChangesAsync();
        Task<bool> ExistsAsync(Guid blockId, int floorNumber);
        Task<int> GetFloorCountByBlockAsync(Guid blockId);
        Task<Floor> GetByBlockAndNumberAsync(Guid blockId, int floorNumber);
    }
}
