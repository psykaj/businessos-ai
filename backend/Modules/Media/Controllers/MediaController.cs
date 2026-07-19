using backend.Exceptions;
using backend.Modules.Media.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace backend.Modules.Media.Controllers;

[ApiController]
[Route("api/media")]
[Authorize]
public class MediaController : ControllerBase
{
    private readonly IMediaService _mediaService;

    public MediaController(IMediaService mediaService)
    {
        _mediaService = mediaService;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadMedia(IFormFile file, [FromQuery] string folder = "general")
    {
        var orgIdClaim = User.FindFirst("organizationId")?.Value;
        if (string.IsNullOrEmpty(orgIdClaim) || !Guid.TryParse(orgIdClaim, out var orgId))
            throw new UnauthorizedAccessException("Organization context is missing.");

        var url = await _mediaService.UploadImageAsync(file, folder, orgId);

        return Ok(new { Url = url });
    }
}
