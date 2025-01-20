# API Development Guidelines

## DTO Structure

### 1. List vs Detail DTOs
For each entity, we maintain separate DTOs for list views and detail views:

```csharp
// List DTO - Used for collection endpoints
public class EntityListDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    // Only essential properties for list view
    // Flatten nested objects into simple properties
    public string RelatedEntityName { get; set; }
    public int Count { get; set; }
}

// Detail DTO - Used for single item endpoints
public class EntityDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    // Include all necessary properties
    // Include nested objects using LookupDetailDto
    public LookupDetailDto SelectedRelatedEntity { get; set; }
}
```

### 2. Mapping Configuration
Configure AutoMapper to handle both DTOs:

```csharp
public class EntityMappingProfile : Profile
{
    public EntityMappingProfile()
    {
        // Detailed mapping for single item view
        CreateMap<Entity, EntityDto>()
            .ForMember(dest => dest.SelectedRelatedEntity, 
                      opt => opt.MapFrom(src => src.RelatedEntity));

        // Simplified mapping for list view
        CreateMap<Entity, EntityListDto>()
            .ForMember(dest => dest.RelatedEntityName, 
                      opt => opt.MapFrom(src => src.RelatedEntity.Name));
    }
}
```

## Service Layer

### 1. Interface Definition
```csharp
public interface IEntityService
{
    // List endpoint returns ListDto
    Task<PagedResult<EntityListDto>> GetEntitiesAsync(QueryParameters parameters);
    
    // Detail endpoint returns full DTO
    Task<EntityDto> GetByIdAsync(Guid id);
    
    // Other operations use full DTO
    Task<EntityDto> CreateAsync(CreateEntityDto dto);
    Task<EntityDto> UpdateAsync(Guid id, UpdateEntityDto dto);
    Task<bool> DeleteAsync(Guid id);
}
```

### 2. Service Implementation
```csharp
public class EntityService : IEntityService
{
    public async Task<PagedResult<EntityListDto>> GetEntitiesAsync(QueryParameters parameters)
    {
        var pagedEntities = await _repository.GetPagedAsync(parameters);
        
        return new PagedResult<EntityListDto>
        {
            Items = _mapper.Map<IEnumerable<EntityListDto>>(pagedEntities.Items),
            TotalItems = pagedEntities.TotalItems,
            PageNumber = pagedEntities.PageNumber,
            PageSize = pagedEntities.PageSize,
            TotalPages = pagedEntities.TotalPages,
            HasNextPage = pagedEntities.HasNextPage,
            HasPreviousPage = pagedEntities.HasPreviousPage
        };
    }

    public async Task<EntityDto> GetByIdAsync(Guid id)
    {
        var entity = await _repository.GetByIdAsync(id);
        return _mapper.Map<EntityDto>(entity);
    }
}
```

## Controller Layer

```csharp
[ApiController]
[Route("api/[controller]")]
public class EntitiesController : ControllerBase
{
    // GET api/entities - Returns list DTO
    [HttpGet]
    public async Task<ActionResult<PagedResult<EntityListDto>>> GetEntities(
        [FromQuery] QueryParameters parameters)
    {
        var result = await _service.GetEntitiesAsync(parameters);
        return Ok(result);
    }

    // GET api/entities/{id} - Returns detail DTO
    [HttpGet("{id}")]
    public async Task<ActionResult<EntityDto>> GetEntity(Guid id)
    {
        var result = await _service.GetByIdAsync(id);
        return Ok(result);
    }
}
```

## Best Practices

1. **List DTOs**
   - Keep them lightweight with only essential properties
   - Flatten nested objects into simple properties
   - Include counts and summary information
   - Used for collection endpoints and paginated results

2. **Detail DTOs**
   - Include all necessary properties
   - Use `LookupDetailDto` for related entities
   - Include nested objects and relationships
   - Used for single item operations (Get by ID, Create, Update)

3. **Mapping**
   - Configure separate mappings for List and Detail DTOs
   - Use `ForMember` to handle nested object flattening in List DTOs
   - Keep Detail DTO mappings comprehensive

4. **Repository Layer**
   - Include necessary includes/joins for both list and detail queries
   - Use filtering and pagination for list queries
   - Optimize queries based on DTO requirements

