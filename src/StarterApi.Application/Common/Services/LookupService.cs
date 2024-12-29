using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StarterApi.Application.Common.Interfaces;
using StarterApi.Application.Common.Models;

namespace StarterApi.Application.Common.Services
{
    public class LookupService : ILookupService
    {
        private readonly ILookupRepository _lookupRepository;

        public LookupService(ILookupRepository lookupRepository)
        {
            _lookupRepository = lookupRepository;
        }

        public async Task<IEnumerable<IndividualLookupDto>> GetIndividualLookupsAsync(LookupRequestDto request)
        {
            var individuals = await _lookupRepository.GetIndividualLookupsAsync(request.SearchTerm, request.MaxResults);

            return individuals.Select(i => new IndividualLookupDto
            {
                Id = i.Id,
                FullName = i.FullName,
                Email = i.Email,
                PhoneNumber = i.PhoneNumber
            });
        }

        public async Task<IEnumerable<UnitLookupDto>> GetUnitLookupsAsync(LookupRequestDto request)
        {
            var units = await _lookupRepository.GetUnitLookupsAsync(request.SearchTerm, request.MaxResults);

            return units.Select(u => new UnitLookupDto
            {
                Id = u.Id,
                UnitNumber = u.UnitNumber,
                FloorName = u.Floor?.FloorName,
                BlockName = u.Floor?.Block?.Name
            });
        }

        public async Task<IEnumerable<BlockLookupDto>> GetBlockLookupsAsync(LookupRequestDto request)
        {
            var blocks = await _lookupRepository.GetBlockLookupsAsync(request.SearchTerm, request.MaxResults);

            return blocks.Select(b => new BlockLookupDto
            {
                Id = b.Id,
                Name = b.Name,
                Code = b.Code,
                SocietyName = b.Society?.Name
            });
        }

        public async Task<IEnumerable<FloorLookupDto>> GetFloorLookupsAsync(LookupRequestDto request)
        {
            var floors = await _lookupRepository.GetFloorLookupsAsync(request.SearchTerm, request.MaxResults);

            return floors.Select(f => new FloorLookupDto
            {
                Id = f.Id,
                FloorName = f.FloorName,
                FloorNumber = f.FloorNumber,
                BlockName = f.Block?.Name
            });
        }

        public async Task<IEnumerable<ResidentLookupDto>> GetResidentLookupsAsync(LookupRequestDto request)
        {
            var residents = await _lookupRepository.GetResidentLookupsAsync(request.SearchTerm, request.MaxResults);

            return residents.Select(r => new ResidentLookupDto
            {
                Id = r.Id,
                FullName = r.Individual?.FullName,
                UnitNumber = r.Unit?.UnitNumber,
                ResidentType = r.ResidentType,
                Status = r.Status
            });
        }

        public async Task<IEnumerable<UserLookupDto>> GetUserLookupsAsync(LookupRequestDto request)
        {
            var users = await _lookupRepository.GetUserLookupsAsync(request.SearchTerm, request.MaxResults);

            return users.Select(u => new UserLookupDto
            {
                Id = u.Id,
                FullName = u.FirstName + " " + u.LastName,
                Email = u.Email,
                PhoneNumber = u.MobileNumber,
                RoleName = u.Role?.Name
            });
        }

        public async Task<IEnumerable<SocietyLookupDto>> GetSocietyLookupsAsync(LookupRequestDto request)
        {
            var societies = await _lookupRepository.GetSocietyLookupsAsync(request.SearchTerm, request.MaxResults);

            return societies.Select(s => new SocietyLookupDto
            {
                Id = s.Id,
                Name = s.Name,
                RegistrationNumber = s.RegistrationNumber,
                City = s.City,
                State = s.State
            });
        }

        public async Task<IEnumerable<FacilityLookupDto>> GetFacilityLookupsAsync(LookupRequestDto request)
        {
            var facilities = await _lookupRepository.GetFacilityLookupsAsync(request.SearchTerm, request.MaxResults);

            return facilities.Select(f => new FacilityLookupDto
            {
                Id = f.Id,
                Name = f.Name,
                Type = f.Type.ToString(),
                Location = f.Location,
                Status = f.Status.ToString()
            });
        }
    }
}
