using backend.Modules.Documents.AuditLogs.DTOs;
using backend.Modules.Documents.AuditLogs.Interfaces;
using backend.Modules.Documents.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.Documents.AuditLogs.Controllers;

[Route("api/documents")]
public class DocumentAuditLogsController : BaseDocumentController
{
    private readonly IDocumentAuditLogService _auditLogService;

    public DocumentAuditLogsController(IDocumentAuditLogService auditLogService)
    {
        _auditLogService = auditLogService;
    }

    [HttpGet("{documentId:guid}/audit-logs")]
    public async Task<ActionResult<List<DocumentAuditEntryDto>>> GetDocumentAuditLogs(Guid documentId, [FromQuery] int limit = 100)
    {
        var logs = await _auditLogService.GetByDocumentIdAsync(GetOrganizationId(), documentId, limit);
        return Ok(logs);
    }

    [HttpGet("audit-logs")]
    public async Task<ActionResult> GetOrganizationAuditLogs(
        [FromQuery] string? entityType,
        [FromQuery] string? action,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var (items, totalCount) = await _auditLogService.GetPagedAsync(GetOrganizationId(), entityType, action, page, pageSize);
        return Ok(new { Items = items, TotalCount = totalCount, Page = page, PageSize = pageSize });
    }
}
