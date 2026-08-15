using System;
using System.Collections.Generic;
using System.Linq;
using backend.Modules.DailyOperatingLoop.Entities;
using backend.Modules.DailyOperatingLoop.Interfaces;

namespace backend.Modules.DailyOperatingLoop.Services;

public class DailyPriorityScoringService : IDailyPriorityScoringService
{
    public DailyPriority ScorePriority(DailyPriority priority)
    {
        decimal baseScore = GetBaseScore(priority.PriorityType);
        decimal severityMultiplier = GetSeverityMultiplier(priority.Severity);
        decimal urgencyMultiplier = GetUrgencyMultiplier(priority.DueAt);
        decimal confidenceMultiplier = priority.Confidence > 0 ? (priority.Confidence / 100m) : 0.8m; // Default confidence

        // Calculate score: Base * Severity * Urgency * Confidence
        // Cap at 100
        decimal rawScore = baseScore * severityMultiplier * urgencyMultiplier * confidenceMultiplier;
        
        priority.PriorityScore = Math.Min(Math.Max(Math.Round(rawScore, 2), 0m), 100m);
        
        return priority;
    }

    public List<DailyPriority> RankPriorities(IEnumerable<DailyPriority> priorities, int limit = 5)
    {
        // Only include active priorities that haven't expired
        var activePriorities = priorities.Where(p => 
            p.Status == PriorityStatus.New || 
            p.Status == PriorityStatus.Viewed || 
            p.Status == PriorityStatus.InProgress);

        return activePriorities
            .OrderByDescending(p => p.PriorityScore)
            .ThenBy(p => p.DueAt ?? DateTime.MaxValue)
            .Take(limit)
            .ToList();
    }

    private decimal GetBaseScore(PriorityType type)
    {
        return type switch
        {
            PriorityType.UrgentRisk => 90m,
            PriorityType.RevenueOpportunity => 85m,
            PriorityType.CustomerFollowUp => 70m,
            PriorityType.Collections => 80m,
            PriorityType.InventoryRisk => 75m,
            PriorityType.GoalRisk => 85m,
            PriorityType.OperationalIssue => 60m,
            PriorityType.AutomationIssue => 80m,
            PriorityType.PositiveOpportunity => 65m,
            PriorityType.ImportantTask => 50m,
            _ => 50m
        };
    }

    private decimal GetSeverityMultiplier(string severity)
    {
        return severity.ToLowerInvariant() switch
        {
            "critical" => 1.5m,
            "high" => 1.2m,
            "medium" => 1.0m,
            "low" => 0.8m,
            _ => 1.0m
        };
    }

    private decimal GetUrgencyMultiplier(DateTime? dueAt)
    {
        if (!dueAt.HasValue) return 1.0m;

        var daysUntilDue = (dueAt.Value.Date - DateTime.UtcNow.Date).TotalDays;

        if (daysUntilDue < 0) return 1.5m; // Overdue
        if (daysUntilDue == 0) return 1.3m; // Due today
        if (daysUntilDue <= 2) return 1.1m; // Due soon
        
        return 0.9m; // Not urgent yet
    }
}
