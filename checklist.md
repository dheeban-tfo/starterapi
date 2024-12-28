# Implementation Checklist

## 0. Module Structure
- [ ] Verify Module Directory Structure
  - [ ] Create module directory under correct layer
    - [ ] Domain: `src/StarterApi.Domain/Entities`
    - [ ] Application: `src/StarterApi.Application/Modules/{ModuleName}`
    - [ ] Infrastructure: `src/StarterApi.Infrastructure/Persistence`
    - [ ] API: `src/StarterApi.Api/Controllers`

  - [ ] Create proper subdirectories in Application layer
    - [ ] DTOs: `src/StarterApi.Application/Modules/{ModuleName}/DTOs`
    - [ ] Interfaces: `src/StarterApi.Application/Modules/{ModuleName}/Interfaces`
    - [ ] Services: `src/StarterApi.Application/Modules/{ModuleName}/Services`
    - [ ] Validators: `src/StarterApi.Application/Modules/{ModuleName}/Validators` (if needed)
    - [ ] Mappings: `src/StarterApi.Application/Modules/{ModuleName}/Mappings` (if needed)

  - [ ] Follow File Naming Conventions
    - [ ] Entity: `{Entity}.cs`
    - [ ] DTOs: `{Entity}Dto.cs` (all DTOs in one file)
    - [ ] Repository Interface: `I{Entity}Repository.cs`
    - [ ] Repository Implementation: `{Entity}Repository.cs`
    - [ ] Service Interface: `I{Entity}Service.cs`
    - [ ] Service Implementation: `{Entity}Service.cs`
    - [ ] Controller: `{Entity}Controller.cs`
    - [ ] Configuration: `{Entity}Configuration.cs`

  - [ ] Verify Namespace Conventions
    - [ ] Domain: `StarterApi.Domain.Entities`
    - [ ] Application DTOs: `StarterApi.Application.Modules.{ModuleName}.DTOs`
    - [ ] Application Interfaces: `StarterApi.Application.Modules.{ModuleName}.Interfaces`
    - [ ] Application Services: `StarterApi.Application.Modules.{ModuleName}.Services`
    - [ ] Infrastructure: `StarterApi.Infrastructure.Persistence.Repositories`
    - [ ] API: `StarterApi.Api.Controllers`

## 1. Domain Layer
- [ ] Create/Update Entity classes
  - [ ] Add all required properties
  - [ ] Add navigation properties
  - [ ] Inherit from base classes if needed (e.g., BaseEntity)
  - [ ] Add any required enums
  - [ ] Initialize collections in constructor
  - [ ] Add required validation attributes

## 2. Infrastructure Layer
- [ ] Create/Update Entity Configuration
  - [ ] Configure table name
  - [ ] Configure primary key
  - [ ] Configure required fields
  - [ ] Configure field lengths and types
  - [ ] Configure relationships and foreign keys
  - [ ] Configure indexes if needed
  - [ ] Configure cascade delete behavior
  - [ ] Configure default values
  - [ ] Configure unique constraints

- [ ] Add DbSet to ITenantDbContext and TenantDbContext
  - [ ] Add to interface
  - [ ] Add to implementation
  - [ ] Register configuration in OnModelCreating
  - [ ] Add any required query filters

- [ ] Create/Update Repository
  - [ ] Create repository interface
  - [ ] Implement repository class
  - [ ] Implement all CRUD operations
  - [ ] Add any specific query methods needed
  - [ ] Register in DI container
  - [ ] Implement soft delete if required
  - [ ] Add proper include statements for related entities

## 3. Application Layer
- [ ] Create/Update DTOs
  - [ ] Create List DTO (for GetAll operations)
    - [ ] Name as `{Entity}ListDto` (e.g., FacilityListDto)
    - [ ] Include only necessary fields for list view
    - [ ] Add computed properties (e.g., counts, status)
    - [ ] Add sorting fields
    - [ ] Add filtering fields
    - [ ] Implement proper inheritance if needed
    - [ ] Add display/formatted properties

  - [ ] Create Details DTO (for GetById operations)
    - [ ] Name as `{Entity}Dto` (e.g., FacilityDto)
    - [ ] Include all necessary fields for detailed view
    - [ ] Include related entity data
    - [ ] Add nested DTOs for child entities
    - [ ] Add computed properties
    - [ ] Add audit information
    - [ ] Add status information
    - [ ] Add formatted display properties

  - [ ] Create DTO for create operations
    - [ ] Name as `Create{Entity}Dto` (e.g., CreateFacilityDto)
    - [ ] Include all required fields
    - [ ] Add validation attributes
    - [ ] Add custom validators if needed
    - [ ] Handle file upload properties if needed

  - [ ] Create DTO for update operations
    - [ ] Name as `Update{Entity}Dto` (e.g., UpdateFacilityDto)
    - [ ] Include all updatable fields
    - [ ] Add validation attributes
    - [ ] Add concurrency handling if needed
    - [ ] Handle partial updates if needed

  - [ ] Add any specific DTOs needed
    - [ ] Follow naming convention: `{Purpose}{Entity}Dto`
    - [ ] For lookups: `{Entity}LookupDto`
    - [ ] For selections: `Selected{Entity}Dto`
    - [ ] For responses: `{Entity}ResponseDto`
    - [ ] For requests: `{Entity}RequestDto`

  - [ ] Add validation attributes
  - [ ] Add pagination/filtering DTOs if needed
    - [ ] Name as `{Entity}QueryParameters` if entity-specific
    - [ ] Include page number
    - [ ] Include page size
    - [ ] Include sorting parameters
    - [ ] Include filter parameters
    - [ ] Add validation for parameters

