using System;
using System.Collections.Generic;


namespace StarterApi.Domain.Entities
{
    public class Tenant : BaseEntity
    {
        public string Name { get; private set; }
        public string DatabaseName { get; private set; }
        public string ConnectionString { get; private set; }
        public TenantStatus Status { get; private set; }
        
        private readonly List<UserTenant> _userTenants = new();
        public IReadOnlyCollection<UserTenant> UserTenants => _userTenants.AsReadOnly();

        private Tenant() : base() { } // For EF Core

        public Tenant(string name, string databaseName) : base()
        {
            Name = name;
            DatabaseName = databaseName;
            Status = TenantStatus.Active;
            ConnectionString = $"Server=localhost;Database={databaseName};User Id=sa;Password=MyPass@word;TrustServerCertificate=True;MultipleActiveResultSets=true;";
        }
         

        public void Deactivate()
        {
            Status = TenantStatus.Inactive;
            UpdatedAt = DateTime.UtcNow;
        }
    }
} 