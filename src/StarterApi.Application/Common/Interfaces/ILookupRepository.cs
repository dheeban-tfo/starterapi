using System.Collections.Generic;
using System.Threading.Tasks;
using StarterApi.Domain.Entities;

namespace StarterApi.Application.Common.Interfaces;

public interface ILookupRepository
{
    Task<IEnumerable<Individual>> GetIndividualLookupsAsync(string searchTerm, int maxResults);
    Task<IEnumerable<Unit>> GetUnitLookupsAsync(string searchTerm, int maxResults);
    Task<IEnumerable<Block>> GetBlockLookupsAsync(string searchTerm, int maxResults);
    Task<IEnumerable<Floor>> GetFloorLookupsAsync(string searchTerm, int maxResults);
    Task<IEnumerable<Resident>> GetResidentLookupsAsync(string searchTerm, int maxResults);
    Task<IEnumerable<TenantUser>> GetUserLookupsAsync(string searchTerm, int maxResults);
    Task<IEnumerable<Society>> GetSocietyLookupsAsync(string searchTerm, int maxResults);
}
