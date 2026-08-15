using System.Text.Json.Serialization;

namespace backend.Modules.DailyOperatingLoop.Entities;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum PriorityType
{
    UrgentRisk,
    RevenueOpportunity,
    CustomerFollowUp,
    Collections,
    InventoryRisk,
    GoalRisk,
    OperationalIssue,
    AutomationIssue,
    PositiveOpportunity,
    ImportantTask
}
