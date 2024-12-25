using System;
using System.Collections.Generic;

namespace StarterApi.Domain.Entities
{
    public class DocumentCategory : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid? ParentCategoryId { get; set; }
        
        // Navigation properties
        public virtual DocumentCategory ParentCategory { get; set; }
        private readonly List<DocumentCategory> _childCategories = new();
        public IReadOnlyCollection<DocumentCategory> ChildCategories => _childCategories.AsReadOnly();
        private readonly List<Document> _documents = new();
        public IReadOnlyCollection<Document> Documents => _documents.AsReadOnly();
    }
}
