namespace backend.Modules.Automation.Domain.Enums;

public enum WorkflowTriggerType
{
    InvoiceOverdue,
    InvoiceCreated,
    PaymentReceived,
    CustomerCreated,
    CustomerInactive,
    InventoryLow,
    InventoryOutOfStock,
    NewLeadCreated,
    LeadNotContacted,
    CustomerPurchaseCompleted,
    DailyBusinessSummary,
    WeeklyBusinessSummary,
    Manual
}

public enum WorkflowStatus
{
    Draft,
    Active,
    Inactive,
    Archived
}

public enum WorkflowStepType
{
    Condition,
    Action,
    AIDecision,
    Approval
}

public enum WorkflowExecutionStatus
{
    Pending,
    InProgress,
    WaitingForApproval,
    Completed,
    Failed,
    Cancelled
}

public enum WorkflowApprovalStatus
{
    Automatic,
    ApprovalRequired,
    Manual
}
