using backend.Modules.Documents.Controllers;
using backend.Modules.Documents.Documents.DTOs;
using backend.Modules.Documents.Sharing.DTOs;
using backend.Modules.Documents.Sharing.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.Documents.Sharing.Controllers;

[Route("api/documents")]
public class SharedDocumentsController : BaseDocumentController
{
    private readonly ISharedDocumentService _sharedDocumentService;

    public SharedDocumentsController(ISharedDocumentService sharedDocumentService)
    {
        _sharedDocumentService = sharedDocumentService;
    }

    [HttpPost("share")]
    public async Task<ActionResult<SharedDocumentDto>> Share([FromBody] ShareDocumentDto dto)
    {
        var share = await _sharedDocumentService.ShareAsync(GetOrganizationId(), GetUserId(), dto);
        return Ok(share);
    }

    [HttpGet("{documentId:guid}/shares")]
    public async Task<ActionResult<List<SharedDocumentDto>>> GetShares(Guid documentId)
    {
        var shares = await _sharedDocumentService.GetByDocumentIdAsync(GetOrganizationId(), documentId);
        return Ok(shares);
    }

    [HttpDelete("shares/{shareId:guid}")]
    public async Task<IActionResult> RevokeShare(Guid shareId)
    {
        await _sharedDocumentService.RevokeAsync(GetOrganizationId(), shareId);
        return Ok(new { Message = "Document share revoked." });
    }

    [AllowAnonymous]
    [HttpPost("public/{token}")]
    public async Task<ActionResult<DocumentPreviewDto>> AccessPublicShare(string token, [FromBody] PublicAccessDto dto)
    {
        var preview = await _sharedDocumentService.AccessPublicShareAsync(token, dto);
        return Ok(preview);
    }
}
