using Microsoft.EntityFrameworkCore;
using StarterApi.Application.Common.Extensions;
using StarterApi.Application.Common.Models;
using StarterApi.Application.Modules.Societies.Interfaces;
using StarterApi.Domain.Entities;
using StarterApi.Infrastructure.Persistence.Contexts;
using StarterApi.Application.Common.Interfaces;

namespace StarterApi.Infrastructure.Persistence.Repositories
{
    public class SocietyRepository : ISocietyRepository
    {
        private readonly ITenantDbContext _context;

        public SocietyRepository(ITenantDbContext context)
        {
            _context = context;
        }

        public async Task<Society> GetByIdAsync(Guid id)
        {
            return await _context.Societies
                .Include(s => s.Blocks.Where(b => b.IsActive))
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<PagedResult<Society>> GetPagedAsync(QueryParameters parameters)
        {
            var query = _context.Societies
                .Include(s => s.Blocks.Where(b => b.IsActive))
                .Where(s => s.IsActive)
                .AsQueryable();

            // Apply Search
            query = query.ApplySearch(parameters.SearchTerm);

            // Apply Filters
            query = query.ApplyFiltering(parameters.Filters);

            // Apply Sorting
            query = query.ApplySort(parameters.SortBy, parameters.IsDescending);

            // Return Paged Result
            return await query.ToPagedResultAsync(parameters);
        }

        public async Task<Society> AddAsync(Society society)
        {
            await _context.Societies.AddAsync(society);
            return society;
        }

        public async Task<Society> UpdateAsync(Society society)
        {
            ((DbContext)_context).Entry(society).State = EntityState.Modified;
            return society;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<Society> GetByRegistrationNumberAsync(string registrationNumber)
        {
            return await _context.Societies
                .FirstOrDefaultAsync(s => s.RegistrationNumber == registrationNumber);
        }

        public async Task<bool> ExistsAsync(string registrationNumber)
        {
            return await _context.Societies
                .AnyAsync(s => s.RegistrationNumber == registrationNumber);
        }
    }
} 