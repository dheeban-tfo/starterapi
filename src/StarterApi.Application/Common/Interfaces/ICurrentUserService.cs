using System;

namespace StarterApi.Application.Common.Interfaces
{
    public interface ICurrentUserService
    {
        Guid? UserId { get; }
        string? Email { get; }
        Guid? TenantId { get; }
        string? TenantName { get; }
        Guid? RoleId { get; }
    }
} 