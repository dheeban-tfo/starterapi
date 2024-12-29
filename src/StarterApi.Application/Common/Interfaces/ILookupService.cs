using System.Collections.Generic;
using System.Threading.Tasks;
using StarterApi.Application.Common.Models;

namespace StarterApi.Application.Common.Interfaces;

public interface ILookupService
{
    Task<IEnumerable<IndividualLookupDto>> GetIndividualLookupsAsync(LookupRequestDto request);
    Task<IEnumerable<UnitLookupDto>> GetUnitLookupsAsync(LookupRequestDto request);
    Task<IEnumerable<BlockLookupDto>> GetBlockLookupsAsync(LookupRequestDto request);
    Task<IEnumerable<FloorLookupDto>> GetFloorLookupsAsync(LookupRequestDto request);
    Task<IEnumerable<ResidentLookupDto>> GetResidentLookupsAsync(LookupRequestDto request);
    Task<IEnumerable<UserLookupDto>> GetUserLookupsAsync(LookupRequestDto request);
    Task<IEnumerable<SocietyLookupDto>> GetSocietyLookupsAsync(LookupRequestDto request);
}
