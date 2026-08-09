namespace backend.Modules.BusinessIntelligence.Entities;

public enum BriefingItemType
{
    Highlight,
    Risk,
    Opportunity,
    Recommendation,
    Action,
    Alert
}

public enum AlertSeverity
{
    Info,
    Low,
    Medium,
    High,
    Critical
}

public enum AlertStatus
{
    Unread,
    Read,
    ActionRequired,
    Resolved,
    Dismissed
}

public enum BriefingStatus
{
    Draft,
    Generated,
    Failed
}
