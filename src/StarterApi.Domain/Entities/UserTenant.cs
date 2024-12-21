using StarterApi.Domain.Entities;

public class UserTenant : BaseEntity
{
    public Guid UserId { get;  set; }
    public User User { get;  set; }
    
    public Guid TenantId { get;  set; }
    public Tenant Tenant { get;  set; }
    
    public Guid RoleId { get;  set; }

    //private UserTenant() : base() { } // For EF Core

} 