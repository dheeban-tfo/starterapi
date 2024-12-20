using System;
using System.Collections.Generic;
using System.Linq;
using StarterApi.Domain.Entities;

public class User : BaseEntity
{
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public UserType UserType { get; private set; }
    public bool IsActive { get; private set; }
    
    private readonly List<UserTenant> _userTenants = new();
    public IReadOnlyCollection<UserTenant> UserTenants => _userTenants.AsReadOnly();

    private User() : base() { } // For EF Core

    public User(string email, string passwordHash, string firstName, string lastName, UserType userType) : base()
    {
        Email = email;
        PasswordHash = passwordHash;
        FirstName = firstName;
        LastName = lastName;
        UserType = userType;
        IsActive = true;
    }

    public void AddTenant(Tenant tenant, Guid roleId)
    {
        if (_userTenants.Any(ut => ut.TenantId == tenant.Id))
            throw new InvalidOperationException("User already has access to this tenant");

        _userTenants.Add(new UserTenant(this, tenant, roleId));
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }
} 