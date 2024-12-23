namespace StarterApi.Domain.Entities
{
    public class Announcement : BaseEntity
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public string Audience { get; set; }
        public string Priority { get; set; }
        public Guid PostedById { get; set; }
        public DateTime PostedAt { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public Guid? BlockId { get; set; }
        public Guid? UnitId { get; set; }

        public virtual Block Block { get; set; }
        public virtual Unit Unit { get; set; }
    }
} 