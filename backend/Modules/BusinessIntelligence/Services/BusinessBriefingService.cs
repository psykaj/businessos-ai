using System;
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

public class BusinessBriefingService : IBusinessBriefingService
{
    private readonly ApplicationDbContext _context;
    private readonly IBriefingGenerator _generator;
    private readonly ILogger<BusinessBriefingService> _logger;

    public BusinessBriefingService(
        ApplicationDbContext context,
        IBriefingGenerator generator,
        ILogger<BusinessBriefingService> logger)
    {
        _context = context;
        _generator = generator;
        _logger = logger;
    }

    public async Task<BusinessBriefingDto?> GetTodayBriefingAsync(Guid organizationId, CancellationToken cancellationToken = default)
    {
        var today = DateTime.UtcNow.Date;
        return await GetBriefingByDateAsync(organizationId, today, cancellationToken);
    }

    public async Task<BusinessBriefingDto?> GetBriefingByDateAsync(Guid organizationId, DateTime date, CancellationToken cancellationToken = default)
    {
        var targetDate = date.Date;
        
        var briefing = await _context.BusinessBriefings
            .Include(b => b.Items)
            .FirstOrDefaultAsync(b => b.OrganizationId == organizationId && b.Date == targetDate, cancellationToken);

        if (briefing == null)
            return null;

        return MapToDto(briefing);
    }

    public async Task<BusinessBriefingDto> GenerateBriefingAsync(Guid organizationId, DateTime date, bool forceRegeneration, CancellationToken cancellationToken = default)
    {
        var targetDate = date.Date;

        var existingBriefing = await _context.BusinessBriefings
            .Include(b => b.Items)
            .FirstOrDefaultAsync(b => b.OrganizationId == organizationId && b.Date == targetDate, cancellationToken);

        if (existingBriefing != null && !forceRegeneration)
        {
            return MapToDto(existingBriefing);
        }

        var newBriefing = await _generator.GenerateBriefingAsync(organizationId, targetDate, cancellationToken);

        if (existingBriefing != null)
        {
            // Remove old briefing
            _context.BriefingItems.RemoveRange(existingBriefing.Items);
            _context.BusinessBriefings.Remove(existingBriefing);
        }

        _context.BusinessBriefings.Add(newBriefing);
        await _context.SaveChangesAsync(cancellationToken);

        return MapToDto(newBriefing);
    }

    private static BusinessBriefingDto MapToDto(BusinessBriefing briefing)
    {
        return new BusinessBriefingDto
        {
            Id = briefing.Id,
            OrganizationId = briefing.OrganizationId,
            Date = briefing.Date,
            PeriodStart = briefing.PeriodStart,
            PeriodEnd = briefing.PeriodEnd,
            Summary = briefing.Summary,
            BusinessHealthScore = briefing.BusinessHealthScore,
            RevenueSummary = briefing.RevenueSummary,
            CustomerSummary = briefing.CustomerSummary,
            FinanceSummary = briefing.FinanceSummary,
            InventorySummary = briefing.InventorySummary,
            RiskCount = briefing.RiskCount,
            OpportunityCount = briefing.OpportunityCount,
            ActionCount = briefing.ActionCount,
            GeneratedAt = briefing.GeneratedAt,
            Status = briefing.Status.ToString(),
            Items = briefing.Items.Select(i => new BriefingItemDto
            {
                Id = i.Id,
                BriefingId = i.BriefingId,
                Type = i.Type.ToString(),
                Title = i.Title,
                Description = i.Description,
                Priority = i.Priority,
                Category = i.Category,
                SourceModule = i.SourceModule,
                SourceEntityId = i.SourceEntityId,
                ConfidenceScore = i.ConfidenceScore,
                ExpectedBusinessImpact = i.ExpectedBusinessImpact,
                RecommendedAction = i.RecommendedAction,
                IsRead = i.IsRead,
                IsDismissed = i.IsDismissed,
                CreatedAt = i.CreatedAt
            }).OrderByDescending(i => i.Priority).ToList()
        };
    }
}
