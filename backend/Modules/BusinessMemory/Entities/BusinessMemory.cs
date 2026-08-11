using System;
using backend.Common;
using backend.Modules.BusinessMemory.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Modules.BusinessMemory.Entities;

public class BusinessMemory : BaseEntity
{
    public Guid OrganizationId { get; set; } // Renamed from BusinessId to fit BusinessOS AI standard
    public MemoryType MemoryType { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    
    [Column(TypeName = "jsonb")]
    public string? StructuredData { get; set; }
    
    public string SourceModule { get; set; } = string.Empty;
    public string SourceEntityId { get; set; } = string.Empty;
    public MemoryImportance Importance { get; set; } = MemoryImportance.Medium;
    public MemoryConfidence Confidence { get; set; } = MemoryConfidence.Medium;
    public DateTime? ExpiresAt { get; set; }
    public DateTime? LastUsedAt { get; set; }
    
    // BaseEntity already has IsDeleted, but we also want IsActive to disable without deleting
    public bool IsActive { get; set; } = true;
}
