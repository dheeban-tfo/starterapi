using System;
using System.Collections.Generic;
using System.Linq;
using StarterApi.Domain.Entities;

public class User : BaseEntity
{
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public UserType UserType { get; set; }
    public bool IsActive { get; set; }
    
    private readonly List<UserTenant> _userTenants = new();
    public IReadOnlyCollection<UserTenant> UserTenants => _userTenants.AsReadOnly();

} 