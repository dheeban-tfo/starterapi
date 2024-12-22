public class UserRoleDto
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
    public string RoleName { get; set; }
}

public class UpdateUserRoleDto
{
    public Guid RoleId { get; set; }
}

public class UsersByRoleDto
{
    public Guid UserId { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
} 