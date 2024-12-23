namespace StarterApi.Domain.Entities
{
    public class AuditHistory : BaseEntity
    {
        public string TableName { get; set; }
        public string RecordId { get; set; }
        public string Action { get; set; }
        public Guid ChangedBy { get; set; }
        public DateTime ChangedAt { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public string IpAddress { get; set; }
        public string UserAgent { get; set; }
    }
} 