## Example Implementation

### Entity Structure
```csharp
// List DTO
public class UnitListDto
{
    public Guid Id { get; set; }
    public string UnitNumber { get; set; }
    public string Type { get; set; }
    public string Status { get; set; }
    public string FloorName { get; set; }
    public string BlockName { get; set; }
    public string CurrentOwnerName { get; set; }
}

// Detail DTO
public class UnitDto
{
    public Guid Id { get; set; }
    public string UnitNumber { get; set; }
    public string Type { get; set; }
    public string Status { get; set; }
    public LookupDetailDto SelectedFloor { get; set; }
    public LookupDetailDto SelectedBlock { get; set; }
    public LookupDetailDto SelectedSociety { get; set; }
    public LookupDetailDto SelectedCurrentOwner { get; set; }
}
```

### Mapping Configuration
```csharp
public class UnitMappingProfile : Profile
{
    public UnitMappingProfile()
    {
        // Detail mapping
        CreateMap<Unit, UnitDto>()
            .ForMember(dest => dest.SelectedFloor, opt => opt.MapFrom(src => src.Floor))
            .ForMember(dest => dest.SelectedBlock, opt => opt.MapFrom(src => src.Floor.Block))
            .ForMember(dest => dest.SelectedSociety, opt => opt.MapFrom(src => src.Floor.Block.Society))
            .ForMember(dest => dest.SelectedCurrentOwner, opt => opt.MapFrom(src => src.CurrentOwner));

        // List mapping
        CreateMap<Unit, UnitListDto>()
            .ForMember(dest => dest.FloorName, opt => opt.MapFrom(src => src.Floor.FloorName))
            .ForMember(dest => dest.BlockName, opt => opt.MapFrom(src => src.Floor.Block.Name))
            .ForMember(dest => dest.CurrentOwnerName, 
                      opt => opt.MapFrom(src => $"{src.CurrentOwner.Individual.FirstName} {src.CurrentOwner.Individual.LastName}"));
    }
}
```

## Module Structure

### Directory Layout
```
src/StarterApi.Application/Modules/{ModuleName}/
├── DTOs/
│   ├── {Entity}Dto.cs              # Detail DTO
│   ├── {Entity}ListDto.cs          # List DTO
│   ├── Create{Entity}Dto.cs        # Creation DTO
│   └── Update{Entity}Dto.cs        # Update DTO
├── Interfaces/
│   ├── I{Entity}Service.cs         # Service interface
│   └── I{Entity}Repository.cs      # Repository interface
├── Services/
│   └── {Entity}Service.cs          # Service implementation
└── Mappings/
    └── {Entity}MappingProfile.cs   # AutoMapper profile
```

### Permission Setup

1. **Add Permissions Constants**
```csharp
// src/StarterApi.Domain/Constants/Permissions.cs
public static class Permissions
{
    public static class YourModule
    {
        public const string View = "YourModule.View";
        public const string Create = "YourModule.Create";
        public const string Edit = "YourModule.Edit";
        public const string Delete = "YourModule.Delete";
    }
}
```

2. **Seed Permissions in Migration**
```csharp
// src/StarterApi.Infrastructure/Persistence/Migrations/{timestamp}_AddYourModulePermissions.cs
public partial class AddYourModulePermissions : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.InsertData(
            table: "TenantPermissions",
            columns: new[] { "Id", "Name", "SystemName", "GroupName", "Description" },
            values: new object[,]
            {
                {
                    Guid.NewGuid(),
                    "View YourModule",
                    YourModule.View,
                    "YourModule",
                    "Permission to view YourModule"
                },
                {
                    Guid.NewGuid(),
                    "Create YourModule",
                    YourModule.Create,
                    "YourModule",
                    "Permission to create YourModule"
                },
                // Add other permissions...
            });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DeleteData(
            table: "TenantPermissions",
            keyColumn: "SystemName",
            keyValues: new object[]
            {
                Permissions.YourModule.View,
                Permissions.YourModule.Create,
                // Add other permissions...
            });
    }
}
```

### Complete Module Example

