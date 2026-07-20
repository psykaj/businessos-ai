using System.ComponentModel.DataAnnotations.Schema;
using backend.Common;
using backend.Modules.CRM.Entities;

namespace backend.Entities;

public class Contact : BaseEntity
{
    public Guid OrganizationId { get; set; }
    public Organization? Organization { get; set; }
    
    [Obsolete("Use FirstName and LastName instead.")]
    public string Name { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty; // Keeping this for backward compatibility if it's heavily used. Will also add Phone for CRM mapping if needed, or just use PhoneNumber. Let's use PhoneNumber for CRM.
    public string JobTitle { get; set; } = string.Empty;
    
    public Guid? CompanyId { get; set; }
    public Company? Company { get; set; }

    public string Address { get; set; } = string.Empty;
    public string SocialLinks { get; set; } = string.Empty; // Stored as JSON or comma-separated
    public string Tags { get; set; } = string.Empty;
}
