using backend.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Entities;

public class LandingPage : BaseEntity
{
    [Required]
    public Guid OrganizationId { get; set; }
    
    [ForeignKey("OrganizationId")]
    public Organization? Organization { get; set; }

    [Required]
    [MaxLength(255)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    public string Slug { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Status { get; set; } = "Draft"; // Draft, Published, Archived

    public DateTime? PublishedAt { get; set; }

    public ICollection<LandingPageSection> Sections { get; set; } = new List<LandingPageSection>();
}
