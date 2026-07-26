using System.Text;
using backend.Modules.Documents.Documents.DTOs;
using backend.Modules.Documents.Documents.Interfaces;
using backend.Modules.Documents.DocumentTemplates.DTOs;
using backend.Modules.Documents.DocumentTemplates.Interfaces;
using backend.Modules.Documents.Entities;

namespace backend.Modules.Documents.DocumentTemplates.Services;

public class DocumentTemplateService : IDocumentTemplateService
{
    private readonly IDocumentTemplateRepository _repository;
    private readonly IDocumentService _documentService;

    public DocumentTemplateService(
        IDocumentTemplateRepository repository,
        IDocumentService documentService)
    {
        _repository = repository;
        _documentService = documentService;
    }

    public async Task<DocumentTemplateDto> CreateAsync(Guid organizationId, CreateDocumentTemplateDto dto)
    {
        var template = new DocumentTemplate
        {
            OrganizationId = organizationId,
            Name = dto.Name.Trim(),
            Description = dto.Description,
            Category = dto.Category ?? "General",
            Content = dto.Content,
            FieldsJson = dto.FieldsJson,
            IsActive = true
        };

        await _repository.AddAsync(template);
        return MapToDto(template);
    }

    public async Task<DocumentTemplateDto> GetByIdAsync(Guid organizationId, Guid id)
    {
        var template = await _repository.GetByIdAsync(organizationId, id)
            ?? throw new KeyNotFoundException($"Template with ID {id} not found.");
        return MapToDto(template);
    }

    public async Task<List<DocumentTemplateDto>> GetAllAsync(Guid organizationId, string? category = null)
    {
        var templates = await _repository.GetAllAsync(organizationId, category);
        return templates.Select(MapToDto).ToList();
    }

    public async Task<DocumentTemplateDto> UpdateAsync(Guid organizationId, Guid id, UpdateDocumentTemplateDto dto)
    {
        var template = await _repository.GetByIdAsync(organizationId, id)
            ?? throw new KeyNotFoundException($"Template with ID {id} not found.");

        template.Name = dto.Name.Trim();
        template.Description = dto.Description;
        template.Category = dto.Category;
        template.Content = dto.Content;
        template.FieldsJson = dto.FieldsJson;
        template.IsActive = dto.IsActive;

        await _repository.UpdateAsync(template);
        return MapToDto(template);
    }

    public async Task DeleteAsync(Guid organizationId, Guid id)
    {
        var template = await _repository.GetByIdAsync(organizationId, id)
            ?? throw new KeyNotFoundException($"Template with ID {id} not found.");

        await _repository.DeleteAsync(template);
    }

    public async Task<DocumentDto> RenderDocumentFromTemplateAsync(
        Guid organizationId,
        Guid templateId,
        Guid userId,
        string userName,
        RenderTemplateDto dto)
    {
        var template = await _repository.GetByIdAsync(organizationId, templateId)
            ?? throw new KeyNotFoundException($"Template with ID {templateId} not found.");

        string renderedContent = template.Content;

        if (dto.FieldValues != null)
        {
            foreach (var kvp in dto.FieldValues)
            {
                var placeholder = $"{{{{{kvp.Key}}}}}";
                renderedContent = renderedContent.Replace(placeholder, kvp.Value);
            }
        }

        var fileName = $"{dto.DocumentName.Trim()}.html";
        var contentBytes = Encoding.UTF8.GetBytes(renderedContent);
        using var stream = new MemoryStream(contentBytes);

        var uploadDto = new UploadDocumentDto(
            dto.FolderId,
            $"Rendered from template: {template.Name}",
            new List<string> { "Template", template.Category },
            null
        );

        return await _documentService.UploadAsync(
            organizationId,
            userId,
            userName,
            stream,
            fileName,
            "text/html",
            uploadDto
        );
    }

    private static DocumentTemplateDto MapToDto(DocumentTemplate template) => new(
        template.Id,
        template.OrganizationId,
        template.Name,
        template.Description,
        template.Category,
        template.Content,
        template.FieldsJson,
        template.IsActive,
        template.CreatedAt,
        template.UpdatedAt
    );
}
