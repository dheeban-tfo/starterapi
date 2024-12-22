public class RoleDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<string> Permissions { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateRoleDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public List<string> Permissions { get; set; }
}

public class UpdateRoleDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public List<string> Permissions { get; set; }
} 