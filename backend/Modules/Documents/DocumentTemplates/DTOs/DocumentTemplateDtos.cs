namespace backend.Modules.Documents.DocumentTemplates.DTOs;

public record DocumentTemplateDto(
    Guid Id,
    Guid OrganizationId,
    string Name,
    string? Description,
    string Category,
    string Content,
    string? FieldsJson,
    bool IsActive,
    DateTime CreatedAt,
    DateTime UpdatedAt
);

public record CreateDocumentTemplateDto(
    string Name,
    string? Description,
    string Category,
    string Content,
    string? FieldsJson
);

public record UpdateDocumentTemplateDto(
    string Name,
    string? Description,
    string Category,
    string Content,
    string? FieldsJson,
    bool IsActive
);

public record RenderTemplateDto(
    Guid? FolderId,
    string DocumentName,
    Dictionary<string, string> FieldValues
);
