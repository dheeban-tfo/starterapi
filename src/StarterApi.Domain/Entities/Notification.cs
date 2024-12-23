 namespace StarterApi.Domain.Entities
{
    public class Notification : BaseEntity
    {
        public Guid RecipientId { get; set; }
        public string Message { get; set; }
        public string Type { get; set; }
        public string SentBy { get; set; }
        public string Status { get; set; }
        public DateTime SentAt { get; set; }
        public string NotificationType { get; set; }
        public string ReferenceId { get; set; }
        public string ReferenceType { get; set; }

        public virtual Resident Recipient { get; set; }
    }
} 