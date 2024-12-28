using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StarterApi.Domain.Entities;

namespace StarterApi.Application.Modules.Facilities.Interfaces
{
    public interface IFacilityImageRepository
    {
        Task<IEnumerable<FacilityImage>> GetByFacilityIdAsync(Guid facilityId);
        Task<FacilityImage> GetByIdAsync(Guid id);
        Task<FacilityImage> AddAsync(FacilityImage image);
        Task<FacilityImage> UpdateAsync(FacilityImage image);
        Task<bool> DeleteAsync(FacilityImage image);
        Task<bool> UpdateRangeAsync(IEnumerable<FacilityImage> images);
        Task SaveChangesAsync();
    }
} 