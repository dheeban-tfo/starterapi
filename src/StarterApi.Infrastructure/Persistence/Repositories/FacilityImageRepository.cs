using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StarterApi.Application.Common.Interfaces;
using StarterApi.Application.Modules.Facilities.Interfaces;
using StarterApi.Domain.Entities;

namespace StarterApi.Infrastructure.Persistence.Repositories
{
    public class FacilityImageRepository : IFacilityImageRepository
    {
        private readonly ITenantDbContext _context;

        public FacilityImageRepository(ITenantDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<FacilityImage>> GetByFacilityIdAsync(Guid facilityId)
        {
            return await _context.FacilityImages
                .Where(i => i.FacilityId == facilityId && i.IsActive)
                .OrderBy(i => i.DisplayOrder)
                .ToListAsync();
        }

        public async Task<FacilityImage> GetByIdAsync(Guid id)
        {
            return await _context.FacilityImages
                .FirstOrDefaultAsync(i => i.Id == id && i.IsActive);
        }

        public async Task<FacilityImage> AddAsync(FacilityImage image)
        {
            await _context.FacilityImages.AddAsync(image);
            await SaveChangesAsync();
            return image;
        }

        public async Task<FacilityImage> UpdateAsync(FacilityImage image)
        {
            _context.FacilityImages.Update(image);
            await SaveChangesAsync();
            return image;
        }

        public async Task<bool> DeleteAsync(FacilityImage image)
        {
            image.IsActive = false;
            _context.FacilityImages.Update(image);
            await SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateRangeAsync(IEnumerable<FacilityImage> images)
        {
            _context.FacilityImages.UpdateRange(images);
            await SaveChangesAsync();
            return true;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
} 