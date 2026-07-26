using backend.Modules.Documents.Documents.DTOs;
using backend.Modules.Documents.Documents.Interfaces;
using backend.Modules.Documents.Entities;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.Documents.Documents.Repositories;

public class DocumentRepository : IDocumentRepository
{
    private readonly ApplicationDbContext _dbContext;

    public DocumentRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Document?> GetByIdAsync(Guid organizationId, Guid id, bool includeDeleted = false)
    {
        var query = _dbContext.Documents
            .Include(d => d.Versions)
            .Include(d => d.Folder)
            .Where(d => d.OrganizationId == organizationId && d.Id == id);

        if (!includeDeleted)
        {
            query = query.Where(d => !d.IsDeleted);
        }

        return await query.FirstOrDefaultAsync();
    }

    public async Task<List<Document>> GetByFolderIdAsync(Guid organizationId, Guid? folderId)
    {
        return await _dbContext.Documents
            .AsNoTracking()
            .Where(d => d.OrganizationId == organizationId && d.FolderId == folderId && !d.IsDeleted)
            .OrderBy(d => d.Name)
            .ToListAsync();
    }

    public async Task<List<Document>> GetFavoritesAsync(Guid organizationId)
    {
        return await _dbContext.Documents
            .AsNoTracking()
            .Where(d => d.OrganizationId == organizationId && d.IsFavorite && !d.IsDeleted)
            .OrderByDescending(d => d.UpdatedAt)
            .ToListAsync();
    }

    public async Task<(List<Document> Items, int TotalCount)> SearchAsync(Guid organizationId, DocumentSearchQueryDto searchQuery)
    {
        var query = _dbContext.Documents.AsQueryable()
            .Where(d => d.OrganizationId == organizationId);

        if (!searchQuery.IncludeDeleted)
        {
            query = query.Where(d => !d.IsDeleted);
        }

        if (searchQuery.FolderId.HasValue)
        {
            query = query.Where(d => d.FolderId == searchQuery.FolderId.Value);
        }

        if (!string.IsNullOrWhiteSpace(searchQuery.Status))
        {
            query = query.Where(d => d.Status.ToLower() == searchQuery.Status.ToLower());
        }

        if (!string.IsNullOrWhiteSpace(searchQuery.MimeType))
        {
            query = query.Where(d => d.MimeType.ToLower().Contains(searchQuery.MimeType.ToLower()));
        }

        if (searchQuery.IsFavorite.HasValue)
        {
            query = query.Where(d => d.IsFavorite == searchQuery.IsFavorite.Value);
        }

        if (!string.IsNullOrWhiteSpace(searchQuery.Tag))
        {
            query = query.Where(d => d.Tags != null && d.Tags.ToLower().Contains(searchQuery.Tag.ToLower()));
        }

        if (!string.IsNullOrWhiteSpace(searchQuery.Query))
        {
            var term = searchQuery.Query.Trim().ToLower();
            query = query.Where(d =>
                d.Name.ToLower().Contains(term) ||
                (d.Description != null && d.Description.ToLower().Contains(term)) ||
                (d.FileExtension != null && d.FileExtension.ToLower().Contains(term)));
        }

        var totalCount = await query.CountAsync();

        // Dynamic sorting
        query = searchQuery.SortBy.ToLower() switch
        {
            "name" => searchQuery.Descending ? query.OrderByDescending(d => d.Name) : query.OrderBy(d => d.Name),
            "filesize" => searchQuery.Descending ? query.OrderByDescending(d => d.FileSize) : query.OrderBy(d => d.FileSize),
            "createdat" => searchQuery.Descending ? query.OrderByDescending(d => d.CreatedAt) : query.OrderBy(d => d.CreatedAt),
            _ => searchQuery.Descending ? query.OrderByDescending(d => d.UpdatedAt) : query.OrderBy(d => d.UpdatedAt)
        };

        var items = await query
            .Skip((searchQuery.Page - 1) * searchQuery.PageSize)
            .Take(searchQuery.PageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task AddAsync(Document document)
    {
        await _dbContext.Documents.AddAsync(document);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Document document)
    {
        _dbContext.Documents.Update(document);
        await _dbContext.SaveChangesAsync();
    }

    public async Task AddVersionAsync(DocumentVersion version)
    {
        await _dbContext.DocumentVersions.AddAsync(version);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<DocumentVersion?> GetVersionByIdAsync(Guid organizationId, Guid versionId)
    {
        return await _dbContext.DocumentVersions
            .FirstOrDefaultAsync(v => v.OrganizationId == organizationId && v.Id == versionId && !v.IsDeleted);
    }

    public async Task<List<DocumentVersion>> GetVersionsByDocumentIdAsync(Guid organizationId, Guid documentId)
    {
        return await _dbContext.DocumentVersions
            .Where(v => v.OrganizationId == organizationId && v.DocumentId == documentId && !v.IsDeleted)
            .OrderByDescending(v => v.VersionNumber)
            .ToListAsync();
    }
}
