namespace backend.Modules.SEO.DTOs;

public class SEODto
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    public string? Keywords { get; set; }
    public string? CanonicalUrl { get; set; }
    public string? OpenGraphTitle { get; set; }
    public string? OpenGraphDescription { get; set; }
    public string? OpenGraphImage { get; set; }
    public string TwitterCard { get; set; } = string.Empty;
    public string Robots { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class UpdateSEODto
{
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    public string? Keywords { get; set; }
    public string? CanonicalUrl { get; set; }
    public string? OpenGraphTitle { get; set; }
    public string? OpenGraphDescription { get; set; }
    public string? OpenGraphImage { get; set; }
    public string? TwitterCard { get; set; }
    public string? Robots { get; set; }
}
