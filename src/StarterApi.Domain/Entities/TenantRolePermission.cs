public class TenantRolePermission : BaseEntity
{
    public Guid RoleId { get; set; }
    public Guid PermissionId { get; set; }
    
    public virtual TenantRole Role { get; set; }
    public virtual TenantPermission Permission { get; set; }
} 