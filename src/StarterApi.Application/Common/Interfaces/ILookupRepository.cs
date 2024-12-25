using System.Collections.Generic;
using System.Threading.Tasks;
using StarterApi.Domain.Entities;

namespace StarterApi.Application.Common.Interfaces;

public interface ILookupRepository
{
    Task<IEnumerable<Individual>> GetIndividualLookupsAsync(string searchTerm, int maxResults);
}
