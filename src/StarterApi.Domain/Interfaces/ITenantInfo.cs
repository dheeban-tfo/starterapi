namespace StarterApi.Domain.Interfaces
{
    public interface ITenantInfo
    {
        Guid Id { get; }
        string Name { get; }
        string ConnectionString { get; }
    }
} 