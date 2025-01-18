using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using StarterApi.Domain.Entities;
using Microsoft.Extensions.Logging;
using StarterApi.Application.Common.Exceptions;



namespace StarterApi.Application.Modules.Tenants.Services
{
    public class TenantService : ITenantService
    {
        private readonly ITenantRepository _tenantRepository;
        private readonly ITenantDbMigrationService _tenantDbMigrationService;
        private readonly IMapper _mapper;
        private readonly ILogger<TenantService> _logger;

        public TenantService(
            ITenantRepository tenantRepository,
            ITenantDbMigrationService tenantDbMigrationService,
            IMapper mapper,
            ILogger<TenantService> logger)
        {
            _tenantRepository = tenantRepository;
            _tenantDbMigrationService = tenantDbMigrationService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<TenantDto> CreateTenantAsync(CreateTenantDto dto)
        {
            var tenant = new Tenant
            {
                Name = dto.Name,
                DatabaseName = dto.DatabaseName,
                Status = TenantStatus.Active,
                ConnectionString = $"Server=localhost;Database={dto.DatabaseName};User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=True;MultipleActiveResultSets=true;"
            };
            
            try
            {
                // Save tenant to root database
                await _tenantRepository.AddAsync(tenant);
                await _tenantRepository.SaveChangesAsync();

                // Create tenant database
                await _tenantDbMigrationService.CreateTenantDatabaseAsync(tenant);

                return _mapper.Map<TenantDto>(tenant);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create tenant {TenantName}", dto.Name);
                throw;
            }
        }

        public async Task<TenantInternalDto> GetTenantByIdAsync(Guid id)
        {
            var tenant = await _tenantRepository.GetByIdAsync(id);
            if (tenant == null)
                throw new NotFoundException($"Tenant with ID {id} not found");

            return new TenantInternalDto
            {
                Id = tenant.Id,
                Name = tenant.Name,
                DatabaseName = tenant.DatabaseName,
                Status = tenant.Status.ToString(),
                ConnectionString = tenant.ConnectionString,
                CreatedAt = tenant.CreatedAt,
                UpdatedAt = tenant.UpdatedAt
            };
        }

        public async Task<IEnumerable<TenantDto>> GetAllTenantsAsync()
        {
            var tenants = await _tenantRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<TenantDto>>(tenants);
        }

        public async Task DeactivateTenantAsync(Guid id)
        {
            var tenant = await _tenantRepository.GetByIdAsync(id);
            if (tenant == null)
                throw new NotFoundException($"Tenant with ID {id} not found");

            tenant.Status = TenantStatus.Inactive;
            await _tenantRepository.SaveChangesAsync();
        }

        public Task<bool> DeleteTenantAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
} 