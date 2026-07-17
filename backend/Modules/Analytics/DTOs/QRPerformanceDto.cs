namespace backend.Modules.Analytics.DTOs;

public class QRPerformanceDto
{
    public Guid QRCodeId { get; set; }
    public string QRCodeName { get; set; } = string.Empty;
    public int TotalScans { get; set; }
    public int UniqueScans { get; set; }
    public DateTime? LastScanDate { get; set; }
}
