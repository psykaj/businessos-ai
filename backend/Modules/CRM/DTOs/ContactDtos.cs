namespace backend.Modules.CRM.DTOs;

public class CreateContactDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string JobTitle { get; set; } = string.Empty;
    public Guid? CompanyId { get; set; }
    public string Address { get; set; } = string.Empty;
    public string SocialLinks { get; set; } = string.Empty;
    public string Tags { get; set; } = string.Empty;
}

public class UpdateContactDto : CreateContactDto
{
}

public class ContactResponseDto : CreateContactDto
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
