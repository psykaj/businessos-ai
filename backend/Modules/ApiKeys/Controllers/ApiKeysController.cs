using backend.Modules.ApiKeys.DTOs;
using backend.Modules.ApiKeys.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.ApiKeys.Controllers;

[ApiController]
[Route("api/v1/organizations/{organizationId}/apikeys")]
[Authorize]
public class ApiKeysController : ControllerBase
{
    private readonly IApiKeyService _apiKeyService;

    public ApiKeysController(IApiKeyService apiKeyService)
    {
        _apiKeyService = apiKeyService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ApiKeyDto>>> ListApiKeys(Guid organizationId, CancellationToken cancellationToken)
    {
        var result = await _apiKeyService.ListApiKeysAsync(organizationId, cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<ApiKeyResponseDto>> GenerateApiKey(Guid organizationId, [FromBody] CreateApiKeyDto dto, CancellationToken cancellationToken)
    {
        var result = await _apiKeyService.GenerateApiKeyAsync(organizationId, dto.Name, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{keyId}")]
    public async Task<IActionResult> RevokeApiKey(Guid organizationId, Guid keyId, CancellationToken cancellationToken)
    {
        await _apiKeyService.RevokeApiKeyAsync(organizationId, keyId, cancellationToken);
        return NoContent();
    }

    [HttpPost("{keyId}/rotate")]
    public async Task<ActionResult<ApiKeyResponseDto>> RotateApiKey(Guid organizationId, Guid keyId, CancellationToken cancellationToken)
    {
        var result = await _apiKeyService.RotateApiKeyAsync(organizationId, keyId, cancellationToken);
        return Ok(result);
    }
}
