using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using StarterApi.Application.Common.Interfaces;
using StarterApi.Domain.Entities;
using StarterApi.Application.Common.Models;
using StarterApi.Application.Common.Exceptions;

namespace StarterApi.Infrastructure.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IBlobStorageService _blobStorageService;
        private const string ContainerName = "documents";

        public DocumentService(IDocumentRepository documentRepository, IBlobStorageService blobStorageService)
        {
            _documentRepository = documentRepository;
            _blobStorageService = blobStorageService;
        }

        public async Task<Document> GetDocumentByIdAsync(Guid id)
        {
            return await _documentRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Document>> GetAllDocumentsAsync()
        {
            return await _documentRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Document>> GetDocumentsByUnitAsync(Guid unitId)
        {
            return await _documentRepository.GetByUnitAsync(unitId);
        }

        public async Task<IEnumerable<Document>> GetDocumentsByBlockAsync(Guid blockId)
        {
            return await _documentRepository.GetByBlockAsync(blockId);
        }

        public async Task<IEnumerable<Document>> GetDocumentsByCategoryAsync(Guid categoryId)
        {
            return await _documentRepository.GetByCategoryAsync(categoryId);
        }

        public async Task<Document> UploadDocumentAsync(string fileName, Stream content, string contentType, Guid? categoryId = null, Guid? unitId = null, Guid? blockId = null)
        {
            var (blobUrl, blobPath) = await _blobStorageService.UploadAsync(ContainerName, fileName, content, contentType);

            var document = new Document
            {
                Name = fileName,
                Description = string.Empty,
                BlobUrl = blobUrl,
                BlobPath = blobPath,
                FileType = Path.GetExtension(fileName),
                ContentType = contentType,
                Size = content.Length,
                CategoryId = categoryId,
                Category = categoryId.HasValue ? null : "Uncategorized",
                UnitId = unitId,
                BlockId = blockId,
                CurrentVersion = 1
            };

            await _documentRepository.CreateAsync(document);

            var version = new DocumentVersion
            {
                DocumentId = document.Id,
                Version = 1,
                BlobUrl = blobUrl,
                BlobPath = blobPath,
                ContentType = contentType,
                Size = content.Length,
                ChangeDescription = "Initial version"
            };

            await _documentRepository.AddVersionAsync(version);

            return document;
        }

        public async Task<Document> UpdateDocumentAsync(Guid id, string fileName, Stream content, string contentType)
        {
            var document = await _documentRepository.GetByIdAsync(id);
            if (document == null)
                throw new Exception("Document not found");

            var (blobUrl, blobPath) = await _blobStorageService.UploadAsync(ContainerName, fileName, content, contentType);

            document.Name = fileName;
            document.BlobUrl = blobUrl;
            document.BlobPath = blobPath;
            document.FileType = Path.GetExtension(fileName);
            document.ContentType = contentType;
            document.Size = content.Length;

            await _documentRepository.UpdateAsync(document);

            return document;
        }

        public async Task DeleteDocumentAsync(Guid id)
        {
            var document = await _documentRepository.GetByIdAsync(id);
            if (document == null)
                return;

            await _blobStorageService.DeleteAsync(ContainerName, document.BlobPath);
            await _documentRepository.DeleteAsync(id);
        }

        public async Task<DocumentVersion> AddVersionAsync(Guid documentId, string fileName, Stream content, string contentType, string changeDescription)
        {
            var document = await _documentRepository.GetByIdAsync(documentId);
            if (document == null)
                throw new Exception("Document not found");

            var (blobUrl, blobPath) = await _blobStorageService.UploadAsync(ContainerName, fileName, content, contentType);

            var version = new DocumentVersion
            {
                DocumentId = documentId,
                Version = document.CurrentVersion + 1,
                BlobUrl = blobUrl,
                BlobPath = blobPath,
                ContentType = contentType,
                Size = content.Length,
                ChangeDescription = changeDescription
            };

            await _documentRepository.AddVersionAsync(version);

            document.BlobUrl = blobUrl;
            document.BlobPath = blobPath;
            document.ContentType = contentType;
            document.Size = content.Length;
            await _documentRepository.UpdateAsync(document);

            return version;
        }

        public async Task<IEnumerable<DocumentVersion>> GetVersionsAsync(Guid documentId)
        {
            return await _documentRepository.GetVersionsAsync(documentId);
        }

        public async Task<Stream> DownloadDocumentAsync(Guid documentId, int? version = null)
        {
            var document = await _documentRepository.GetByIdAsync(documentId);
            if (document == null)
                throw new Exception("Document not found");

            if (version.HasValue)
            {
                var versions = await _documentRepository.GetVersionsAsync(documentId);
                var specificVersion = versions.FirstOrDefault(v => v.Version == version.Value);
                if (specificVersion != null)
                {
                    return await _blobStorageService.DownloadAsync(ContainerName, specificVersion.BlobPath);
                }
            }

            return await _blobStorageService.DownloadAsync(ContainerName, document.BlobPath);
        }

        public async Task<bool> HasAccessAsync(Guid documentId, Guid userId)
        {
            return await _documentRepository.HasAccessAsync(documentId, userId);
        }

        public async Task<DocumentAccess> GrantAccessAsync(Guid documentId, Guid userId, string accessLevel)
        {
            var access = new DocumentAccess
            {
                DocumentId = documentId,
                UserId = userId,
                AccessLevel = accessLevel
            };

            return await _documentRepository.GrantAccessAsync(access);
        }

        public async Task RevokeAccessAsync(Guid documentId, Guid userId)
        {
            await _documentRepository.RevokeAccessAsync(documentId, userId);
        }

        public async Task<PagedResult<Document>> GetDocumentsAsync(QueryParameters parameters)
        {
            return await _documentRepository.GetDocumentsAsync(parameters);
        }

        public async Task<byte[]> GetDocumentContentAsync(Guid id)
        {
            var document = await _documentRepository.GetByIdAsync(id);
            if (document == null)
                throw new NotFoundException($"Document with ID {id} not found");

            using var stream = await _blobStorageService.DownloadAsync(ContainerName, document.BlobPath);
            using var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }
    }
}
