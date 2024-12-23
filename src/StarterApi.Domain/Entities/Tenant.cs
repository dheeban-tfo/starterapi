using System;
using System.Collections.Generic;
namespace StarterApi.Domain.Entities
{
    public class Tenant : BaseEntity
    {
        public string Name { get; set; }
        public string DatabaseName { get; set; }
        public string ConnectionString { get; set; }
        public TenantStatus Status { get; set; }
        
        private readonly List<UserTenant> _userTenants = new();
        public IReadOnlyCollection<UserTenant> UserTenants => _userTenants.AsReadOnly();

        
    }
} 