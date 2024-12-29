using System;
using System.Collections.Generic;
using StarterApi.Application.Common.Models;

namespace StarterApi.Application.Modules.Documents.DTOs
{
    public class DocumentDto
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public long FileSize { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Status { get; set; }
        public string DownloadUrl { get; set; }
        public DateTime UploadDate { get; set; }
        public AuditDto Audit { get; set; }
    }

    public class CreateDocumentDto
    {
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public long FileSize { get; set; }
        public string Description { get; set; }
        public Guid CategoryId { get; set; }
        public string Base64Content { get; set; }
    }

    public class UpdateDocumentDto
    {
        public string Description { get; set; }
        public Guid CategoryId { get; set; }
        public string Status { get; set; }
    }

    public class DocumentListDto
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public long FileSize { get; set; }
        public string Category { get; set; }
        public string Status { get; set; }
        public DateTime UploadDate { get; set; }
        public string UploadedBy { get; set; }
    }
} 