using backend.Modules.Documents.Approvals.DTOs;
using backend.Modules.Documents.Approvals.Interfaces;
using backend.Modules.Documents.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.Documents.Approvals.Controllers;

[Route("api/approvals")]
public class ApprovalsController : BaseDocumentController
{
    private readonly IApprovalService _approvalService;

    public ApprovalsController(IApprovalService approvalService)
    {
        _approvalService = approvalService;
    }

    [HttpPost]
    public async Task<ActionResult<ApprovalRequestDto>> Create([FromBody] CreateApprovalRequestDto dto)
    {
        var request = await _approvalService.CreateAsync(
            GetOrganizationId(),
            GetUserId(),
            GetUserName(),
            dto
        );
        return Ok(request);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApprovalRequestDto>> GetById(Guid id)
    {
        var request = await _approvalService.GetByIdAsync(GetOrganizationId(), id);
        return Ok(request);
    }

    [HttpGet("document/{documentId:guid}")]
    public async Task<ActionResult<List<ApprovalRequestDto>>> GetByDocument(Guid documentId)
    {
        var requests = await _approvalService.GetByDocumentIdAsync(GetOrganizationId(), documentId);
        return Ok(requests);
    }

    [HttpGet("pending")]
    public async Task<ActionResult<List<ApprovalRequestDto>>> GetPending()
    {
        var requests = await _approvalService.GetPendingForUserAsync(GetOrganizationId(), GetUserId(), GetUserEmail());
        return Ok(requests);
    }

    [HttpPost("{id:guid}/approve")]
    public async Task<ActionResult<ApprovalRequestDto>> Approve(Guid id, [FromBody] ApproveRejectStepDto dto)
    {
        var updated = await _approvalService.ApproveStepAsync(GetOrganizationId(), id, GetUserId(), GetUserEmail(), dto);
        return Ok(updated);
    }

    [HttpPost("{id:guid}/reject")]
    public async Task<ActionResult<ApprovalRequestDto>> Reject(Guid id, [FromBody] ApproveRejectStepDto dto)
    {
        var updated = await _approvalService.RejectStepAsync(GetOrganizationId(), id, GetUserId(), GetUserEmail(), dto);
        return Ok(updated);
    }

    [HttpPost("{id:guid}/escalate")]
    public async Task<ActionResult<ApprovalRequestDto>> Escalate(Guid id, [FromBody] EscalateApprovalDto dto)
    {
        var updated = await _approvalService.EscalateAsync(GetOrganizationId(), id, dto);
        return Ok(updated);
    }

    [HttpPost("check-overdue")]
    public async Task<IActionResult> CheckOverdue()
    {
        await _approvalService.ProcessOverdueEscalationsAsync(GetOrganizationId());
        return Ok(new { Message = "Overdue approval escalations processed." });
    }
}
