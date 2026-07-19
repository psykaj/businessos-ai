using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backend.Interfaces;

using backend.Common;

namespace backend.Entities;

public class Notification : BaseEntity
{
    [Required]
    public Guid OrganizationId { get; set; }
    
    [Required]
    public string UserId { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;
    
    [Required]
    public string Message { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(50)]
    public string Type { get; set; } = "Info"; // Info, Success, Warning, Error
    
    [Required]
    [MaxLength(20)]
    public string Status { get; set; } = "Unread"; // Unread, Read
    
    public DateTime? ReadAt { get; set; }
}
