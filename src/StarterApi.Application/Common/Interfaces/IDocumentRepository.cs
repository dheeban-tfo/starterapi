using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StarterApi.Domain.Entities;
using StarterApi.Application.Common.Models;

namespace StarterApi.Application.Common.Interfaces
{
    public interface IDocumentRepository
    {
        Task<Document> GetByIdAsync(Guid id);
        Task<IEnumerable<Document>> GetAllAsync();
        Task<PagedResult<Document>> GetDocumentsAsync(QueryParameters parameters);
        Task<IEnumerable<Document>> GetByUnitAsync(Guid unitId);
        Task<IEnumerable<Document>> GetByBlockAsync(Guid blockId);
        Task<IEnumerable<Document>> GetByCategoryAsync(Guid categoryId);
        Task<Document> CreateAsync(Document document);
        Task<Document> UpdateAsync(Document document);
        Task DeleteAsync(Guid id);
        Task<DocumentVersion> AddVersionAsync(DocumentVersion version);
        Task<IEnumerable<DocumentVersion>> GetVersionsAsync(Guid documentId);
        Task<bool> HasAccessAsync(Guid documentId, Guid userId);
        Task<DocumentAccess> GrantAccessAsync(DocumentAccess access);
        Task RevokeAccessAsync(Guid documentId, Guid userId);
    }
}
