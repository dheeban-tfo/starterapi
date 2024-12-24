using StarterApi.Application.Modules.Societies.DTOs;

namespace StarterApi.Application.Modules.Societies.Interfaces
{
    public interface ISocietyService
    {
        Task<SocietyDto> CreateSocietyAsync(CreateSocietyDto dto);
        Task<SocietyDto> UpdateSocietyAsync(Guid id, UpdateSocietyDto dto);
        Task<bool> DeleteSocietyAsync(Guid id);
        Task<SocietyDto> GetSocietyByIdAsync(Guid id);
        Task<IEnumerable<SocietyDto>> GetAllSocietiesAsync();
        Task<bool> ExistsByRegistrationNumberAsync(string registrationNumber);
    }
}
