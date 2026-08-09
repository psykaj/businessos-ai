using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using backend.Modules.BusinessIntelligence.DTOs;
using backend.Modules.BusinessIntelligence.Entities;
using backend.Modules.BusinessIntelligence.Interfaces;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace backend.Modules.BusinessIntelligence.Services;

public class ProactiveAlertService : IProactiveAlertService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ProactiveAlertService> _logger;

    public ProactiveAlertService(ApplicationDbContext context, ILogger<ProactiveAlertService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<ProactiveAlertDto>> GetAlertsAsync(Guid organizationId, CancellationToken cancellationToken = default)
    {
        var alerts = await _context.ProactiveAlerts
            .Where(a => a.OrganizationId == organizationId)
            .OrderByDescending(a => a.CreatedAt)
            .Take(50)
            .ToListAsync(cancellationToken);

        return alerts.Select(MapToDto).ToList();
    }

    public async Task<List<ProactiveAlertDto>> GetUnreadAlertsAsync(Guid organizationId, CancellationToken cancellationToken = default)
    {
        var alerts = await _context.ProactiveAlerts
            .Where(a => a.OrganizationId == organizationId && a.Status == AlertStatus.Unread)
            .OrderByDescending(a => a.Severity)
            .ThenByDescending(a => a.CreatedAt)
            .ToListAsync(cancellationToken);

        return alerts.Select(MapToDto).ToList();
    }

    public async Task<List<ProactiveAlertDto>> GetActionRequiredAlertsAsync(Guid organizationId, CancellationToken cancellationToken = default)
    {
        var alerts = await _context.ProactiveAlerts
            .Where(a => a.OrganizationId == organizationId && a.Status == AlertStatus.ActionRequired)
            .OrderByDescending(a => a.Severity)
            .ThenByDescending(a => a.CreatedAt)
            .ToListAsync(cancellationToken);

        return alerts.Select(MapToDto).ToList();
    }

    public async Task MarkAsReadAsync(Guid alertId, Guid organizationId, CancellationToken cancellationToken = default)
    {
        var alert = await _context.ProactiveAlerts
            .FirstOrDefaultAsync(a => a.Id == alertId && a.OrganizationId == organizationId, cancellationToken);

        if (alert != null && alert.Status == AlertStatus.Unread)
        {
            alert.Status = AlertStatus.Read;
            alert.ReadAt = DateTime.UtcNow;
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task DismissAsync(Guid alertId, Guid organizationId, CancellationToken cancellationToken = default)
    {
        var alert = await _context.ProactiveAlerts
            .FirstOrDefaultAsync(a => a.Id == alertId && a.OrganizationId == organizationId, cancellationToken);

        if (alert != null && alert.Status != AlertStatus.Dismissed && alert.Status != AlertStatus.Resolved)
        {
            alert.Status = AlertStatus.Dismissed;
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task ResolveAsync(Guid alertId, Guid organizationId, CancellationToken cancellationToken = default)
    {
        var alert = await _context.ProactiveAlerts
            .FirstOrDefaultAsync(a => a.Id == alertId && a.OrganizationId == organizationId, cancellationToken);

        if (alert != null && alert.Status != AlertStatus.Resolved)
        {
            alert.Status = AlertStatus.Resolved;
            alert.ResolvedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<ProactiveAlert?> CreateAlertAsync(ProactiveAlert alert, CancellationToken cancellationToken = default)
    {
        // Deduplication check
        if (!string.IsNullOrEmpty(alert.DeduplicationKey))
        {
            var existingAlert = await _context.ProactiveAlerts
                .Where(a => a.OrganizationId == alert.OrganizationId && 
                            a.DeduplicationKey == alert.DeduplicationKey &&
                            (a.Status == AlertStatus.Unread || a.Status == AlertStatus.Read || a.Status == AlertStatus.ActionRequired))
                .FirstOrDefaultAsync(cancellationToken);

            if (existingAlert != null)
            {
                _logger.LogInformation("Alert with deduplication key {Key} already exists. Skipping.", alert.DeduplicationKey);
                return null;
            }
        }

        _context.ProactiveAlerts.Add(alert);
        await _context.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("Created proactive alert {AlertId} for organization {OrgId}", alert.Id, alert.OrganizationId);
        
        return alert;
    }

    private static ProactiveAlertDto MapToDto(ProactiveAlert alert)
    {
        return new ProactiveAlertDto
        {
            Id = alert.Id,
            OrganizationId = alert.OrganizationId,
            Type = alert.Type,
            Severity = alert.Severity.ToString(),
            Title = alert.Title,
            Description = alert.Description,
            SourceModule = alert.SourceModule,
            SourceEntityId = alert.SourceEntityId,
            RecommendedAction = alert.RecommendedAction,
            Status = alert.Status.ToString(),
            ReadAt = alert.ReadAt,
            ResolvedAt = alert.ResolvedAt,
            CreatedAt = alert.CreatedAt
        };
    }
}
