using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using StarterApi.Domain.Entities;
using Microsoft.Extensions.Logging;



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
            var tenant = new Tenant(dto.Name, dto.DatabaseName);
            
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

        public async Task<TenantDto> GetTenantByIdAsync(Guid id)
        {
            var tenant = await _tenantRepository.GetByIdAsync(id);
            if (tenant == null)
                throw new NotFoundException($"Tenant with ID {id} not found");

            return _mapper.Map<TenantDto>(tenant);
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

            tenant.Deactivate();
            await _tenantRepository.SaveChangesAsync();
        }
    }
} 