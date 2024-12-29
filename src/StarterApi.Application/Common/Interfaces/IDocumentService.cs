using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using StarterApi.Domain.Entities;
using StarterApi.Application.Common.Models;

namespace StarterApi.Application.Common.Interfaces
{
    public interface IDocumentService
    {
        Task<Document> GetDocumentByIdAsync(Guid id);
        Task<IEnumerable<Document>> GetAllDocumentsAsync();
        Task<PagedResult<Document>> GetDocumentsAsync(QueryParameters parameters);
        Task<IEnumerable<Document>> GetDocumentsByUnitAsync(Guid unitId);
        Task<IEnumerable<Document>> GetDocumentsByBlockAsync(Guid blockId);
        Task<IEnumerable<Document>> GetDocumentsByCategoryAsync(Guid categoryId);
        Task<Document> UploadDocumentAsync(string fileName, Stream content, string contentType, Guid? categoryId = null, Guid? unitId = null, Guid? blockId = null);
        Task<Document> UpdateDocumentAsync(Guid id, string fileName, Stream content, string contentType);
        Task DeleteDocumentAsync(Guid id);
        Task<DocumentVersion> AddVersionAsync(Guid documentId, string fileName, Stream content, string contentType, string changeDescription);
        Task<IEnumerable<DocumentVersion>> GetVersionsAsync(Guid documentId);
        Task<Stream> DownloadDocumentAsync(Guid documentId, int? version = null);
        Task<bool> HasAccessAsync(Guid documentId, Guid userId);
        Task<DocumentAccess> GrantAccessAsync(Guid documentId, Guid userId, string accessLevel);
        Task RevokeAccessAsync(Guid documentId, Guid userId);
        Task<byte[]> GetDocumentContentAsync(Guid id);
    }
}
