using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using StarterApi.Application.Common.Interfaces;
using StarterApi.Application.Common.Models;

namespace StarterApi.Application.Common.Services;

public class LookupService : ILookupService
{
    private readonly ILookupRepository _repository;
    private readonly IMapper _mapper;

    public LookupService(ILookupRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<IndividualLookupDto>> GetIndividualLookupsAsync(LookupRequestDto request)
    {
        var individuals = await _repository.GetIndividualLookupsAsync(request.SearchTerm, request.MaxResults);
        return _mapper.Map<IEnumerable<IndividualLookupDto>>(individuals);
    }
}
