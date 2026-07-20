namespace backend.Modules.CRM.Enums;

public enum LeadStatus
{
    New,
    Contacted,
    Qualified,
    Unqualified,
    Converted
}

public enum PipelineStage
{
    NewLead,
    Contacted,
    Qualified,
    ProposalSent,
    Negotiation,
    Won,
    Lost
}

public enum ActivityType
{
    Call,
    Meeting,
    Email,
    Note,
    SystemEvent
}

public enum TaskPriority
{
    Low,
    Medium,
    High,
    Urgent
}

public enum CrmTaskStatus
{
    Pending,
    InProgress,
    Completed,
    Cancelled
}
