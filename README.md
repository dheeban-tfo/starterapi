# StarterApi

A robust .NET Core API template with multi-tenancy support, role-based access control, and modular architecture.

## Implementing New Modules

This guide outlines the step-by-step process for implementing new modules in the project.

### 1. Domain Layer

1. Create the Entity class in `src/StarterApi.Domain/Entities/`
   - Inherit from `BaseEntity`
   - Define all properties
   - Add navigation properties if needed
   - Add any required collections

2. Add permissions in `src/StarterApi.Domain/Constants/Permissions.cs`
   - Add a new permission group for the module
   - Define CRUD permissions (View, Create, Edit, Delete)
   - Add any additional specific permissions

### 2. Infrastructure Layer

1. Create Entity Configuration in `src/StarterApi.Infrastructure/Persistence/Configurations/`
   - Inherit from `BaseConfiguration<TEntity>`
   - Configure table name, properties, and constraints
   - Set up relationships and foreign keys
   - Add any required indexes

2. Add DbSet to `ITenantDbContext` and `TenantDbContext`
   - Add the DbSet property in both interface and implementation
   - Place it in the appropriate section with comments

3. Create Repository in `src/StarterApi.Infrastructure/Persistence/Repositories/`
   - Implement the repository interface
   - Include standard CRUD operations
   - Add any custom query methods
   - Ensure proper filtering and includes

### 3. Application Layer

1. Create DTOs in `src/StarterApi.Application/Modules/{ModuleName}/DTOs/`
   - Create separate DTOs for different operations (Create, Update, Response)
   - Add data annotations for validation
   - Keep DTOs focused on their specific use cases

2. Create Repository Interface in `src/StarterApi.Application/Modules/{ModuleName}/Interfaces/`
   - Define CRUD operations
   - Add any custom query methods
   - Include documentation comments

3. Create Service Interface in `src/StarterApi.Application/Modules/{ModuleName}/Interfaces/`
   - Define business operations
   - Include proper documentation
   - Add validation rules

4. Create Service Implementation in `src/StarterApi.Application/Modules/{ModuleName}/Services/`
   - Implement the service interface
   - Add business logic and validation
   - Use AutoMapper for object mapping
   - Handle exceptions appropriately

5. Add AutoMapper Mappings in `src/StarterApi.Application/Common/Mappings/MappingProfile.cs`
   - Add mappings between entity and DTOs
   - Configure any custom mapping logic
   - Handle nested object mappings

### 4. API Layer

1. Create Controller in `src/StarterApi.Api/Controllers/`
   - Inherit from `BaseController`
   - Add proper route and API version attributes
   - Implement CRUD endpoints
   - Add proper authorization attributes
   - Include XML documentation

2. Register Services in `Program.cs`
   - Register repository
   - Register service
   - Add any required configurations

### 5. Database Migration

1. Create Migration
   ```bash
   dotnet ef migrations add Add{ModuleName}Table
   ```

2. Update Database
   ```bash
   dotnet ef database update
   ```

### 6. Seeding (If Required)

1. Update Permission Seeders
   - Add new permissions to `PermissionSeeder`
   - Add new permissions to `TenantPermissionSeeder`
   - Add role-permission mappings

2. Create Module-Specific Seeder (if needed)
   - Implement seeding logic
   - Register seeder in `RootDataSeeder` or `TenantDataSeeder`

### 7. Testing

1. Test CRUD Operations
   - Verify all endpoints work as expected
   - Test validation rules
   - Check authorization rules
   - Verify proper error handling

2. Test Business Logic
   - Verify service layer logic
   - Test edge cases
   - Check integration with other modules

### Best Practices

1. **Naming Conventions**
   - Use consistent naming across all layers
   - Follow C# naming conventions
   - Use meaningful and descriptive names

2. **Authorization**
   - Always implement proper authorization
   - Use permission-based access control
   - Test with different user roles

3. **Validation**
   - Implement proper validation at DTO level
   - Add business rule validation in service layer
   - Handle validation errors consistently

4. **Error Handling**
   - Use appropriate exception types
   - Return proper HTTP status codes
   - Provide meaningful error messages

5. **Documentation**
   - Add XML comments to public APIs
   - Document complex business logic
   - Keep README updated

### Common Gotchas

1. Don't forget to:
   - Register services in DI container
   - Add DbSet to both context interface and implementation
   - Configure entity relationships
   - Add proper indexes for performance
   - Implement proper validation
   - Handle null cases
   - Add proper authorization

2. Always check:
   - Foreign key constraints
   - Unique indexes
   - Proper error handling
   - Proper object mapping
   - Performance implications of queries

### Example Module Structure

```
src/
├── StarterApi.Domain/
│   ├── Entities/
│   │   └── NewEntity.cs
│   └── Constants/
│       └── Permissions.cs
│
├── StarterApi.Infrastructure/
│   ├── Persistence/
│   │   ├── Configurations/
│   │   │   └── NewEntityConfiguration.cs
│   │   └── Repositories/
│   │       └── NewEntityRepository.cs
│
└── StarterApi.Application/
    └── Modules/
        └── NewModule/
            ├── DTOs/
            │   ├── CreateNewEntityDto.cs
            │   ├── UpdateNewEntityDto.cs
            │   └── NewEntityDto.cs
            ├── Interfaces/
            │   ├── INewEntityRepository.cs
            │   └── INewEntityService.cs
            └── Services/
                └── NewEntityService.cs
```

### Modules and Permissions

The system includes the following modules with their respective permissions:

1. **Users**
   - View, Create, Edit, Delete
   - ManageRoles, InviteUser

2. **Roles**
   - View, Create, Edit, Delete
   - ManagePermissions

3. **Tenants**
   - View, Create, Edit, Delete
   - ManageUsers

4. **Societies**
   - View, Create, Edit, Delete
   - ManageBlocks

5. **Blocks**
   - View, Create, Edit, Delete
   - ManageFloors

6. **Floors**
   - View, Create, Edit, Delete
   - ManageUnits

7. **Units**
   - View, Create, Edit, Delete

8. **Individuals**
   - View, Create, Edit, Delete
   - Verify

Each module's permissions are automatically assigned to the Root Admin role during seeding.
