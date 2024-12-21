namespace StarterApi.Domain.Entities
{
    public class Role : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid TenantId { get; set; }

        // Navigation properties
        public virtual Tenant Tenant { get; set; }
        public virtual ICollection<RolePermission> RolePermissions { get; set; }
    }
} 