- [ ] Create/Update Service Interface
  - [ ] Define all required methods
  - [ ] Define proper return types
  - [ ] Include documentation comments
  - [ ] Define pagination/filtering methods if needed

- [ ] Create/Update Service Implementation
  - [ ] Implement all interface methods
  - [ ] Add proper validation
  - [ ] Add proper error handling
  - [ ] Use AutoMapper for mapping
  - [ ] Handle related entities properly
  - [ ] Register in DI container
  - [ ] Implement business rules
  - [ ] Add logging
  - [ ] Handle concurrent operations
  - [ ] Implement caching if needed

- [ ] Add AutoMapper Mappings
  - [ ] Entity to DTO mappings
  - [ ] DTO to Entity mappings
  - [ ] Handle nested objects
  - [ ] Configure custom value resolvers if needed
  - [ ] Add ignore mappings for sensitive data

## 4. API Layer
- [ ] Create/Update Controller
  - [ ] Add proper route attributes
  - [ ] Add proper HTTP method attributes
  - [ ] Add authorization attributes
  - [ ] Add permission requirements
  - [ ] Implement proper error handling
  - [ ] Add proper response types
  - [ ] Add XML documentation
  - [ ] Add request validation
  - [ ] Add proper HTTP status codes
  - [ ] Add pagination/filtering support
  - [ ] Add proper model binding attributes
  - [ ] Handle file uploads properly

## 5. Database
- [ ] Create Migration
  - [ ] Run migration command
  - [ ] Verify migration content
  - [ ] Apply migration
  - [ ] Add indexes for frequently queried fields
  - [ ] Add Root data seeding if required
  - [ ] Add Tenant data seeding if required

## 6. Permissions
- [ ] Add Permission Constants
  - [ ] Add to Permissions class
  - [ ] Add View permission
  - [ ] Add Create permission
  - [ ] Add Edit permission
  - [ ] Add Delete permission
  - [ ] Add any specific permissions
  - [ ] Add manage permissions for admin operations

- [ ] Update Permission Seeders
  - [ ] Add to TenantPermissionSeeder
  - [ ] Add to PermissionSeeder for root admin
  - [ ] Create migration for new permissions
  - [ ] Apply migration
  - [ ] Verify permissions are assigned to default roles

## 7. Integration
- [ ] External Services Integration
  - [ ] Configure service settings
  - [ ] Add interface if needed
  - [ ] Implement service if needed
  - [ ] Register in DI container
  - [ ] Add proper error handling
  - [ ] Add proper configuration validation
  - [ ] Add retry policies
  - [ ] Add circuit breaker if needed
  - [ ] Add proper timeouts
  - [ ] Handle service unavailability
  - [ ] Add proper authentication
  - [ ] Add proper logging

## 8. Final Verification
- [ ] Code Review
  - [ ] Check all required parameters in method calls
  - [ ] Verify proper error handling
  - [ ] Check proper use of async/await
  - [ ] Verify proper disposal of resources
  - [ ] Check for potential memory leaks
  - [ ] Verify proper use of transactions if needed
  - [ ] Check for N+1 query problems
  - [ ] Verify proper use of includes
  - [ ] Check for proper null checks
  - [ ] Verify proper exception handling
  - [ ] Check for proper validation
  - [ ] Verify proper use of cancellation tokens

- [ ] Dependency Injection
  - [ ] Verify all services are registered
  - [ ] Verify proper lifetime scope
  - [ ] Verify all dependencies are resolved
  - [ ] Check for circular dependencies
  - [ ] Verify scoped services usage

- [ ] Security Review
  - [ ] Check for proper authorization
  - [ ] Verify proper handling of sensitive data
  - [ ] Check for proper input validation
  - [ ] Verify proper file upload handling
  - [ ] Check for proper error message handling
  - [ ] Verify proper CORS configuration
  - [ ] Check for proper rate limiting
