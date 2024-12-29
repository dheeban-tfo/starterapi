using StarterApi.Application.Common.Models;
using StarterApi.Domain.Entities;

namespace StarterApi.Application.Modules.Facilities.Interfaces
{
    public interface IFacilityRepository
    {
        Task<Facility> GetByIdAsync(Guid id);
        Task<Facility> GetByIdWithDetailsAsync(Guid id);
        Task<PagedResult<Facility>> GetPagedAsync(QueryParameters parameters);
        Task<IEnumerable<Facility>> GetAllAsync();
        Task<IEnumerable<Facility>> GetBySocietyIdAsync(Guid societyId);
        Task<bool> ExistsAsync(string name, Guid? excludeId = null);
        Task<int> GetActiveBookingsCountAsync(Guid facilityId);
        Task<Dictionary<Guid, int>> GetActiveBookingsCountForFacilitiesAsync(List<Guid> facilityIds);
        Task<Facility> AddAsync(Facility entity);
        Task<Facility> UpdateAsync(Facility entity);
        Task<bool> DeleteAsync(Facility entity);
        Task SaveChangesAsync();
    }
} 