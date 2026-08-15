using System;
using System.Threading;
using System.Threading.Tasks;
using backend.Modules.DailyOperatingLoop.DTOs;
using backend.Modules.DailyOperatingLoop.Entities;
using backend.Modules.DailyOperatingLoop.Interfaces;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.DailyOperatingLoop.Services;

public class DailyPriorityService : IDailyPriorityService
{
    private readonly ApplicationDbContext _dbContext;

    public DailyPriorityService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<DailyPriorityDto?> GetPriorityAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var priority = await _dbContext.DailyPriorities
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
            
        return priority != null ? MapToDto(priority) : null;
    }

    public async Task<bool> CompletePriorityAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var priority = await _dbContext.DailyPriorities
            .Include(p => p.Briefing)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

        if (priority == null) return false;

        priority.Status = PriorityStatus.Completed;
        priority.CompletedAt = DateTime.UtcNow;

        if (priority.Briefing != null)
        {
            priority.Briefing.CompletedPriorityCount++;
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DismissPriorityAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var priority = await _dbContext.DailyPriorities
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

        if (priority == null) return false;

        priority.Status = PriorityStatus.Dismissed;

        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> SnoozePriorityAsync(Guid id, DateTime? snoozeUntil = null, CancellationToken cancellationToken = default)
    {
        var priority = await _dbContext.DailyPriorities
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

        if (priority == null) return false;

        priority.Status = PriorityStatus.Snoozed;
        
        if (snoozeUntil.HasValue)
        {
            priority.DueAt = snoozeUntil;
        }
        else
        {
            // Default snooze for 24 hours
            priority.DueAt = DateTime.UtcNow.AddDays(1);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
    
    private DailyPriorityDto MapToDto(DailyPriority p)
    {
        return new DailyPriorityDto
        {
            Id = p.Id,
            OrganizationId = p.OrganizationId,
            BriefingId = p.BriefingId,
            PriorityType = p.PriorityType.ToString(),
            Title = p.Title,
            Description = p.Description,
            Reason = p.Reason,
            Severity = p.Severity,
            PriorityScore = p.PriorityScore,
            RelatedEntityType = p.RelatedEntityType,
            RelatedEntityId = p.RelatedEntityId,
            RelatedGoalId = p.RelatedGoalId,
            RelatedKpiId = p.RelatedKpiId,
            SuggestedAction = p.SuggestedAction,
            ExpectedImpact = p.ExpectedImpact,
            ImpactType = p.ImpactType,
            Confidence = p.Confidence,
            Status = p.Status.ToString(),
            DueAt = p.DueAt,
            CompletedAt = p.CompletedAt
        };
    }
}
