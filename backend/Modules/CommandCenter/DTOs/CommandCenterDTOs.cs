using System;
using System.Collections.Generic;
using backend.Modules.BusinessIntelligence.DTOs;
using backend.Modules.ActionCenter.DTOs;

namespace backend.Modules.CommandCenter.DTOs;

public class CommandCenterSummaryDto
{
    public BusinessHealthDto? BusinessHealth { get; set; }
    public BusinessBriefingDto? ExecutiveSummary { get; set; }
    public List<CommandMetricDto> KeyMetrics { get; set; } = new();
    public List<ProactiveAlertDto> PriorityAlerts { get; set; } = new();
    public List<CommandOpportunityDto> TopOpportunities { get; set; } = new();
    public List<AiActionDto> RecommendedActions { get; set; } = new();
    public CommandAutomationSummaryDto? AutomationSummary { get; set; }
    public List<CommandActivityDto> RecentActivity { get; set; } = new();
    public List<CommandTrendDto> Trends { get; set; } = new();
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
}

public class CommandMetricDto
{
    public string Name { get; set; } = string.Empty;
    public decimal Value { get; set; }
    public decimal? PreviousValue { get; set; }
    public decimal? ChangePercentage { get; set; }
    public string Trend { get; set; } = "Neutral"; // Up, Down, Neutral
}

public class CommandOpportunityDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Priority { get; set; } = "Medium";
    public decimal? PotentialImpact { get; set; }
    public string RecommendedAction { get; set; } = string.Empty;
    public string SourceModule { get; set; } = string.Empty;
    public Guid SourceEntityId { get; set; }
}

public class CommandAutomationSummaryDto
{
    public int ActiveAutomations { get; set; }
    public int FailedAutomations { get; set; }
    public int PausedAutomations { get; set; }
    public int RecentlyExecuted { get; set; }
    public decimal AutomationSuccessRate { get; set; }
    public List<CommandAutomationOpportunityDto> AutomationOpportunities { get; set; } = new();
}

public class CommandAutomationOpportunityDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

public class CommandActivityDto
{
    public Guid Id { get; set; }
    public string ActivityType { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public string SourceModule { get; set; } = string.Empty;
}

public class CommandTrendDto
{
    public string MetricName { get; set; } = string.Empty;
    public string Period { get; set; } = "30 Days"; // 7 days, 30 days, 90 days
    public decimal Value { get; set; }
    public decimal PreviousValue { get; set; }
    public decimal PercentageChange { get; set; }
    public string TrendDirection { get; set; } = "Neutral";
}

public class CommandCenterAskRequestDto
{
    public string Question { get; set; } = string.Empty;
    public string? OptionalContext { get; set; }
}

public class CommandCenterAskResponseDto
{
    public string Answer { get; set; } = string.Empty;
    public List<string> RelatedMetrics { get; set; } = new();
    public List<string> RelatedEntities { get; set; } = new();
    public List<SuggestedActionDto> SuggestedActions { get; set; } = new();
    public List<string> SourceModules { get; set; } = new();
    public string Confidence { get; set; } = "High";
}

public class SuggestedActionDto
{
    public string Description { get; set; } = string.Empty;
    public string ActionType { get; set; } = string.Empty;
    public Guid? ActionCenterActionId { get; set; }
}
