public class RoleDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public List<PermissionDto> Permissions { get; set; }
}

public class PermissionDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string SystemName { get; set; }
    public string Group { get; set; }
} 