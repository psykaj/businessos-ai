using backend.Modules.Analytics.Interfaces;
using backend.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.Analytics.Controllers;

[ApiController]
[Route("r")]
public class RedirectController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IScanTrackingService _scanTrackingService;

    public RedirectController(ApplicationDbContext dbContext, IScanTrackingService scanTrackingService)
    {
        _dbContext = dbContext;
        _scanTrackingService = scanTrackingService;
    }

    [HttpGet("{shortCode}")]
    public async Task<IActionResult> RedirectToOriginal(string shortCode)
    {
        var qrCode = await _dbContext.QRCodes
            .FirstOrDefaultAsync(q => q.ShortCode == shortCode);

        if (qrCode == null)
        {
            return NotFound("QR Code not found.");
        }

        if (qrCode.Status != "Active")
        {
            return BadRequest("QR Code is inactive.");
        }

        if (qrCode.ExpirationDate.HasValue && qrCode.ExpirationDate.Value < DateTime.UtcNow)
        {
            return BadRequest("QR Code has expired.");
        }

        if (qrCode.PasswordProtected)
        {
            // For an API redirect, typically we'd redirect to a frontend challenge page 
            // if we don't have the password in the query string or session.
            // Assuming we expect a frontend challenge, we can return a 401 or a redirect to a specific challenge URL.
            // Let's return a specific status that the frontend can intercept, or redirect to a predefined challenge path.
            // As per requirements: "Verify password if Password Protection is enabled. Handle: Invalid password"
            
            // For now, if it's password protected, and we're hitting the raw GET endpoint, 
            // it cannot supply a body password. It might be in the query.
            var password = Request.Query["pwd"].ToString();
            
            if (string.IsNullOrEmpty(password) || !BCrypt.Net.BCrypt.Verify(password, qrCode.PasswordHash))
            {
                return Unauthorized("Password is required or invalid to access this QR code.");
            }
        }

        // Track Scan
        await _scanTrackingService.TrackScanAsync(qrCode, HttpContext);

        // Redirect
        if (string.IsNullOrEmpty(qrCode.OriginalValue))
        {
             return BadRequest("Invalid destination.");
        }

        return Redirect(qrCode.OriginalValue);
    }
}
