using System.Collections.Generic;
using System.Threading.Tasks;
using StarterApi.Application.Common.Models;

namespace StarterApi.Application.Common.Interfaces;

public interface ILookupService
{
    Task<IEnumerable<IndividualLookupDto>> GetIndividualLookupsAsync(LookupRequestDto request);
}
