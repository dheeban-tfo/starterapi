using StarterApi.Application.Common.Models;
using StarterApi.Domain.Entities;

namespace StarterApi.Application.Modules.Individuals.Interfaces
{
    public interface IIndividualRepository
    {
        Task<Individual> GetByIdAsync(Guid id);
        Task<PagedResult<Individual>> GetPagedAsync(QueryParameters parameters);
        Task<Individual> AddAsync(Individual individual);
        Task<Individual> UpdateAsync(Individual individual);
        Task SaveChangesAsync();
        Task<bool> ExistsByEmailAsync(string email);
        Task<bool> ExistsByPhoneNumberAsync(string phoneNumber);
        Task<bool> ExistsByIdProofAsync(string idProofType, string idProofNumber);
        Task<Individual> GetByEmailAsync(string email);
        Task<Individual> GetByPhoneNumberAsync(string phoneNumber);
        Task<Individual> GetByIdProofAsync(string idProofType, string idProofNumber);
    }
}
