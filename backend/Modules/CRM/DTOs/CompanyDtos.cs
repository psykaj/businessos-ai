namespace backend.Modules.CRM.DTOs;

public class CreateCompanyDto
{
    public string CompanyName { get; set; } = string.Empty;
    public string Industry { get; set; } = string.Empty;
    public string Website { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}

public class UpdateCompanyDto : CreateCompanyDto
{
}

public class CompanyResponseDto : CreateCompanyDto
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
