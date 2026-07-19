namespace backend.Modules.CustomDomains.DTOs;

public class CustomDomainDto
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public string Domain { get; set; } = string.Empty;
    public string VerificationStatus { get; set; } = string.Empty;
    public string SSLStatus { get; set; } = string.Empty;
    public bool IsPrimary { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateCustomDomainDto
{
    public string Domain { get; set; } = string.Empty;
}
