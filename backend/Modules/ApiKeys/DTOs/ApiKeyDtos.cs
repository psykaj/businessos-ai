namespace backend.Modules.ApiKeys.DTOs;

public class ApiKeyDto
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime? LastUsedAt { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class CreateApiKeyDto
{
    public string Name { get; set; } = string.Empty;
}

public class ApiKeyResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Key { get; set; } = string.Empty; // Only returned once
}
