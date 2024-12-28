using Microsoft.EntityFrameworkCore;
using StarterApi.Application.Common.Extensions;
using StarterApi.Application.Common.Interfaces;
using StarterApi.Application.Common.Models;
using StarterApi.Application.Modules.Facilities.Interfaces;
using StarterApi.Domain.Entities;

namespace StarterApi.Infrastructure.Persistence.Repositories
{
    public class FacilityBlackoutDateRepository : IFacilityBlackoutDateRepository
    {
        private readonly ITenantDbContext _context;

        public FacilityBlackoutDateRepository(ITenantDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<FacilityBlackoutDate>> GetPagedAsync(Guid facilityId, QueryParameters parameters)
        {
            var query = _context.FacilityBlackoutDates
                .Where(b => b.FacilityId == facilityId && b.IsActive)
                .AsQueryable();

            // Apply search
            query = query.ApplySearch(parameters.SearchTerm);

            // Apply sorting
            query = query.ApplySort(parameters.SortBy, parameters.IsDescending);

            // Return paged result
            return await query.ToPagedResultAsync(parameters);
        }

        public async Task<FacilityBlackoutDate> GetByIdAsync(Guid id)
        {
            return await _context.FacilityBlackoutDates
                .FirstOrDefaultAsync(b => b.Id == id && b.IsActive);
        }

        public async Task<IEnumerable<FacilityBlackoutDate>> GetByDateRangeAsync(Guid facilityId, DateTime startDate, DateTime endDate)
        {
            return await _context.FacilityBlackoutDates
                .Where(b => b.FacilityId == facilityId && 
                           b.IsActive &&
                           ((b.StartDate <= startDate && b.EndDate >= startDate) ||
                            (b.StartDate <= endDate && b.EndDate >= endDate) ||
                            (b.StartDate >= startDate && b.EndDate <= endDate)))
                .ToListAsync();
        }

        public async Task<FacilityBlackoutDate> AddAsync(FacilityBlackoutDate blackoutDate)
        {
            await _context.FacilityBlackoutDates.AddAsync(blackoutDate);
            await SaveChangesAsync();
            return blackoutDate;
        }

        public async Task<FacilityBlackoutDate> UpdateAsync(FacilityBlackoutDate blackoutDate)
        {
            _context.FacilityBlackoutDates.Update(blackoutDate);
            await SaveChangesAsync();
            return blackoutDate;
        }

        public async Task<bool> DeleteAsync(FacilityBlackoutDate blackoutDate)
        {
            blackoutDate.IsActive = false;
            _context.FacilityBlackoutDates.Update(blackoutDate);
            await SaveChangesAsync();
            return true;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
} 