using StarterApi.Application.Common.Models;
using StarterApi.Domain.Entities;

namespace StarterApi.Application.Modules.Floors.Interfaces
{
    public interface IFloorRepository
    {
        Task<Floor> GetByIdAsync(Guid id);
        Task<PagedResult<Floor>> GetPagedAsync(QueryParameters parameters);
        Task<Floor> AddAsync(Floor floor);
        Task<Floor> UpdateAsync(Floor floor);
        Task SaveChangesAsync();
        Task<Floor> GetByNumberAsync(int number, Guid blockId);
        Task<bool> ExistsAsync(int number, Guid blockId);
        Task<int> GetFloorCountByBlockAsync(Guid blockId);
        Task<Floor> GetByNumberAndBlockAsync(int floorNumber, Guid blockId);
    }
}
