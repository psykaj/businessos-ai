using backend.Common;
using backend.Entities;
using backend.Modules.QRCode.Enums;

namespace backend.Modules.QRCode.Models;

public class QRCode : BaseEntity
{
    public Guid OrganizationId { get; set; }
    public Organization? Organization { get; set; }

    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    
    public QRType QRType { get; set; }
    public string OriginalValue { get; set; } = string.Empty;
    public string ShortCode { get; set; } = string.Empty;
    public string? QrImageUrl { get; set; }
    
    // Using string for Status to keep it simple, or an Enum if preferred. Assuming string for flexibility.
    public string Status { get; set; } = "Active"; 
    public string? Folder { get; set; }
    
    // Store tags as a comma-separated string or a JSON array depending on DB support, PostgreSQL supports array so we can use List<string> or string[]
    public List<string> Tags { get; set; } = new();

    public string ForegroundColor { get; set; } = "#000000";
    public string BackgroundColor { get; set; } = "#FFFFFF";
    public string? LogoUrl { get; set; }
    
    public int Size { get; set; } = 256;
    public int Margin { get; set; } = 1;
    
    // QRCoder error correction level mapping: L, M, Q, H
    public string ErrorCorrectionLevel { get; set; } = "M";
    
    public bool PasswordProtected { get; set; }
    public string? PasswordHash { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public int ScanCount { get; set; }
}
