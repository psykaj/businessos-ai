namespace backend.Modules.ActionCenter.Entities;

public enum AiActionCategory
{
    GenerateInvoice,
    SendPaymentReminder,
    ReorderInventory,
    CreateFollowUpTask,
    CreateCrmActivity,
    SendWhatsAppCampaign,
    SendEmailCampaign,
    GenerateDiscountCoupon,
    ScheduleCustomerFollowUp,
    MarkInvoicePaid,
    Other
}

public enum AiActionPriority
{
    Low,
    Medium,
    High,
    Critical
}

public enum AiActionRiskLevel
{
    Low,
    Medium,
    High
}

public enum AiActionStatus
{
    Pending,
    Approved,
    Rejected,
    Executed,
    Failed
}
