public class TenantRole : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    
    public virtual ICollection<TenantPermission> Permissions { get; set; }
} 