using backend.Entities;
using backend.Interfaces;
using backend.Modules.AuditLogs.DTOs;
using backend.Modules.AuditLogs.Interfaces;
using backend.Modules.AuditLogs.Repositories;

namespace backend.Modules.AuditLogs.Services;

public class AuditLogService : IAuditLogService
{
    private readonly IAuditLogRepository _auditLogRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AuditLogService(IAuditLogRepository auditLogRepository, IUnitOfWork unitOfWork)
    {
        _auditLogRepository = auditLogRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task LogActionAsync(Guid organizationId, Guid? userId, string action, string module, string description, string? ipAddress, string? userAgent, CancellationToken cancellationToken = default)
    {
        var log = new AuditLog
        {
            OrganizationId = organizationId,
            UserId = userId,
            Action = action,
            Module = module,
            Description = description,
            IPAddress = ipAddress,
            UserAgent = userAgent
        };

        await _auditLogRepository.AddAsync(log, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);
    }

    public async Task<PagedResult<AuditLogDto>> GetAuditLogsAsync(Guid organizationId, int page, int pageSize, string? module, string? action, CancellationToken cancellationToken = default)
    {
        var (items, totalCount) = await _auditLogRepository.GetPagedAsync(organizationId, page, pageSize, module, action, cancellationToken);

        var dtos = items.Select(a => new AuditLogDto
        {
            Id = a.Id,
            OrganizationId = a.OrganizationId,
            UserId = a.UserId,
            UserFullName = a.User?.FullName ?? string.Empty,
            UserEmail = a.User?.Email ?? string.Empty,
            Action = a.Action,
            Module = a.Module,
            Description = a.Description,
            IPAddress = a.IPAddress,
            UserAgent = a.UserAgent,
            CreatedAt = a.CreatedAt
        }).ToList();

        return new PagedResult<AuditLogDto>
        {
            Items = dtos,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }
}
