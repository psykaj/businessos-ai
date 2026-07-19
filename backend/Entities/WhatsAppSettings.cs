using System.ComponentModel.DataAnnotations;
using backend.Common;

namespace backend.Entities;

public class WhatsAppSettings : BaseEntity
{
    public Guid OrganizationId { get; set; }
    public Organization? Organization { get; set; }

    [Required]
    public string PhoneNumberId { get; set; } = string.Empty;

    [Required]
    public string BusinessAccountId { get; set; } = string.Empty;

    [Required]
    public string AccessToken { get; set; } = string.Empty;

    public string WebhookVerifyToken { get; set; } = string.Empty;
}
