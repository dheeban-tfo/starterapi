public class TenantDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string DatabaseName { get; set; }
    public string Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
} 