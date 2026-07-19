using backend.Modules.AuditLogs.DTOs;
using backend.Modules.AuditLogs.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.AuditLogs.Controllers;

[ApiController]
[Route("api/v1/organizations/{organizationId}/auditlogs")]
[Authorize]
public class AuditLogsController : ControllerBase
{
    private readonly IAuditLogService _auditLogService;

    public AuditLogsController(IAuditLogService auditLogService)
    {
        _auditLogService = auditLogService;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<AuditLogDto>>> GetAuditLogs(
        Guid organizationId, 
        [FromQuery] int page = 1, 
        [FromQuery] int pageSize = 20, 
        [FromQuery] string? module = null, 
        [FromQuery] string? action = null,
        CancellationToken cancellationToken = default)
    {
        if (page < 1) page = 1;
        if (pageSize < 1 || pageSize > 100) pageSize = 20;

        var result = await _auditLogService.GetAuditLogsAsync(organizationId, page, pageSize, module, action, cancellationToken);
        return Ok(result);
    }
}
