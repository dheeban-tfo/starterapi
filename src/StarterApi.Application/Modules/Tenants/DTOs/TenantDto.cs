// Public DTO - for API responses
public class TenantDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string DatabaseName { get; set; }
    public string Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

// Internal DTO - for service-to-service communication
public class TenantInternalDto : TenantDto
{
    public string ConnectionString { get; set; }
} 