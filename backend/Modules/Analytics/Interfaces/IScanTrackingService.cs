using backend.Modules.Analytics.Models;
using backend.Modules.QRCode.Models;
using Microsoft.AspNetCore.Http;

namespace backend.Modules.Analytics.Interfaces;

public interface IScanTrackingService
{
    Task TrackScanAsync(QRCode.Models.QRCode qrCode, HttpContext httpContext);
}
