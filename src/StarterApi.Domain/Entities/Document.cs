using System;
using System.Collections.Generic;

namespace StarterApi.Domain.Entities
{
    public class Document : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string BlobUrl { get; set; }  // URL to access the file in Azure Blob Storage
        public string BlobPath { get; set; }  // Container/path within Azure Blob Storage
        public string FileType { get; set; }
        public long Size { get; set; }
        public string Category { get; set; }
        public Guid? CategoryId { get; set; }
        public int CurrentVersion { get; set; }
        public string ContentType { get; set; }  // MIME type of the document
        
        // Navigation properties
        public virtual DocumentCategory DocumentCategory { get; set; }
        private readonly List<DocumentVersion> _versions = new();
        public IReadOnlyCollection<DocumentVersion> Versions => _versions.AsReadOnly();
        private readonly List<DocumentAccess> _accessControls = new();
        public IReadOnlyCollection<DocumentAccess> AccessControls => _accessControls.AsReadOnly();

        // Block and Unit references (optional)
        public Guid? BlockId { get; set; }
        public virtual Block Block { get; set; }
        public Guid? UnitId { get; set; }
        public virtual Unit Unit { get; set; }
    }
}
