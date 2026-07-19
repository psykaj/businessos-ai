using backend.Common;
using backend.Controllers;
using backend.Modules.QRCode.DTOs;
using backend.Modules.QRCode.Enums;
using backend.Modules.QRCode.Interfaces;
using backend.Persistence;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace backend.Modules.QRCode.Controllers;

[ApiController]
[Route("api/qrcodes")]
[Authorize]
[Produces("application/json")]
public class QRCodesController : ControllerBase
{
    private readonly IQRCodeService _qrCodeService;
    private readonly IValidator<CreateQRCodeDto> _createValidator;
    private readonly IValidator<UpdateQRCodeDto> _updateValidator;
    private readonly ApplicationDbContext _context;

    public QRCodesController(
        IQRCodeService qrCodeService,
        IValidator<CreateQRCodeDto> createValidator,
        IValidator<UpdateQRCodeDto> updateValidator,
        ApplicationDbContext context)
    {
        _qrCodeService = qrCodeService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _context = context;
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<QRCodeDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateQRCodeDto dto, CancellationToken cancellationToken)
    {
        var orgId = await GetOrganizationIdAsync(cancellationToken);
        if (orgId == Guid.Empty)
            return Unauthorized(ApiResponse<object>.Fail(["User not associated with an organization."]));

        dto.OrganizationId = orgId;

        var validation = await _createValidator.ValidateAsync(dto, cancellationToken);
        if (!validation.IsValid)
            return BadRequest(ApiResponse<object>.Fail(validation.Errors.Select(e => e.ErrorMessage)));

        var result = await _qrCodeService.CreateAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, ApiResponse<QRCodeDto>.Ok(result));
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<QRCodeDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? search,
        [FromQuery] string? folder,
        [FromQuery] string? status,
        [FromQuery] QRType? type,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var orgId = await GetOrganizationIdAsync(cancellationToken);
        if (orgId == Guid.Empty)
            return Unauthorized(ApiResponse<object>.Fail(["User not associated with an organization."]));

        var result = await _qrCodeService.SearchAsync(orgId, search, folder, status, type, pageNumber, pageSize, cancellationToken);
        return Ok(ApiResponse<PagedResult<QRCodeDto>>.Ok(result));
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<QRCodeDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var orgId = await GetOrganizationIdAsync(cancellationToken);
        if (orgId == Guid.Empty)
            return Unauthorized(ApiResponse<object>.Fail(["User not associated with an organization."]));

        try
        {
            var result = await _qrCodeService.GetByIdAsync(id, orgId, cancellationToken);
            return Ok(ApiResponse<QRCodeDto>.Ok(result));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ApiResponse<object>.Fail([ex.Message]));
        }
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<QRCodeDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateQRCodeDto dto, CancellationToken cancellationToken)
    {
        var orgId = await GetOrganizationIdAsync(cancellationToken);
        if (orgId == Guid.Empty)
            return Unauthorized(ApiResponse<object>.Fail(["User not associated with an organization."]));

        var validation = await _updateValidator.ValidateAsync(dto, cancellationToken);
        if (!validation.IsValid)
            return BadRequest(ApiResponse<object>.Fail(validation.Errors.Select(e => e.ErrorMessage)));

        try
        {
            var result = await _qrCodeService.UpdateAsync(id, orgId, dto, cancellationToken);
            return Ok(ApiResponse<QRCodeDto>.Ok(result));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ApiResponse<object>.Fail([ex.Message]));
        }
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var orgId = await GetOrganizationIdAsync(cancellationToken);
        if (orgId == Guid.Empty)
            return Unauthorized(ApiResponse<object>.Fail(["User not associated with an organization."]));

        try
        {
            await _qrCodeService.DeleteAsync(id, orgId, cancellationToken);
            return Ok(ApiResponse<object>.Ok(null, "QR Code deleted successfully."));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ApiResponse<object>.Fail([ex.Message]));
        }
    }

    [HttpGet("{id:guid}/image")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetImage(Guid id, [FromQuery] string format = "png", CancellationToken cancellationToken = default)
    {
        try
        {
            var imageBytes = await _qrCodeService.GenerateImageAsync(id, Guid.Empty, format, cancellationToken);
            if (format.ToLower() == "svg")
                return File(imageBytes, "image/svg+xml");
            else
                return File(imageBytes, "image/png");
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ApiResponse<object>.Fail([ex.Message]));
        }
    }

    private Guid GetUserIdFromClaims()
    {
        var subClaim = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                    ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        return Guid.TryParse(subClaim, out var userId) ? userId : Guid.Empty;
    }

    private async Task<Guid> GetOrganizationIdAsync(CancellationToken cancellationToken)
    {
        var userId = GetUserIdFromClaims();
        if (userId == Guid.Empty) return Guid.Empty;

        // Optionally, check if OrganizationId is in claims directly
        var orgClaim = User.FindFirst("organizationId")?.Value;
        if (Guid.TryParse(orgClaim, out var orgId))
            return orgId;

        // Fallback to database
        var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
        return user?.OrganizationId ?? Guid.Empty;
    }
}
