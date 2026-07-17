using backend.Common;
using backend.Controllers;
using backend.Modules.QRCode.DTOs;
using backend.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.QRCode.Controllers;

[ApiController]
[Route("api/public/qrcodes")]
[AllowAnonymous]
[Produces("application/json")]
public class PublicQRCodesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public PublicQRCodesController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("{shortCode}")]
    [ProducesResponseType(typeof(ApiResponse<PublicQRCodeDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByShortCode(string shortCode, CancellationToken cancellationToken)
    {
        var qrCode = await _context.Set<Models.QRCode>()
            .AsNoTracking()
            .FirstOrDefaultAsync(q => q.ShortCode == shortCode && q.Status == "Active", cancellationToken);

        if (qrCode == null)
        {
            return NotFound(ApiResponse<object>.Fail(["QR Code not found or inactive."]));
        }

        var dto = new PublicQRCodeDto
        {
            Id = qrCode.Id,
            Name = qrCode.Name,
            Description = qrCode.Description,
            PasswordProtected = qrCode.PasswordProtected,
            OriginalValue = qrCode.PasswordProtected ? null : qrCode.OriginalValue // Hide if protected
        };

        return Ok(ApiResponse<PublicQRCodeDto>.Ok(dto));
    }

    [HttpPost("{shortCode}/verify")]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> VerifyPassword(string shortCode, [FromBody] VerifyPasswordDto dto, CancellationToken cancellationToken)
    {
        var qrCode = await _context.Set<Models.QRCode>()
            .FirstOrDefaultAsync(q => q.ShortCode == shortCode && q.Status == "Active", cancellationToken);

        if (qrCode == null)
        {
            return NotFound(ApiResponse<object>.Fail(["QR Code not found or inactive."]));
        }

        if (qrCode.PasswordProtected)
        {
            if (string.IsNullOrWhiteSpace(dto.Password) || !BCrypt.Net.BCrypt.Verify(dto.Password, qrCode.PasswordHash))
            {
                return Unauthorized(ApiResponse<object>.Fail(["Invalid password."]));
            }
        }

        // Log scan
        qrCode.ScanCount++;
        await _context.SaveChangesAsync(cancellationToken);

        return Ok(ApiResponse<string>.Ok(qrCode.OriginalValue));
    }
}

public class PublicQRCodeDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool PasswordProtected { get; set; }
    public string? OriginalValue { get; set; }
}

public class VerifyPasswordDto
{
    public string? Password { get; set; }
}
