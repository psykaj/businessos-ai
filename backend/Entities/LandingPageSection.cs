using backend.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Entities;

public class LandingPageSection : BaseEntity
{
    [Required]
    public Guid LandingPageId { get; set; }
    
    [ForeignKey("LandingPageId")]
    public LandingPage? LandingPage { get; set; }

    [Required]
    [MaxLength(50)]
    public string SectionType { get; set; } = string.Empty; // Hero, Features, Pricing, etc.

    public int SortOrder { get; set; }

    [Required]
    public string ContentJson { get; set; } = "{}";
}
