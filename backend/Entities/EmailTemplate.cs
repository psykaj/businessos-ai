using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backend.Interfaces;

using backend.Common;

namespace backend.Entities;

public class EmailTemplate : BaseEntity
{
    [Required]
    public Guid OrganizationId { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(200)]
    public string Subject { get; set; } = string.Empty;
    
    [Required]
    public string HtmlContent { get; set; } = string.Empty;
    
    [Column(TypeName = "jsonb")]
    public string Variables { get; set; } = "[]"; // JSON array of variable names
    
    public bool IsDefault { get; set; } = false;
}
