using backend.Modules.Documents.Entities;

namespace backend.Modules.Documents.DocumentTemplates.Interfaces;

public interface IDocumentTemplateRepository
{
    Task<DocumentTemplate?> GetByIdAsync(Guid organizationId, Guid id);
    Task<List<DocumentTemplate>> GetAllAsync(Guid organizationId, string? category = null);
    Task AddAsync(DocumentTemplate template);
    Task UpdateAsync(DocumentTemplate template);
    Task DeleteAsync(DocumentTemplate template);
}
