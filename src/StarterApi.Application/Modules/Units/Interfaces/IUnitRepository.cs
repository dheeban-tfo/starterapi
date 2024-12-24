using StarterApi.Domain.Entities;

namespace StarterApi.Application.Modules.Units.Interfaces
{
    public interface IUnitRepository
    {
        Task<Unit> GetByIdAsync(Guid id);
        Task<IEnumerable<Unit>> GetAllAsync();
        Task<IEnumerable<Unit>> GetByFloorIdAsync(Guid floorId);
        Task<Unit> AddAsync(Unit unit);
        Task<Unit> UpdateAsync(Unit unit);
        Task SaveChangesAsync();
        Task<bool> ExistsAsync(Guid floorId, string unitNumber);
        Task<int> GetUnitCountByFloorAsync(Guid floorId);
        Task<Unit> GetByFloorAndNumberAsync(Guid floorId, string unitNumber);
        Task<decimal> GetTotalMaintenanceByFloorAsync(Guid floorId);
    }
}
