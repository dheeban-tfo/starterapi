public class TenantRole : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public virtual ICollection<TenantRolePermission> RolePermissions { get; set; }
    public virtual ICollection<TenantUser> Users { get; set; }
} 