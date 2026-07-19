using backend.Modules.AuditLogs.DTOs;

namespace backend.Modules.AuditLogs.Interfaces;

public interface IAuditLogService
{
    Task LogActionAsync(Guid organizationId, Guid? userId, string action, string module, string description, string? ipAddress, string? userAgent, CancellationToken cancellationToken = default);
    Task<PagedResult<AuditLogDto>> GetAuditLogsAsync(Guid organizationId, int page, int pageSize, string? module, string? action, CancellationToken cancellationToken = default);
}
