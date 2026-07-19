using backend.Entities;
using backend.Interfaces;

namespace backend.Modules.AuditLogs.Repositories;

public interface IAuditLogRepository : IGenericRepository<AuditLog>
{
    Task<(IReadOnlyList<AuditLog> Items, int TotalCount)> GetPagedAsync(
        Guid organizationId, int page, int pageSize, string? module, string? action, CancellationToken cancellationToken = default);
}
