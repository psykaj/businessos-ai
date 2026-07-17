using backend.Modules.QRCode.Enums;

namespace backend.Modules.QRCode.DTOs;

public class CreateQRCodeDto
{
    public Guid OrganizationId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public QRType QRType { get; set; }
    public string OriginalValue { get; set; } = string.Empty;
    public string? Folder { get; set; }
    public List<string> Tags { get; set; } = new();
    
    // Customization
    public string ForegroundColor { get; set; } = "#000000";
    public string BackgroundColor { get; set; } = "#FFFFFF";
    public string? LogoUrl { get; set; }
    public int Size { get; set; } = 256;
    public int Margin { get; set; } = 1;
    public string ErrorCorrectionLevel { get; set; } = "M"; // L, M, Q, H
    
    public string? Password { get; set; }
    public DateTime? ExpirationDate { get; set; }
}
