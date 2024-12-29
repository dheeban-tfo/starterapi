using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StarterApi.Application.Common.Interfaces;
using StarterApi.Application.Common.Models;
using StarterApi.Domain.Entities;

namespace StarterApi.Application.Modules.Documents.Interfaces
{
    public interface IDocumentRepository : IBaseRepository<Document>
    {
        Task<Document> GetByIdWithDetailsAsync(Guid id);
        Task<List<Document>> GetByIdsAsync(List<Guid> ids);
        Task<List<Document>> GetByEntityAsync(string entityType, Guid entityId);
        Task<bool> HasAccessAsync(Guid documentId, Guid userId);
        Task<DocumentAccess> GrantAccessAsync(DocumentAccess access);
        Task RevokeAccessAsync(Guid documentId, Guid userId);
    }
} 