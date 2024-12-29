using Microsoft.EntityFrameworkCore;
using StarterApi.Application.Common.Extensions;
using StarterApi.Application.Common.Interfaces;
using StarterApi.Application.Common.Models;
using StarterApi.Application.Modules.Facilities.Interfaces;
using StarterApi.Domain.Entities;

namespace StarterApi.Infrastructure.Persistence.Repositories
{
    public class FacilityRepository : IFacilityRepository
    {
        private readonly ITenantDbContext _context;

        public FacilityRepository(ITenantDbContext context)
        {
            _context = context;
        }

        public async Task<Facility> GetByIdAsync(Guid id)
        {
            return await _context.Facilities
                .FirstOrDefaultAsync(f => f.Id == id && f.IsActive);
        }

        public async Task<Facility> GetByIdWithDetailsAsync(Guid id)
        {
            return await _context.Facilities
                .Include(f => f.Society)
                .Include(f => f.Bookings.Where(b => b.IsActive))
                .FirstOrDefaultAsync(f => f.Id == id && f.IsActive);
        }

        public async Task<PagedResult<Facility>> GetPagedAsync(QueryParameters parameters)
        {
            var query = _context.Facilities
                .Include(f => f.Society)
                .Where(f => f.IsActive)
                .AsQueryable();

            // Apply search
            query = query.ApplySearch(parameters.SearchTerm);

            // Apply sorting
            query = query.ApplySort(parameters.SortBy, parameters.IsDescending);

            // Return paged result
            return await query.ToPagedResultAsync(parameters);
        }

        public async Task<IEnumerable<Facility>> GetAllAsync()
        {
            return await _context.Facilities
                .Where(f => f.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<Facility>> GetBySocietyIdAsync(Guid societyId)
        {
            return await _context.Facilities
                .Where(f => f.SocietyId == societyId && f.IsActive)
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(string name, Guid? excludeId = null)
        {
            var query = _context.Facilities.Where(f => f.Name == name && f.IsActive);
            
            if (excludeId.HasValue)
            {
                query = query.Where(f => f.Id != excludeId.Value);
            }

            return await query.AnyAsync();
        }

        public async Task<int> GetActiveBookingsCountAsync(Guid facilityId)
        {
            return await _context.FacilityBookings
                .CountAsync(b => b.FacilityId == facilityId && 
                                b.IsActive && 
                                b.StartTime > DateTime.UtcNow.TimeOfDay);
        }

        public async Task<Dictionary<Guid, int>> GetActiveBookingsCountForFacilitiesAsync(List<Guid> facilityIds)
        {
            return await _context.FacilityBookings
                .Where(b => facilityIds.Contains(b.FacilityId) && 
                            b.IsActive && 
                            b.StartTime > DateTime.UtcNow.TimeOfDay)
                .GroupBy(b => b.FacilityId)
                .ToDictionaryAsync(
                    g => g.Key,
                    g => g.Count());
        }

        public async Task<Facility> AddAsync(Facility entity)
        {
            await _context.Facilities.AddAsync(entity);
            await SaveChangesAsync();
            return entity;
        }

        public async Task<Facility> UpdateAsync(Facility entity)
        {
            _context.Facilities.Update(entity);
            await SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(Facility entity)
        {
            entity.IsActive = false;
            _context.Facilities.Update(entity);
            await SaveChangesAsync();
            return true;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
} 