using backend.Modules.QRCode.Enums;

namespace backend.Modules.QRCode.DTOs;

public class QRCodeDto
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string QRType { get; set; } = string.Empty;
    public string OriginalValue { get; set; } = string.Empty;
    public string ShortCode { get; set; } = string.Empty;
    public string? QrImageUrl { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? Folder { get; set; }
    public List<string> Tags { get; set; } = new();
    public string ForegroundColor { get; set; } = string.Empty;
    public string BackgroundColor { get; set; } = string.Empty;
    public string? LogoUrl { get; set; }
    public int Size { get; set; }
    public int Margin { get; set; }
    public string ErrorCorrectionLevel { get; set; } = string.Empty;
    public bool PasswordProtected { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public int ScanCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
