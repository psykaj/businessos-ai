namespace backend.Modules.Workflow.Constants;

public enum WorkflowStatus
{
    Draft,
    Active,
    Paused,
    Archived
}

public enum WorkflowExecutionStatus
{
    Pending,
    Running,
    Completed,
    Failed,
    Retrying,
    Cancelled
}

public enum TriggerType
{
    LeadCreated,
    LeadUpdated,
    LeadQualified,
    QRCodeScanned,
    CustomerRegistered,
    InvoicePaid,
    PaymentFailed,
    SubscriptionExpiring,
    FormSubmitted,
    CampaignCompleted,
    TeamMemberAdded,
    ScheduledTrigger,
    ManualTrigger
}

public enum ActionType
{
    CreateCrmLead,
    UpdateCrm,
    AssignSalesperson,
    SendEmail,
    SendWhatsApp,
    SendNotification,
    GenerateInvoice,
    GenerateQR,
    CallAiAssistant,
    CallWebhook,
    DelayExecution,
    UpdateCustomerJourney,
    LogActivity
}

public enum ConditionOperator
{
    Equals,
    NotEquals,
    Contains,
    DoesNotContain,
    GreaterThan,
    LessThan,
    GreaterThanOrEqual,
    LessThanOrEqual,
    DateBefore,
    DateAfter,
    DateSameDay,
    CustomerSegmentEquals,
    LeadScoreGreaterThan,
    PipelineStageEquals
}

public enum LogicalOperator
{
    AND,
    OR
}

public enum IntegrationProvider
{
    GoogleSheets,
    Slack,
    MicrosoftTeams,
    Discord,
    GoogleCalendar,
    OutlookCalendar,
    Resend,
    Twilio,
    Stripe,
    Razorpay,
    Webhook,
    RestApi
}

public enum IntegrationStatus
{
    Active,
    Disconnected,
    Expired,
    Error
}
