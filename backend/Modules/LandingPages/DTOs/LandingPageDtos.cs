namespace backend.Modules.LandingPages.DTOs;

public class LandingPageDto
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime? PublishedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<LandingPageSectionDto> Sections { get; set; } = new();
}

public class LandingPageSectionDto
{
    public Guid Id { get; set; }
    public string SectionType { get; set; } = string.Empty;
    public int SortOrder { get; set; }
    public string ContentJson { get; set; } = "{}";
}

public class CreateLandingPageDto
{
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
}

public class UpdateLandingPageDto
{
    public string? Title { get; set; }
    public string? Slug { get; set; }
    public string? Status { get; set; }
    public List<UpdateLandingPageSectionDto>? Sections { get; set; }
}

public class UpdateLandingPageSectionDto
{
    public Guid? Id { get; set; } // If null, it's a new section
    public string SectionType { get; set; } = string.Empty;
    public int SortOrder { get; set; }
    public string ContentJson { get; set; } = "{}";
}
