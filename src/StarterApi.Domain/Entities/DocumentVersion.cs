using System;

namespace StarterApi.Domain.Entities
{
    public class DocumentVersion : BaseEntity
    {
        public Guid DocumentId { get; set; }
        public int Version { get; set; }
        public string BlobUrl { get; set; }  // URL to access the file in Azure Blob Storage
        public string BlobPath { get; set; }  // Container/path within Azure Blob Storage
        public string ChangeDescription { get; set; }
        public string ContentType { get; set; }  // MIME type of the document
        public long Size { get; set; }
        
        // Navigation property
        public virtual Document Document { get; set; }
    }
}
