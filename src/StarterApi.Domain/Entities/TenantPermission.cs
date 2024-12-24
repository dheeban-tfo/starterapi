public class TenantPermission : BaseEntity
{
    public string Name { get; set; }
    public string SystemName { get; set; }
    public string Description { get; set; }
    public string Group { get; set; }
    public bool IsEnabled { get; set; }
    public bool IsSystem { get; set; }
    public virtual ICollection<TenantRolePermission> RolePermissions { get; set; }
} 