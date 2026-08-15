using System.Text.Json.Serialization;

namespace backend.Modules.DailyOperatingLoop.Entities;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum BusinessHealthState
{
    Healthy,
    NeedsAttention,
    AtRisk,
    Critical
}
