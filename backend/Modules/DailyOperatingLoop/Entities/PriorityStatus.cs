using System.Text.Json.Serialization;

namespace backend.Modules.DailyOperatingLoop.Entities;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum PriorityStatus
{
    New,
    Viewed,
    InProgress,
    Completed,
    Dismissed,
    Snoozed,
    Expired
}
