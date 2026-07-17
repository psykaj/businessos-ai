using backend.Modules.Analytics.Interfaces;
using backend.Modules.Analytics.Models;
using backend.Modules.QRCode.Models;
using backend.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using UAParser;

namespace backend.Modules.Analytics.Services;

public class ScanTrackingService : IScanTrackingService
{
    private readonly ApplicationDbContext _dbContext;

    public ScanTrackingService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task TrackScanAsync(QRCode.Models.QRCode qrCode, HttpContext httpContext)
    {
        var request = httpContext.Request;
        
        var ipAddress = GetClientIpAddress(httpContext);
        var userAgentString = request.Headers["User-Agent"].ToString();
        var referrer = request.Headers["Referer"].ToString();
        var language = request.Headers["Accept-Language"].ToString();

        // Use Cloudflare header if available for country, else fallback
        var country = request.Headers["CF-IPCountry"].ToString();

        var uaParser = Parser.GetDefault();
        ClientInfo? clientInfo = null;
        if (!string.IsNullOrEmpty(userAgentString))
        {
             clientInfo = uaParser.Parse(userAgentString);
        }

        var utmSource = request.Query["utm_source"].ToString();
        var utmMedium = request.Query["utm_medium"].ToString();
        var utmCampaign = request.Query["utm_campaign"].ToString();
        var utmTerm = request.Query["utm_term"].ToString();
        var utmContent = request.Query["utm_content"].ToString();

        var scan = new QRScan
        {
            OrganizationId = qrCode.OrganizationId,
            QRCodeId = qrCode.Id,
            ScanDateTime = DateTime.UtcNow,
            IPAddress = ipAddress,
            UserAgent = userAgentString,
            DeviceType = GetDeviceType(clientInfo),
            Browser = clientInfo?.UA.Family,
            BrowserVersion = clientInfo?.UA.Major,
            OperatingSystem = clientInfo?.OS.Family,
            OperatingSystemVersion = clientInfo?.OS.Major,
            Country = !string.IsNullOrEmpty(country) ? country : "Unknown",
            Referrer = referrer,
            Language = language,
            UTMSource = utmSource,
            UTMMedium = utmMedium,
            UTMCampaign = utmCampaign,
            UTMTerm = utmTerm,
            UTMContent = utmContent
        };

        _dbContext.QRScans.Add(scan);
        
        // Update QR Code scan count
        qrCode.ScanCount += 1;
        _dbContext.QRCodes.Update(qrCode);

        await _dbContext.SaveChangesAsync();
    }

    private string? GetClientIpAddress(HttpContext context)
    {
        var forwardedHeader = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
        if (!string.IsNullOrEmpty(forwardedHeader))
        {
            return forwardedHeader.Split(',').FirstOrDefault()?.Trim();
        }

        return context.Connection.RemoteIpAddress?.ToString();
    }

    private string GetDeviceType(ClientInfo? clientInfo)
    {
        if (clientInfo == null) return "Unknown";
        
        if (clientInfo.Device.Family.ToLower().Contains("spider") || clientInfo.Device.Family.ToLower().Contains("bot"))
        {
            return "Bot";
        }
        
        var osFamily = clientInfo.OS.Family.ToLower();
        if (osFamily.Contains("ios") || osFamily.Contains("android") || osFamily.Contains("windows phone"))
        {
            return "Mobile";
        }

        return "Desktop";
    }
}