1. **Entity Definition**
```csharp
// src/StarterApi.Domain/Entities/Vehicle.cs
public class Vehicle : BaseEntity
{
    public string RegistrationNumber { get; set; }
    public string Type { get; set; }
    public Guid ResidentId { get; set; }
    public virtual Resident Resident { get; set; }
}
```

2. **Repository Interface**
```csharp
// src/StarterApi.Application/Modules/Vehicles/Interfaces/IVehicleRepository.cs
public interface IVehicleRepository 
{
    Task<PagedResult<Vehicle>> GetPagedAsync(QueryParameters parameters);
    Task<bool> ExistsAsync(string registrationNumber);
    Task<IEnumerable<Vehicle>> GetByResidentIdAsync(Guid residentId);
}
```

3. **Repository Implementation**
```csharp
// src/StarterApi.Infrastructure/Persistence/Repositories/VehicleRepository.cs
public class VehicleRepository : IVehicleRepository
{
    public VehicleRepository(ITenantDbContext context) : base(context)
    {
    }

    public async Task<PagedResult<Vehicle>> GetPagedAsync(QueryParameters parameters)
    {
        var query = _context.Vehicles
            .Include(v => v.Resident)
                .ThenInclude(r => r.Individual)
            .Where(v => v.IsActive);

        // Apply search
        if (!string.IsNullOrEmpty(parameters.SearchTerm))
        {
            query = query.Where(v => 
                v.RegistrationNumber.Contains(parameters.SearchTerm) ||
                v.Type.Contains(parameters.SearchTerm));
        }

        return await query.ToPagedResultAsync(parameters);
    }

    public async Task<bool> ExistsAsync(string registrationNumber)
    {
        return await _context.Vehicles
            .AnyAsync(v => v.RegistrationNumber == registrationNumber && v.IsActive);
    }

    public async Task<IEnumerable<Vehicle>> GetByResidentIdAsync(Guid residentId)
    {
        return await _context.Vehicles
            .Where(v => v.ResidentId == residentId && v.IsActive)
            .ToListAsync();
    }
}
```

4. **Service Interface**
```csharp
// src/StarterApi.Application/Modules/Vehicles/Interfaces/IVehicleService.cs
public interface IVehicleService
{
    Task<VehicleDto> GetByIdAsync(Guid id);
    Task<PagedResult<VehicleListDto>> GetVehiclesAsync(QueryParameters parameters);
    Task<VehicleDto> CreateVehicleAsync(CreateVehicleDto dto);
    Task<VehicleDto> UpdateVehicleAsync(Guid id, UpdateVehicleDto dto);
    Task<bool> DeleteVehicleAsync(Guid id);
    Task<IEnumerable<VehicleDto>> GetByResidentIdAsync(Guid residentId);
}
```

5. **Controller with RBAC**
```csharp
// src/StarterApi.Api/Controllers/VehiclesController.cs
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class VehiclesController : ControllerBase
{
    private readonly IVehicleService _vehicleService;
    private readonly ILogger<VehiclesController> _logger;

    public VehiclesController(
        IVehicleService vehicleService,
        ILogger<VehiclesController> logger)
    {
        _vehicleService = vehicleService;
        _logger = logger;
    }

    [HttpGet]
    [RequirePermission(Permissions.Vehicles.View)]
    public async Task<ActionResult<PagedResult<VehicleListDto>>> GetVehicles(
        [FromQuery] QueryParameters parameters)
    {
        try
        {
            var vehicles = await _vehicleService.GetVehiclesAsync(parameters);
            return Ok(vehicles);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving vehicles");
            return StatusCode(500, "An error occurred while retrieving vehicles");
        }
    }

    // Other endpoints...
}
```

6. **Register Dependencies**
```csharp
// src/StarterApi.Api/DependencyInjection.cs
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Register AutoMapper
        services.AddAutoMapper(typeof(VehicleMappingProfile).Assembly);

        // Register Services
        services.AddScoped<IVehicleService, VehicleService>();

        // Register Repositories
        services.AddScoped<IVehicleRepository, VehicleRepository>();

        return services;
    }
}
```

## Database Context

