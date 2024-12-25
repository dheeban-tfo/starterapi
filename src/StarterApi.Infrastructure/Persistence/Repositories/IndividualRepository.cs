using Microsoft.EntityFrameworkCore;
using StarterApi.Application.Common.Extensions;
using StarterApi.Application.Common.Models;
using StarterApi.Application.Modules.Individuals.Interfaces;
using StarterApi.Domain.Entities;
using StarterApi.Infrastructure.Persistence.Contexts;
using StarterApi.Application.Common.Interfaces;

namespace StarterApi.Infrastructure.Persistence.Repositories
{
    public class IndividualRepository : IIndividualRepository
    {
        private readonly ITenantDbContext _context;

        public IndividualRepository(ITenantDbContext context)
        {
            _context = context;
        }

        public async Task<Individual> GetByIdAsync(Guid id)
        {
            return await _context.Individuals
                .FirstOrDefaultAsync(i => i.Id == id && i.IsActive);
        }

        public async Task<PagedResult<Individual>> GetPagedAsync(QueryParameters parameters)
        {
            var query = _context.Individuals
                .Where(i => i.IsActive)
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

        public async Task<Individual> AddAsync(Individual individual)
        {
            await _context.Individuals.AddAsync(individual);
            return individual;
        }

        public async Task<Individual> UpdateAsync(Individual individual)
        {
            ((DbContext)_context).Entry(individual).State = EntityState.Modified;
            return individual;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await _context.Individuals
                .AnyAsync(i => i.Email == email && i.IsActive);
        }

        public async Task<bool> ExistsByPhoneNumberAsync(string phoneNumber)
        {
            return await _context.Individuals
                .AnyAsync(i => i.PhoneNumber == phoneNumber && i.IsActive);
        }

        public async Task<bool> ExistsByIdProofAsync(string idProofType, string idProofNumber)
        {
            return await _context.Individuals
                .AnyAsync(i => i.IdProofType == idProofType && i.IdProofNumber == idProofNumber && i.IsActive);
        }

        public async Task<Individual> GetByEmailAsync(string email)
        {
            return await _context.Individuals
                .FirstOrDefaultAsync(i => i.Email == email && i.IsActive);
        }

        public async Task<Individual> GetByPhoneNumberAsync(string phoneNumber)
        {
            return await _context.Individuals
                .FirstOrDefaultAsync(i => i.PhoneNumber == phoneNumber && i.IsActive);
        }

        public async Task<Individual> GetByIdProofAsync(string idProofType, string idProofNumber)
        {
            return await _context.Individuals
                .FirstOrDefaultAsync(i => i.IdProofType == idProofType && i.IdProofNumber == idProofNumber && i.IsActive);
        }
    }
}
