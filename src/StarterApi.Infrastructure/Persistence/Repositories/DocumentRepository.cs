using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StarterApi.Application.Common.Interfaces;
using StarterApi.Domain.Entities;
using StarterApi.Application.Common.Exceptions;

namespace StarterApi.Infrastructure.Repositories
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly ITenantDbContext _context;

        public DocumentRepository(ITenantDbContext context)
        {
            _context = context;
        }

        public async Task<Document> GetByIdAsync(Guid id)
        {
            var document = await _context.Documents
                .Include(d => d.DocumentCategory)
                .Include(d => d.AccessControls)
                .Include(d => d.Versions)
                .FirstOrDefaultAsync(d => d.Id == id && d.IsActive);

            if (document == null)
                throw new NotFoundException($"Document with ID {id} not found");

            return document;
        }

        public async Task<IEnumerable<Document>> GetAllAsync()
        {
            return await _context.Documents
                .Include(d => d.DocumentCategory)
                .Where(d => d.IsActive)
                .OrderByDescending(d => d.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Document>> GetByUnitAsync(Guid unitId)
        {
            return await _context.Documents
                .Include(d => d.DocumentCategory)
                .Where(d => d.UnitId == unitId && d.IsActive)
                .OrderByDescending(d => d.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Document>> GetByBlockAsync(Guid blockId)
        {
            return await _context.Documents
                .Include(d => d.DocumentCategory)
                .Where(d => d.BlockId == blockId && d.IsActive)
                .OrderByDescending(d => d.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Document>> GetByCategoryAsync(Guid categoryId)
        {
            return await _context.Documents
                .Include(d => d.DocumentCategory)
                .Where(d => d.CategoryId == categoryId && d.IsActive)
                .OrderByDescending(d => d.CreatedAt)
                .ToListAsync();
        }

        public async Task<Document> CreateAsync(Document document)
        {
            await _context.Documents.AddAsync(document);
            await _context.SaveChangesAsync();
            return document;
        }

        public async Task<Document> UpdateAsync(Document document)
        {
            _context.Documents.Update(document);
            await _context.SaveChangesAsync();
            return document;
        }

        public async Task DeleteAsync(Guid id)
        {
            var document = await _context.Documents.FindAsync(id);
            if (document != null)
            {
                document.IsActive = false;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<DocumentVersion> AddVersionAsync(DocumentVersion version)
        {
            await _context.DocumentVersions.AddAsync(version);
            
            var document = await _context.Documents.FindAsync(version.DocumentId);
            if (document != null)
            {
                document.CurrentVersion = version.Version;
                _context.Documents.Update(document);
            }
            
            await _context.SaveChangesAsync();
            return version;
        }

        public async Task<IEnumerable<DocumentVersion>> GetVersionsAsync(Guid documentId)
        {
            return await _context.DocumentVersions
                .Where(v => v.DocumentId == documentId && v.IsActive)
                .OrderByDescending(v => v.Version)
                .ToListAsync();
        }

        public async Task<bool> HasAccessAsync(Guid documentId, Guid userId)
        {
            return await _context.DocumentAccesses
                .AnyAsync(a => a.DocumentId == documentId && 
                              a.UserId == userId && 
                              a.IsActive);
        }

        public async Task<DocumentAccess> GrantAccessAsync(DocumentAccess access)
        {
            var existingAccess = await _context.DocumentAccesses
                .FirstOrDefaultAsync(a => a.DocumentId == access.DocumentId && 
                                        a.UserId == access.UserId);

            if (existingAccess != null)
            {
                existingAccess.AccessLevel = access.AccessLevel;
                existingAccess.IsActive = true;
                _context.DocumentAccesses.Update(existingAccess);
            }
            else
            {
                await _context.DocumentAccesses.AddAsync(access);
            }

            await _context.SaveChangesAsync();
            return access;
        }

        public async Task RevokeAccessAsync(Guid documentId, Guid userId)
        {
            var access = await _context.DocumentAccesses
                .FirstOrDefaultAsync(a => a.DocumentId == documentId && 
                                        a.UserId == userId);

            if (access != null)
            {
                access.IsActive = false;
                await _context.SaveChangesAsync();
            }
        }
    }
} 