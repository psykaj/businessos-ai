using backend.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Entities;

public class CustomDomain : BaseEntity
{
    [Required]
    public Guid OrganizationId { get; set; }
    
    [ForeignKey("OrganizationId")]
    public Organization? Organization { get; set; }

    [Required]
    [MaxLength(255)]
    public string Domain { get; set; } = string.Empty;

    [MaxLength(255)]
    public string? VerificationToken { get; set; }

    [Required]
    [MaxLength(50)]
    public string VerificationStatus { get; set; } = "Pending"; // Pending, Verified, Failed

    [Required]
    [MaxLength(50)]
    public string SSLStatus { get; set; } = "Pending"; // Pending, Active, Failed

    public bool IsPrimary { get; set; }
}
