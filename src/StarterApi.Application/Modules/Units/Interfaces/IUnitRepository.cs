using StarterApi.Application.Common.Models;
using StarterApi.Domain.Entities;

namespace StarterApi.Application.Modules.Units.Interfaces
{
    public interface IUnitRepository
    {
        Task<Unit> GetByIdAsync(Guid id);
        Task<PagedResult<Unit>> GetPagedAsync(QueryParameters parameters);
        Task<Unit> AddAsync(Unit unit);
        Task<Unit> UpdateAsync(Unit unit);
        Task SaveChangesAsync();
        Task<Unit> GetByNumberAsync(string number, Guid floorId);
        Task<bool> ExistsAsync(string number, Guid floorId);
        Task<int> GetUnitCountByFloorAsync(Guid floorId);
        Task<Owner> GetOwnerByIdAsync(Guid id);
        Task<Individual> GetIndividualByIdAsync(Guid id);
        Task<Owner> GetOwnerByIndividualIdAsync(Guid individualId);
        Task<Owner> AddOwnerAsync(Owner owner);
    }
}
