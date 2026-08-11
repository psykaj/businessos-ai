using System;
using backend.Modules.BusinessMemory.Enums;

namespace backend.Modules.BusinessMemory.DTOs;

public class CreateMemoryDto
{
    public MemoryType MemoryType { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? StructuredData { get; set; }
    public string SourceModule { get; set; } = string.Empty;
    public string SourceEntityId { get; set; } = string.Empty;
    public MemoryImportance Importance { get; set; } = MemoryImportance.Medium;
    public MemoryConfidence Confidence { get; set; } = MemoryConfidence.Medium;
    public DateTime? ExpiresAt { get; set; }
}
