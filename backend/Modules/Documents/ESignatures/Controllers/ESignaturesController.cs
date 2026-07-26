using backend.Modules.Documents.Controllers;
using backend.Modules.Documents.ESignatures.DTOs;
using backend.Modules.Documents.ESignatures.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.Documents.ESignatures.Controllers;

[Route("api/esignatures")]
public class ESignaturesController : BaseDocumentController
{
    private readonly IESignatureService _esignatureService;

    public ESignaturesController(IESignatureService esignatureService)
    {
        _esignatureService = esignatureService;
    }

    [HttpPost]
    public async Task<ActionResult<SignatureRequestDto>> Create([FromBody] CreateSignatureRequestDto dto)
    {
        var request = await _esignatureService.CreateAsync(
            GetOrganizationId(),
            GetUserId(),
            GetUserName(),
            dto
        );
        return Ok(request);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<SignatureRequestDto>> GetById(Guid id)
    {
        var request = await _esignatureService.GetByIdAsync(GetOrganizationId(), id);
        return Ok(request);
    }

    [HttpGet("document/{documentId:guid}")]
    public async Task<ActionResult<List<SignatureRequestDto>>> GetByDocument(Guid documentId)
    {
        var requests = await _esignatureService.GetByDocumentIdAsync(GetOrganizationId(), documentId);
        return Ok(requests);
    }

    [HttpPost("{id:guid}/cancel")]
    public async Task<ActionResult<SignatureRequestDto>> Cancel(Guid id, [FromQuery] string reason = "Cancelled by user")
    {
        var cancelled = await _esignatureService.CancelRequestAsync(GetOrganizationId(), id, reason);
        return Ok(cancelled);
    }

    [HttpGet("{id:guid}/audit-trail")]
    public async Task<ActionResult<SignatureAuditTrailDto>> GetAuditTrail(Guid id)
    {
        var auditTrail = await _esignatureService.GetAuditTrailAsync(GetOrganizationId(), id);
        return Ok(auditTrail);
    }

    [HttpPost("check-expirations")]
    public async Task<IActionResult> CheckExpirations()
    {
        await _esignatureService.CheckExpirationsAsync(GetOrganizationId());
        return Ok(new { Message = "Expired signature requests processed." });
    }

    [AllowAnonymous]
    [HttpGet("public/sign/{token}")]
    public async Task<ActionResult<SignatureRecipientDto>> GetRecipientByToken(string token)
    {
        var recipient = await _esignatureService.GetRecipientByTokenAsync(token);
        return Ok(recipient);
    }

    [AllowAnonymous]
    [HttpPost("public/sign/{token}")]
    public async Task<ActionResult<SignatureRecipientDto>> SubmitSignature(string token, [FromBody] SubmitSignatureDto dto)
    {
        var updatedRecipient = await _esignatureService.SubmitSignatureAsync(token, dto);
        return Ok(updatedRecipient);
    }
}
