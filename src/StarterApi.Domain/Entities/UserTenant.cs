using StarterApi.Domain.Entities;

public class UserTenant : BaseEntity
{
    public Guid UserId { get; private set; }
    public User User { get; private set; }
    
    public Guid TenantId { get; private set; }
    public Tenant Tenant { get; private set; }
    
    public Guid RoleId { get; private set; }

    private UserTenant() : base() { } // For EF Core

    public UserTenant(User user, Tenant tenant, Guid roleId) : base()
    {
        User = user ?? throw new ArgumentNullException(nameof(user));
        UserId = user.Id;
        Tenant = tenant ?? throw new ArgumentNullException(nameof(tenant));
        TenantId = tenant.Id;
        RoleId = roleId;
    }
} 