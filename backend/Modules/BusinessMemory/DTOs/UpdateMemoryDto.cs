using System;
using backend.Modules.BusinessMemory.Enums;

namespace backend.Modules.BusinessMemory.DTOs;

public class UpdateMemoryDto
{
    public string? Title { get; set; }
    public string? Content { get; set; }
    public string? StructuredData { get; set; }
    public MemoryImportance? Importance { get; set; }
    public MemoryConfidence? Confidence { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public bool? IsActive { get; set; }
}
