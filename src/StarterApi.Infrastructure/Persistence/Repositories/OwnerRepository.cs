using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StarterApi.Application.Common.Interfaces;
using StarterApi.Application.Common.Models;
using StarterApi.Application.Modules.Owners.Interfaces;
using StarterApi.Domain.Entities;

namespace StarterApi.Infrastructure.Persistence.Repositories
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly ITenantDbContext _context;

        public OwnerRepository(ITenantDbContext context)
        {
            _context = context;
        }

        public async Task<Owner> GetByIdAsync(Guid id)
        {
            return await _context.Owners
                .Include(o => o.Individual)
                .Include(o => o.Units)
                .FirstOrDefaultAsync(o => o.Id == id && o.IsActive);
        }

        public async Task<IEnumerable<Owner>> GetAllAsync()
        {
            return await _context.Owners
                .Include(o => o.Individual)
                .Include(o => o.Units)
                .Where(o => o.IsActive)
                .ToListAsync();
        }

        public async Task<Owner> GetByIdWithDetailsAsync(Guid id)
        {
            return await _context.Owners
                .Include(o => o.Individual)
                .Include(o => o.Units)
                .Include(o => o.OwnershipHistory)
                .FirstOrDefaultAsync(o => o.Id == id && o.IsActive);
        }

        public async Task<List<Owner>> GetByUnitIdAsync(Guid unitId)
        {
            return await _context.Owners
                .Include(o => o.Individual)
                .Include(o => o.Units)
                .Include(o => o.OwnershipHistory)
                .Where(o => o.Units.Any(u => u.Id == unitId) && o.IsActive)
                .ToListAsync();
        }

        public async Task<bool> HasActiveOwnershipAsync(Guid unitId, Guid? excludeOwnerId = null)
        {
            var query = _context.Owners
                .Where(o => o.Units.Any(u => u.Id == unitId) && o.IsActive);

            if (excludeOwnerId.HasValue)
            {
                query = query.Where(o => o.Id != excludeOwnerId.Value);
            }

            return await query.AnyAsync();
        }

        public async Task<PagedResult<Owner>> GetPagedAsync(QueryParameters parameters)
        {
            var query = _context.Owners
                .Include(o => o.Individual)
                .Include(o => o.Units)
                .Where(o => o.IsActive)
                .AsQueryable();

            // Apply search
            if (!string.IsNullOrWhiteSpace(parameters.SearchTerm))
            {
                query = query.Where(o =>
                    o.Individual.FirstName.Contains(parameters.SearchTerm) ||
                    o.Individual.LastName.Contains(parameters.SearchTerm) ||
                    o.Individual.Email.Contains(parameters.SearchTerm) ||
                    o.Individual.PhoneNumber.Contains(parameters.SearchTerm));
            }

            // Apply sorting
            if (!string.IsNullOrWhiteSpace(parameters.SortBy))
            {
                switch (parameters.SortBy.ToLower())
                {
                    case "name":
                        query = parameters.IsDescending
                            ? query.OrderByDescending(o => o.Individual.FirstName)
                            : query.OrderBy(o => o.Individual.FirstName);
                        break;
                    case "email":
                        query = parameters.IsDescending
                            ? query.OrderByDescending(o => o.Individual.Email)
                            : query.OrderBy(o => o.Individual.Email);
                        break;
                    default:
                        query = query.OrderBy(o => o.Individual.FirstName);
                        break;
                }
            }
            else
            {
                query = query.OrderBy(o => o.Individual.FirstName);
            }

            var totalItems = await query.CountAsync();
            var items = await query
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();

            return new PagedResult<Owner>
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

        public async Task<Owner> AddAsync(Owner owner)
        {
            await _context.Owners.AddAsync(owner);
            return owner;
        }

        public async Task<Owner> UpdateAsync(Owner owner)
        {
            _context.Owners.Update(owner);
            return owner;
        }

        public async Task DeleteAsync(Guid id)
        {
            var owner = await _context.Owners.FindAsync(id);
            if (owner != null)
            {
                owner.IsActive = false;
            }
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
} 