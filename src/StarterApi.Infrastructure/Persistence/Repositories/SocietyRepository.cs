using Microsoft.EntityFrameworkCore;
using StarterApi.Application.Modules.Societies.Interfaces;
using StarterApi.Domain.Entities;
using StarterApi.Infrastructure.Persistence.Contexts;

namespace StarterApi.Infrastructure.Persistence.Repositories
{
    public class SocietyRepository : ISocietyRepository
    {
        private readonly TenantDbContext _context;

        public SocietyRepository(TenantDbContext context)
        {
            _context = context;
        }

        public async Task<Society> GetByIdAsync(Guid id)
        {
            return await _context.Societies.FindAsync(id);
        }

        public async Task<IEnumerable<Society>> GetAllAsync()
        {
            return await _context.Societies.ToListAsync();
        }

        public async Task<Society> AddAsync(Society society)
        {
            await _context.Societies.AddAsync(society);
            return society;
        }

        public async Task<Society> UpdateAsync(Society society)
        {
            _context.Entry(society).State = EntityState.Modified;
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