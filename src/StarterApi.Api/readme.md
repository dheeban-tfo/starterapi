
## Features

- Multi-tenant architecture with database per tenant
- Clean Architecture implementation
- Automated tenant database creation
- Tenant resolution middleware
- Root database for tenant management
- Swagger UI integration

## Database Structure

### Root Database
- Manages tenants and root-level users
- Tables:
  - Tenants
  - Users
  - UserTenants

### Tenant Databases
- Separate database for each tenant
- Tables:
  - Users
  - Roles

## Getting Started

1. **Prerequisites**
   - .NET 8 SDK
   - SQL Server
   - Visual Studio/VS Code

2. **Database Configuration**
   ```json
   {
     "ConnectionStrings": {
       "RootDatabase": "Server=localhost;Database=StarterApi_Root;User Id=sa;Password=MyPass@word;TrustServerCertificate=True;"
     }
   }
   ```

3. **Run Migrations**
   ```bash
   cd src/StarterApi.Infrastructure
   dotnet ef database update --context RootDbContext
   ```

4. **Run the Application**
   ```bash
   cd src/StarterApi.Api
   dotnet run
   ```

## API Endpoints

### Tenant Management
- `POST /api/tenants` - Create new tenant
  ```json
  {
    "name": "TenantName",
    "databaseName": "Tenant_DB"
  }
  ```
- `GET /api/tenants` - List all tenants
- `GET /api/tenants/{id}` - Get specific tenant
- `GET /api/tenants/current` - Get current tenant (requires X-TenantId header)

## Multi-tenancy Implementation

1. **Tenant Resolution**
   - Uses `X-TenantId` header
   - Middleware validates tenant existence and status

2. **Database Creation**
   - Automatic creation on tenant registration
   - Applies migrations
   - Seeds default data (roles)

3. **Data Isolation**
   - Complete database isolation per tenant
   - Root database for tenant management

## Development Workflow

1. **Adding New Tenant Features**
   ```bash
  # Make sure you're in the Infrastructure project directory
cd src/StarterApi.Infrastructure

# Create Root DB migration
dotnet ef migrations add InitialRootSchema --context RootDbContext --output-dir Persistence/Migrations/RootDb

# Create Tenant DB migration
dotnet ef migrations add InitialTenantSchema --context TenantDbContext --output-dir Persistence/Migrations/TenantDb

# To Update Database
cd ../StarterApi.Api
dotnet ef database update --context RootDbContext

dotnet ef migrations add AddRefreshTokensTable --context RootDbContext

   ```

2. **Adding Root Features**
   ```bash
   # Create new root database migration
   dotnet ef migrations add MigrationName --context RootDbContext
   ```

## Security Considerations

- Tenant isolation at database level
- Connection string security
- Tenant validation middleware
- Role-based access control

## Current Status

✅ Implemented:
- Multi-tenant architecture
- Tenant database creation
- Root database management
- Basic API endpoints
- Swagger documentation
- Tenant resolution middleware

🚧 Todo:
- Authentication/Authorization
- Tenant user management
- Role-based access control
- Tenant database backup/restore
- Tenant data migration tools





6. Register the middleware in Program.cs:

```csharp:src/StarterApi.Api/Program.cs
// ... existing using statements ...
using StarterApi.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

// ... existing configurations ...

// JWT and Auth configurations
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.AddScoped<IJwtService, JwtService>();

var app = builder.Build();

// ... existing middleware ...

// Add JWT Authorization middleware before MVC
app.UseMiddleware<JwtAuthorizationMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

// ... rest of configuration
```


Now let's test the complete flow:

1. **Request OTP:**
```bash
curl -X POST 'http://localhost:5126/api/Auth/request-otp' \
-H 'Content-Type: application/json' \
-d '{
  "mobileNumber": "+1234567890"
}'
```


2. **Verify OTP (Get Base Token):**
```bash
curl -X POST 'http://localhost:5126/api/Auth/verify-otp' \
-H 'Content-Type: application/json' \
-d '{
  "mobileNumber": "+1234567890",
  "otpCode": "111000"
}'
```
Response:
```json
{
  "baseToken": "eyJhbG...",
  "refreshToken": "abc...",
  "user": {
    "id": "guid",
    "firstName": "Test",
    "lastName": "User"
  },
  "availableTenants": [
    {
      "tenantId": "guid1",
      "tenantName": "Alpha",
      "role": "Admin"
    }
  ]
}
```


3. **Set Tenant (Get Tenant Token):**
```bash
curl -X POST 'http://localhost:5126/api/Auth/set-tenant' \
-H 'Content-Type: application/json' \
-d '{
  "baseToken": "eyJhbG...",
  "tenantId": "guid1"
}'
```
Response:
```json
{
  "accessToken": "eyJhbG...",
  "tenantContext": {
    "tenantId": "guid1",
    "tenantName": "Alpha",
    "role": "Admin",
    "permissions": []
  }
}
```


4. **Access Protected Resource:**
```bash
curl -X GET 'http://localhost:5126/api/Users' \
-H 'Authorization: Bearer eyJhbG...'
```


The flow ensures:
1. Initial authentication gives base token
2. User can see available tenants
3. Tenant selection gives tenant-specific token
4. Protected resources require tenant token

Would you like me to explain any part in more detail or proceed with implementing the permissions system?
