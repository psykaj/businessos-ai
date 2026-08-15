using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using backend.Modules.AI.Interfaces;
using backend.Modules.DailyOperatingLoop.DTOs;
using backend.Modules.DailyOperatingLoop.Entities;
using backend.Modules.DailyOperatingLoop.Interfaces;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace backend.Modules.DailyOperatingLoop.Services;

public class DailyBriefingService : IDailyBriefingService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IDailyPriorityScoringService _scoringService;
    private readonly IAIService _aiService;
    private readonly ILogger<DailyBriefingService> _logger;

    public DailyBriefingService(
        ApplicationDbContext dbContext,
        IDailyPriorityScoringService scoringService,
        IAIService aiService,
        ILogger<DailyBriefingService> logger)
    {
        _dbContext = dbContext;
        _scoringService = scoringService;
        _aiService = aiService;
        _logger = logger;
    }

    public async Task<DailyBusinessBriefingDto?> GetTodayBriefingAsync(Guid organizationId, CancellationToken cancellationToken = default)
    {
        var today = DateTime.UtcNow.Date;
        return await GetBriefingByDateAsync(organizationId, today, cancellationToken);
    }

    public async Task<DailyBusinessBriefingDto?> GetBriefingByDateAsync(Guid organizationId, DateTime date, CancellationToken cancellationToken = default)
    {
        var briefing = await _dbContext.DailyBusinessBriefings
            .Include(b => b.Priorities)
            .Where(b => b.OrganizationId == organizationId && b.BriefingDate.Date == date.Date)
            .OrderByDescending(b => b.GeneratedAt)
            .FirstOrDefaultAsync(cancellationToken);

        return briefing != null ? MapToDto(briefing) : null;
    }

    public async Task<DailyBusinessBriefingDto> GenerateBriefingAsync(Guid organizationId, DateTime date, bool forceRegeneration = false, CancellationToken cancellationToken = default)
    {
        if (!forceRegeneration)
        {
            var existing = await GetBriefingByDateAsync(organizationId, date, cancellationToken);
            if (existing != null && existing.ExpiresAt > DateTime.UtcNow)
            {
                return existing;
            }
        }

        var briefingId = Guid.NewGuid();
        var rawPriorities = new List<DailyPriority>();

        // 1. Load Goals & KPIs Status
        await LoadGoalAndKpiPriorities(organizationId, briefingId, rawPriorities, cancellationToken);

        // 2. Load Finance (Overdue Invoices)
        await LoadFinancePriorities(organizationId, briefingId, rawPriorities, cancellationToken);

        // 3. Load CRM (Customer Follow-ups)
        await LoadCustomerPriorities(organizationId, briefingId, rawPriorities, cancellationToken);

        // 4. Load Alerts
        await LoadAlertPriorities(organizationId, briefingId, rawPriorities, cancellationToken);

        // 5. Load Memory & Outcomes context (Enhancing existing priorities)
        await EnhanceWithMemoryAndOutcomes(organizationId, rawPriorities, cancellationToken);

        // 6. Score and Rank Priorities
        foreach (var p in rawPriorities)
        {
            _scoringService.ScorePriority(p);
        }

        var topPriorities = _scoringService.RankPriorities(rawPriorities, 5);
        
        // Count metrics
        int riskCount = topPriorities.Count(p => p.PriorityType == PriorityType.UrgentRisk || p.PriorityType == PriorityType.InventoryRisk || p.PriorityType == PriorityType.GoalRisk);
        int oppCount = topPriorities.Count(p => p.PriorityType == PriorityType.RevenueOpportunity || p.PriorityType == PriorityType.PositiveOpportunity);
        int priorityCount = topPriorities.Count;

        var briefing = new DailyBusinessBriefing
        {
            Id = briefingId,
            OrganizationId = organizationId,
            BriefingDate = date.Date,
            GeneratedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.Date.AddDays(1).AddTicks(-1),
            Status = "Active",
            PriorityCount = priorityCount,
            RiskCount = riskCount,
            OpportunityCount = oppCount,
            CompletedPriorityCount = 0,
            BusinessHealth = DetermineBusinessHealth(topPriorities),
            Priorities = topPriorities
        };

        // 7. AI Summary Generation
        await GenerateAiSummary(organizationId, briefing, topPriorities);

        // 8. Store and Return
        // Invalidate old briefings for today if forced
        if (forceRegeneration)
        {
            var oldBriefings = await _dbContext.DailyBusinessBriefings
                .Where(b => b.OrganizationId == organizationId && b.BriefingDate.Date == date.Date)
                .ToListAsync(cancellationToken);
            _dbContext.DailyBusinessBriefings.RemoveRange(oldBriefings);
        }

        _dbContext.DailyBusinessBriefings.Add(briefing);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return MapToDto(briefing);
    }

    public async Task<DailyBusinessBriefingDto> RefreshBriefingAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var briefing = await _dbContext.DailyBusinessBriefings
            .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
            
        if (briefing == null) throw new KeyNotFoundException("Briefing not found");

        return await GenerateBriefingAsync(briefing.OrganizationId, briefing.BriefingDate, true, cancellationToken);
    }

    private async Task LoadGoalAndKpiPriorities(Guid orgId, Guid briefingId, List<DailyPriority> priorities, CancellationToken token)
    {
        var atRiskGoals = await _dbContext.Set<backend.Modules.BusinessGoals.Entities.BusinessGoal>()
            .Where(g => g.OrganizationId == orgId && g.Status == "AtRisk" && g.IsActive)
            .ToListAsync(token);

        foreach (var goal in atRiskGoals)
        {
            priorities.Add(new DailyPriority
            {
                Id = Guid.NewGuid(),
                OrganizationId = orgId,
                BriefingId = briefingId,
                PriorityType = PriorityType.GoalRisk,
                Title = $"Goal at Risk: {goal.Name}",
                Description = $"Goal '{goal.Name}' is currently marked as At Risk. Progress is {goal.ProgressPercentage}%.",
                Reason = "Goal targets are falling behind schedule.",
                Severity = "High",
                RelatedEntityType = "BusinessGoal",
                RelatedEntityId = goal.Id.ToString(),
                RelatedGoalId = goal.Id,
                SuggestedAction = "Review goal and update action plan.",
                ExpectedImpact = "Goal recovery",
                ImpactType = "Goal",
                Confidence = 90m,
                Status = PriorityStatus.New,
                DueAt = DateTime.UtcNow.AddDays(1)
            });
        }
    }

    private async Task LoadFinancePriorities(Guid orgId, Guid briefingId, List<DailyPriority> priorities, CancellationToken token)
    {
        var overdueInvoices = await _dbContext.Set<backend.Modules.Invoices.Entities.FinanceInvoice>()
            .Where(i => i.OrganizationId == orgId && i.Status == "Overdue" && i.DueDate < DateTime.UtcNow)
            .OrderByDescending(i => i.TotalAmount)
            .Take(10)
            .ToListAsync(token);

        foreach (var invoice in overdueInvoices)
        {
            var daysOverdue = (int)(DateTime.UtcNow - invoice.DueDate).TotalDays;
            priorities.Add(new DailyPriority
            {
                Id = Guid.NewGuid(),
                OrganizationId = orgId,
                BriefingId = briefingId,
                PriorityType = PriorityType.Collections,
                Title = $"Overdue Invoice: {invoice.InvoiceNumber}",
                Description = $"Invoice {invoice.InvoiceNumber} is {daysOverdue} days overdue.",
                Reason = $"Missing payment of {invoice.TotalAmount} {invoice.Currency}.",
                Severity = daysOverdue > 30 ? "High" : "Medium",
                RelatedEntityType = "Invoice",
                RelatedEntityId = invoice.Id.ToString(),
                SuggestedAction = "Send payment reminder to customer.",
                ExpectedImpact = "Cash flow recovery",
                ImpactType = "Revenue",
                Confidence = 95m,
                Status = PriorityStatus.New,
                DueAt = DateTime.UtcNow.AddDays(1)
            });
        }
    }

    private async Task LoadCustomerPriorities(Guid orgId, Guid briefingId, List<DailyPriority> priorities, CancellationToken token)
    {
        // Example: Follow up with high-value customers
        var customers = await _dbContext.Set<backend.Modules.CustomerAnalytics.Entities.CustomerPerformance>()
            .Where(c => c.OrganizationId == orgId && c.LifetimeValue > 50000 && c.LastPurchaseDate < DateTime.UtcNow.AddDays(-60))
            .Take(5)
            .ToListAsync(token);

        foreach (var customer in customers)
        {
            priorities.Add(new DailyPriority
            {
                Id = Guid.NewGuid(),
                OrganizationId = orgId,
                BriefingId = briefingId,
                PriorityType = PriorityType.CustomerFollowUp,
                Title = $"Follow up with High-Value Customer: {customer.CustomerName}",
                Description = $"Customer {customer.CustomerName} hasn't purchased in over 60 days.",
                Reason = "Re-engage valuable customer to prevent churn.",
                Severity = "Medium",
                RelatedEntityType = "Customer",
                RelatedEntityId = customer.Id.ToString(),
                SuggestedAction = "Contact customer with a special offer or check-in.",
                ExpectedImpact = "Repeat purchase",
                ImpactType = "Revenue",
                Confidence = 80m,
                Status = PriorityStatus.New,
                DueAt = DateTime.UtcNow.AddDays(3)
            });
        }
    }

    private async Task LoadAlertPriorities(Guid orgId, Guid briefingId, List<DailyPriority> priorities, CancellationToken token)
    {
        // Load recent high-severity proactive alerts from Day 29
        var alerts = await _dbContext.Set<backend.Modules.BusinessIntelligence.Entities.ProactiveAlert>()
            .Where(a => a.OrganizationId == orgId && a.Status != backend.Modules.BusinessIntelligence.Entities.AlertStatus.Resolved && a.Status != backend.Modules.BusinessIntelligence.Entities.AlertStatus.Dismissed && (a.Severity == backend.Modules.BusinessIntelligence.Entities.AlertSeverity.Critical || a.Severity == backend.Modules.BusinessIntelligence.Entities.AlertSeverity.High))
            .OrderByDescending(a => a.CreatedAt)
            .Take(3)
            .ToListAsync(token);

        foreach (var alert in alerts)
        {
            priorities.Add(new DailyPriority
            {
                Id = Guid.NewGuid(),
                OrganizationId = orgId,
                BriefingId = briefingId,
                PriorityType = PriorityType.UrgentRisk,
                Title = alert.Title,
                Description = alert.Description,
                Reason = "System identified urgent risk.",
                Severity = alert.Severity.ToString(),
                RelatedEntityType = alert.SourceModule,
                RelatedEntityId = alert.SourceEntityId,
                SuggestedAction = alert.RecommendedAction,
                ExpectedImpact = "Risk Mitigation",
                ImpactType = "Risk Mitigation",
                Confidence = 90m, // High confidence for system alerts
                Status = PriorityStatus.New,
                DueAt = DateTime.UtcNow.AddHours(24)
            });
        }
    }

    private async Task EnhanceWithMemoryAndOutcomes(Guid orgId, List<DailyPriority> priorities, CancellationToken token)
    {
        // Example: Retrieve outcomes related to "Collections"
        // In a real scenario, this would use semantic search. We'll do a simple match for now.
        var collectionsOutcome = await _dbContext.Set<backend.Modules.Outcomes.Entities.BusinessOutcome>()
            .Where(o => o.BusinessId == orgId && o.OutcomeType == backend.Modules.Outcomes.Entities.OutcomeType.RevenueRecovered && o.RevenueImpact > 0)
            .OrderByDescending(o => o.OccurredAt)
            .FirstOrDefaultAsync(token);

        if (collectionsOutcome != null)
        {
            var collectionPriorities = priorities.Where(p => p.PriorityType == PriorityType.Collections);
            foreach (var p in collectionPriorities)
            {
                p.Description += $" Note: Previous collections actions successfully recovered {collectionsOutcome.RevenueImpact}.";
                p.Confidence += 5m; // Boost confidence
            }
        }
    }

    private BusinessHealthState DetermineBusinessHealth(List<DailyPriority> priorities)
    {
        int criticalCount = priorities.Count(p => p.Severity == "Critical" || p.Severity == "High");
        
        if (criticalCount >= 3) return BusinessHealthState.Critical;
        if (criticalCount > 0) return BusinessHealthState.AtRisk;
        if (priorities.Any(p => p.Severity == "Medium")) return BusinessHealthState.NeedsAttention;
        
        return BusinessHealthState.Healthy;
    }

    private async Task GenerateAiSummary(Guid orgId, DailyBusinessBriefing briefing, List<DailyPriority> topPriorities)
    {
        if (!topPriorities.Any())
        {
            briefing.Summary = "No urgent priorities today. Business is running smoothly.";
            return;
        }

        var prompt = "You are a CEO's top business assistant. Summarize today's top business priorities into a single concise paragraph. Focus on what matters, why, and what to do.";
        var prioritiesJson = JsonSerializer.Serialize(topPriorities.Select(p => new { p.Title, p.Reason, p.SuggestedAction, p.ExpectedImpact }));

        try
        {
            var aiResponse = await _aiService.ChatCompletionAsync(orgId, prompt, $"Priorities: {prioritiesJson}");
            briefing.Summary = string.IsNullOrWhiteSpace(aiResponse) ? "Summary generation failed." : aiResponse.Trim();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to generate AI summary for daily briefing. Using fallback.");
            briefing.Summary = $"You have {topPriorities.Count} key priorities today. Please review them in the action center.";
        }
    }

    private DailyBusinessBriefingDto MapToDto(DailyBusinessBriefing briefing)
    {
        return new DailyBusinessBriefingDto
        {
            Id = briefing.Id,
            OrganizationId = briefing.OrganizationId,
            BriefingDate = briefing.BriefingDate,
            Summary = briefing.Summary,
            BusinessHealth = briefing.BusinessHealth.ToString(),
            PriorityCount = briefing.PriorityCount,
            OpportunityCount = briefing.OpportunityCount,
            RiskCount = briefing.RiskCount,
            CompletedPriorityCount = briefing.CompletedPriorityCount,
            GeneratedAt = briefing.GeneratedAt,
            ExpiresAt = briefing.ExpiresAt,
            Status = briefing.Status,
            Priorities = briefing.Priorities.Select(p => new DailyPriorityDto
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
            }).ToList()
        };
    }
}
