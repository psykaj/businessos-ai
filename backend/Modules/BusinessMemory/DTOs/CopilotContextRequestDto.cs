namespace backend.Modules.BusinessMemory.DTOs;

public class CopilotContextRequestDto
{
    public string Question { get; set; } = string.Empty;
    public string? EntityType { get; set; }
    public string? EntityId { get; set; }
}
