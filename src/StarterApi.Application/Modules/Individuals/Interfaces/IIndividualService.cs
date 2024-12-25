using StarterApi.Application.Common.Models;
using StarterApi.Application.Modules.Individuals.DTOs;

namespace StarterApi.Application.Modules.Individuals.Interfaces
{
    public interface IIndividualService
    {
        Task<IndividualDto> CreateIndividualAsync(CreateIndividualDto dto);
        Task<IndividualDto> UpdateIndividualAsync(Guid id, UpdateIndividualDto dto);
        Task<IndividualDto> VerifyIndividualAsync(Guid id, VerifyIndividualDto dto);
        Task<bool> DeleteIndividualAsync(Guid id);
        Task<IndividualDto> GetIndividualByIdAsync(Guid id);
        Task<PagedResult<IndividualDto>> GetIndividualsAsync(QueryParameters parameters);
        Task<bool> ExistsByEmailAsync(string email);
        Task<bool> ExistsByPhoneNumberAsync(string phoneNumber);
        Task<bool> ExistsByIdProofAsync(string idProofType, string idProofNumber);
    }
}
