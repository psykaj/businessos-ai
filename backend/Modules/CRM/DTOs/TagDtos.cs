namespace backend.Modules.CRM.DTOs;

public class CreateTagDto
{
    public string Name { get; set; } = string.Empty;
    public string Color { get; set; } = "#cccccc";
}

public class UpdateTagDto : CreateTagDto
{
}

public class TagResponseDto : CreateTagDto
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
