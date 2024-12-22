public class PermissionDto
{
    public string Name { get; set; }
    public string SystemName { get; set; }
    public string Description { get; set; }
    public string Group { get; set; }
    public bool IsEnabled { get; set; }
    public bool IsSystem { get; set; }
}

public class RolePermissionUpdateDto
{
    public List<string> Permissions { get; set; }
} 