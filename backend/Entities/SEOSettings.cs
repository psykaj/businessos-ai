using backend.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Entities;

public class SEOSettings : BaseEntity
{
    [Required]
    public Guid OrganizationId { get; set; }
    
    [ForeignKey("OrganizationId")]
    public Organization? Organization { get; set; }

    [MaxLength(255)]
    public string? MetaTitle { get; set; }

    [MaxLength(500)]
    public string? MetaDescription { get; set; }

    [MaxLength(500)]
    public string? Keywords { get; set; }

    [MaxLength(500)]
    public string? CanonicalUrl { get; set; }

    [MaxLength(255)]
    public string? OpenGraphTitle { get; set; }

    [MaxLength(500)]
    public string? OpenGraphDescription { get; set; }

    [MaxLength(500)]
    public string? OpenGraphImage { get; set; }

    [MaxLength(50)]
    public string TwitterCard { get; set; } = "summary_large_image";

    [MaxLength(50)]
    public string Robots { get; set; } = "index, follow";
}
