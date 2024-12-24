using StarterApi.Application.Common.Models;
using StarterApi.Domain.Entities;

namespace StarterApi.Application.Modules.Societies.Interfaces
{
    public interface ISocietyRepository
    {
        Task<Society> GetByIdAsync(Guid id);
        Task<PagedResult<Society>> GetPagedAsync(QueryParameters parameters);
        Task<Society> AddAsync(Society society);
        Task<Society> UpdateAsync(Society society);
        Task SaveChangesAsync();
        Task<Society> GetByRegistrationNumberAsync(string registrationNumber);
        Task<bool> ExistsAsync(string registrationNumber);
    }
}
