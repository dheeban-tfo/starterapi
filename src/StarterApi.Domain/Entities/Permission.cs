using System.Collections.Generic;

namespace StarterApi.Domain.Entities
{
    public class Permission : BaseEntity
    {
        public string Name { get; set; }
        public string SystemName { get; set; }
        public string Description { get; set; }
        public string Group { get; set; }
        public bool IsSystem { get; set; } = true;

        // Navigation property for role permissions
        public virtual ICollection<RolePermission> RolePermissions { get; set; }
    }
} 