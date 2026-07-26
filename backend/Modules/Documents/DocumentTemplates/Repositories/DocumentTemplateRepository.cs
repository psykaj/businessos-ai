using backend.Modules.Documents.DocumentTemplates.Interfaces;
using backend.Modules.Documents.Entities;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.Documents.DocumentTemplates.Repositories;

public class DocumentTemplateRepository : IDocumentTemplateRepository
{
    private readonly ApplicationDbContext _dbContext;

    public DocumentTemplateRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<DocumentTemplate?> GetByIdAsync(Guid organizationId, Guid id)
    {
        return await _dbContext.DocumentTemplates
            .FirstOrDefaultAsync(t => t.OrganizationId == organizationId && t.Id == id && !t.IsDeleted);
    }

    public async Task<List<DocumentTemplate>> GetAllAsync(Guid organizationId, string? category = null)
    {
        var query = _dbContext.DocumentTemplates
            .AsNoTracking()
            .Where(t => t.OrganizationId == organizationId && !t.IsDeleted);

        if (!string.IsNullOrWhiteSpace(category))
        {
            query = query.Where(t => t.Category.ToLower() == category.ToLower());
        }

        return await query.OrderBy(t => t.Name).ToListAsync();
    }

    public async Task AddAsync(DocumentTemplate template)
    {
        await _dbContext.DocumentTemplates.AddAsync(template);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(DocumentTemplate template)
    {
        _dbContext.DocumentTemplates.Update(template);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(DocumentTemplate template)
    {
        template.IsDeleted = true;
        template.DeletedAt = DateTime.UtcNow;
        _dbContext.DocumentTemplates.Update(template);
        await _dbContext.SaveChangesAsync();
    }
}
