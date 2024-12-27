using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StarterApi.Application.Common.Interfaces;
using StarterApi.Application.Common.Models;
using StarterApi.Application.Modules.Residents.Interfaces;
using StarterApi.Domain.Entities;


namespace StarterApi.Infrastructure.Persistence.Repositories
{
    public class ResidentRepository : IResidentRepository
    {
        private readonly ITenantDbContext _context;

        public ResidentRepository(ITenantDbContext context)
        {
            _context = context;
        }

        public async Task<Resident> GetByIdAsync(Guid id)
        {
            return await _context.Residents
                .Include(r => r.Individual)
                .Include(r => r.Unit)
                .Include(r => r.Documents)
                .FirstOrDefaultAsync(r => r.Id == id && r.IsActive);
        }

        public async Task<IEnumerable<Resident>> GetAllAsync()
        {
            return await _context.Residents
                .Include(r => r.Individual)
                .Include(r => r.Unit)
                .Include(r => r.Documents)
                .Where(r => r.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<Resident>> GetByUnitIdAsync(Guid unitId)
        {
            return await _context.Residents
                .Include(r => r.Individual)
                .Include(r => r.Unit)
                .Where(r => r.UnitId == unitId && r.IsActive)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Resident>> GetByIndividualIdAsync(Guid individualId)
        {
            return await _context.Residents
                .Include(r => r.Unit)
                .Include(r => r.Documents)
                .Where(r => r.IndividualId == individualId && r.IsActive)
                .ToListAsync();
        }

        public async Task<Resident> GetByUserIdAsync(Guid userId)
        {
            return await _context.Residents
                .Include(r => r.Individual)
                .Include(r => r.Unit)
                .Include(r => r.Documents)
                .FirstOrDefaultAsync(r => r.UserId == userId && r.IsActive);
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Residents
                .AnyAsync(r => r.Id == id && r.IsActive);
        }

        public async Task<bool> IsPrimaryResidentExistsForUnitAsync(Guid unitId, Guid? excludeResidentId = null)
        {
            var query = _context.Residents
                .Where(r => r.UnitId == unitId && r.IsActive && r.PrimaryResident);

            if (excludeResidentId.HasValue)
            {
                query = query.Where(r => r.Id != excludeResidentId.Value);
            }

            return await query.AnyAsync();
        }

        public async Task<Resident> CreateAsync(Resident resident)
        {
            // Verify user exists before creating resident
            var user = await _context.Users.FindAsync(resident.UserId);
            if (user == null)
            {
                throw new InvalidOperationException($"User {resident.UserId} not found in tenant database. Cannot create resident.");
            }

            _context.Residents.Add(resident);
            await _context.SaveChangesAsync();
            return resident;
        }

        public async Task<Resident> UpdateAsync(Resident resident)
        {
            _context.Residents.Update(resident);
            await _context.SaveChangesAsync();
            return resident;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var resident = await _context.Residents.FindAsync(id);
            if (resident == null) return false;

            resident.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Resident>> GetPendingVerificationsAsync()
        {
            return await _context.Residents
                .Include(r => r.Individual)
                .Include(r => r.Unit)
                .Include(r => r.Documents)
                .Where(r => r.IsActive && !r.IsVerified)
                .ToListAsync();
        }

        public async Task<bool> HasActiveResidencyAsync(Guid individualId)
        {
            return await _context.Residents
                .AnyAsync(r => r.IndividualId == individualId && 
                              r.IsActive && 
                              (r.CheckOutDate == null || r.CheckOutDate > DateTime.UtcNow));
        }

        public async Task<bool> IsUnitAvailableForResidentAsync(Guid unitId)
        {
            var unit = await _context.Units
                .Include(u => u.Residents.Where(r => r.IsActive))
                .FirstOrDefaultAsync(u => u.Id == unitId);

            return unit != null && unit.IsActive;
        }

        public async Task<PagedResult<Resident>> GetResidentsAsync(QueryParameters parameters)
        {
            var query = _context.Residents
                .Include(r => r.Individual)
                .Include(r => r.Unit)
                .Include(r => r.Documents)
                .Where(r => r.IsActive);

            // Apply search
            if (!string.IsNullOrWhiteSpace(parameters.SearchTerm))
            {
                var searchTerm = parameters.SearchTerm.ToLower();
                query = query.Where(r =>
                    r.Individual.FirstName.ToLower().Contains(searchTerm) ||
                    r.Individual.LastName.ToLower().Contains(searchTerm) ||
                    r.Individual.Email.ToLower().Contains(searchTerm) ||
                    r.Unit.UnitNumber.ToLower().Contains(searchTerm) ||
                    r.ResidentType.ToLower().Contains(searchTerm) ||
                    r.Status.ToLower().Contains(searchTerm));
            }

            // Apply filters
            foreach (var filter in parameters.Filters)
            {
                switch (filter.PropertyName.ToLower())
                {
                    case "residenttype":
                        query = query.Where(r => r.ResidentType == filter.Value);
                        break;
                    case "status":
                        query = query.Where(r => r.Status == filter.Value);
                        break;
                    case "unitid":
                        if (Guid.TryParse(filter.Value, out Guid unitId))
                        {
                            query = query.Where(r => r.UnitId == unitId);
                        }
                        break;
                    case "isverified":
                        if (bool.TryParse(filter.Value, out bool isVerified))
                        {
                            query = query.Where(r => r.IsVerified == isVerified);
                        }
                        break;
                }
            }

            // Apply sorting
            if (!string.IsNullOrWhiteSpace(parameters.SortBy))
            {
                switch (parameters.SortBy.ToLower())
                {
                    case "name":
                        query = parameters.IsDescending
                            ? query.OrderByDescending(r => r.Individual.FirstName)
                            : query.OrderBy(r => r.Individual.FirstName);
                        break;
                    case "email":
                        query = parameters.IsDescending
                            ? query.OrderByDescending(r => r.Individual.Email)
                            : query.OrderBy(r => r.Individual.Email);
                        break;
                    case "residenttype":
                        query = parameters.IsDescending
                            ? query.OrderByDescending(r => r.ResidentType)
                            : query.OrderBy(r => r.ResidentType);
                        break;
                    case "status":
                        query = parameters.IsDescending
                            ? query.OrderByDescending(r => r.Status)
                            : query.OrderBy(r => r.Status);
                        break;
                    case "checkindate":
                        query = parameters.IsDescending
                            ? query.OrderByDescending(r => r.CheckInDate)
                            : query.OrderBy(r => r.CheckInDate);
                        break;
                    default:
                        query = query.OrderBy(r => r.Individual.FirstName);
                        break;
                }
            }
            else
            {
                query = query.OrderBy(r => r.Individual.FirstName);
            }

            var totalItems = await query.CountAsync();
            var items = await query
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();

            return new PagedResult<Resident>
            {
                Items = items,
                TotalItems = totalItems,
                PageNumber = parameters.PageNumber,
                PageSize = parameters.PageSize,
                TotalPages = (int)Math.Ceiling(totalItems / (double)parameters.PageSize),
                HasNextPage = parameters.PageNumber < (int)Math.Ceiling(totalItems / (double)parameters.PageSize),
                HasPreviousPage = parameters.PageNumber > 1
            };
        }
    }
} 