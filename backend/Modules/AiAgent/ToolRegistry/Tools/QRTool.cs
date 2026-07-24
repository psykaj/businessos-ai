using backend.Modules.AiAgent.ToolRegistry.Interfaces;
using backend.Modules.AiAgent.ToolRegistry.Models;
using backend.Modules.QRCode.Enums;
using backend.Persistence;

namespace backend.Modules.AiAgent.ToolRegistry.Tools;

public class QRTool : ITool
{
    private readonly ApplicationDbContext _context;

    public QRTool(ApplicationDbContext context)
    {
        _context = context;
    }

    public string Name => "QRTool";
    public string Description => "QR Code Generation Tool: generate custom QR codes for URLs, contact cards, or landing pages.";
    public string Category => "Marketing";
    public string[] RequiredPermissions => new[] { "QRCode.Create" };
    public bool IsDestructive => false;
    public string ParametersSchema => "{\"type\":\"object\",\"properties\":{\"targetUrl\":{\"type\":\"string\"},\"name\":{\"type\":\"string\"}}}";

    public async Task<ToolResult> ExecuteAsync(
        ToolExecutionContext context,
        IDictionary<string, object> parameters,
        CancellationToken cancellationToken = default)
    {
        var targetUrl = parameters.TryGetValue("targetUrl", out var urlObj) ? urlObj?.ToString() : "https://businessos.ai";
        var qrName = parameters.TryGetValue("name", out var nameObj) ? nameObj?.ToString() : "AI Generated QR Code";

        var shortCode = Guid.NewGuid().ToString("N")[..8];

        var qrCode = new QRCode.Models.QRCode
        {
            Id = Guid.NewGuid(),
            OrganizationId = context.OrganizationId,
            Name = qrName ?? "AI QR Code",
            OriginalValue = targetUrl ?? "https://businessos.ai",
            ShortCode = shortCode,
            QRType = QRType.Website,
            Status = "Active",
            QrImageUrl = $"/api/qr/{shortCode}/image",
            Size = 256
        };

        await _context.QRCodes.AddAsync(qrCode, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return ToolResult.Ok($"QR Code '{qrCode.Name}' generated for {qrCode.OriginalValue}.", new { qrCode.Id, qrCode.Name, qrCode.ShortCode, qrCode.QrImageUrl });
    }
}
