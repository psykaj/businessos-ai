using System;
using backend.Modules.BusinessMemory.Enums;

namespace backend.Modules.BusinessMemory.DTOs;

public class MemoryDto
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public MemoryType MemoryType { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? StructuredData { get; set; }
    public string SourceModule { get; set; } = string.Empty;
    public string SourceEntityId { get; set; } = string.Empty;
    public MemoryImportance Importance { get; set; }
    public MemoryConfidence Confidence { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public DateTime? LastUsedAt { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
