using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StarterApi.Application.Common.Models;
using StarterApi.Application.Modules.Documents.DTOs;

namespace StarterApi.Application.Modules.Documents.Interfaces
{
    public interface IDocumentService
    {
        Task<DocumentDto> GetByIdAsync(Guid id);
        Task<PagedResult<DocumentListDto>> GetDocumentsAsync(QueryParameters parameters);
        Task<DocumentDto> CreateAsync(CreateDocumentDto dto);
        Task<DocumentDto> UpdateAsync(Guid id, UpdateDocumentDto dto);
        Task<bool> DeleteAsync(Guid id);
        Task<string> GetDownloadUrlAsync(Guid id);
        Task<List<DocumentDto>> GetByIdsAsync(List<Guid> ids);
        Task<List<DocumentDto>> GetByEntityAsync(string entityType, Guid entityId);
    }
} 