1. **Add DbSet to ITenantDbContext**
```csharp
// src/StarterApi.Application/Common/Interfaces/ITenantDbContext.cs
public interface ITenantDbContext
{
    DbSet<Vehicle> Vehicles { get; set; }
    // Other DbSets...
}
```

2. **Add DbSet to TenantDbContext**
```csharp
// src/StarterApi.Infrastructure/Persistence/TenantDbContext.cs
public class TenantDbContext : DbContext, ITenantDbContext
{
    public DbSet<Vehicle> Vehicles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Apply configurations
        modelBuilder.ApplyConfiguration(new VehicleConfiguration());
    }
}
```

3. **Entity Configuration**
```csharp
// src/StarterApi.Infrastructure/Persistence/Configurations/VehicleConfiguration.cs
public class VehicleConfiguration : BaseConfiguration<Vehicle>
{
    public override void Configure(EntityTypeBuilder<Vehicle> builder)
    {
        base.Configure(builder);

        builder.Property(e => e.RegistrationNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(e => e.Type)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasOne(e => e.Resident)
            .WithMany(r => r.Vehicles)
            .HasForeignKey(e => e.ResidentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(e => e.RegistrationNumber)
            .IsUnique();
    }
}
```

This structure ensures:
1. Clear separation of concerns
2. Consistent module organization
3. Proper dependency injection
4. Role-based access control (RBAC)
5. Clean database context integration
6. Type-safe permission management

When creating a new module:
1. Create the directory structure
2. Define the entity and DTOs
3. Create repository and service interfaces
4. Implement repository and service
5. Add permissions to constants
6. Create migration for permissions
7. Add controller with RBAC
8. Register dependencies
9. Update database context

### DTOs Structure

```csharp
// List DTO - Used for collection endpoints
public class VehicleListDto
{
    public Guid Id { get; set; }
    public string RegistrationNumber { get; set; }
    public string Type { get; set; }
    public string ResidentName { get; set; }  // Flattened for list view
}

// Detail DTO - Used for single item endpoints
public class VehicleDto
{
    public Guid Id { get; set; }
    public string RegistrationNumber { get; set; }
    public string Type { get; set; }
    public LookupDetailDto SelectedResident { get; set; }  // Nested object for details
    // Additional properties for detailed view
}
```

### Mapping Profile Example

```csharp
public class VehicleMappingProfile : Profile
{
    public VehicleMappingProfile()
    {
        // List mapping - simplified for collections
        CreateMap<Vehicle, VehicleListDto>()
            .ForMember(dest => dest.ResidentName, 
                opt => opt.MapFrom(src => $"{src.Resident.Individual.FirstName} {src.Resident.Individual.LastName}"));

        // Detail mapping - complete object with nested DTOs
        CreateMap<Vehicle, VehicleDto>()
            .ForMember(dest => dest.SelectedResident, opt => opt.MapFrom(src => src.Resident));
    }
}
```

### Service Implementation Example

```csharp
public class VehicleService : IVehicleService
{
    // Get list with simplified DTO
    public async Task<PagedResult<VehicleListDto>> GetVehiclesAsync(QueryParameters parameters)
    {
        var vehicles = await _repository.GetPagedAsync(parameters);
        return vehicles.MapTo<VehicleListDto>(_mapper);
    }

    // Get single item with detailed DTO
    public async Task<VehicleDto> GetByIdAsync(Guid id)
    {
        var vehicle = await _repository.GetByIdAsync(id);
        return _mapper.Map<VehicleDto>(vehicle);
    }
}
```

### Controller Example

```csharp
[ApiController]
[Route("api/[controller]")]
public class VehiclesController : ControllerBase
{
    [HttpGet]
    [RequirePermission(Permissions.Vehicles.View)]
    public async Task<ActionResult<PagedResult<VehicleListDto>>> GetVehicles(
        [FromQuery] QueryParameters parameters)
    {
        return Ok(await _service.GetVehiclesAsync(parameters));
    }

    [HttpGet("{id}")]
    [RequirePermission(Permissions.Vehicles.View)]
    public async Task<ActionResult<VehicleDto>> GetVehicle(Guid id)
    {
        return Ok(await _service.GetByIdAsync(id));
    }
}
```
