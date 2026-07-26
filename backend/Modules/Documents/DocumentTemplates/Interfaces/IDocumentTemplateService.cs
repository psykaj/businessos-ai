using backend.Modules.Documents.Documents.DTOs;
using backend.Modules.Documents.DocumentTemplates.DTOs;

namespace backend.Modules.Documents.DocumentTemplates.Interfaces;

public interface IDocumentTemplateService
{
    Task<DocumentTemplateDto> CreateAsync(Guid organizationId, CreateDocumentTemplateDto dto);
    Task<DocumentTemplateDto> GetByIdAsync(Guid organizationId, Guid id);
    Task<List<DocumentTemplateDto>> GetAllAsync(Guid organizationId, string? category = null);
    Task<DocumentTemplateDto> UpdateAsync(Guid organizationId, Guid id, UpdateDocumentTemplateDto dto);
    Task DeleteAsync(Guid organizationId, Guid id);
    Task<DocumentDto> RenderDocumentFromTemplateAsync(Guid organizationId, Guid templateId, Guid userId, string userName, RenderTemplateDto dto);
}
