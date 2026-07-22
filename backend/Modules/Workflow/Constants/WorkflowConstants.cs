namespace backend.Modules.Workflow.Constants;

public static class WorkflowConstants
{
    public const int MaxExecutionTimeoutSeconds = 300; // 5 minutes max per execution
    public const int MaxRetryAttempts = 3;
    public const int DefaultPageSize = 20;
    public const int MaxPageSize = 100;

    public static class ContextKeys
    {
        public const string LeadId = "LeadId";
        public const string CustomerId = "CustomerId";
        public const string CustomerName = "CustomerName";
        public const string CustomerEmail = "CustomerEmail";
        public const string CustomerPhone = "CustomerPhone";
        public const string CompanyName = "CompanyName";
        public const string InvoiceId = "InvoiceId";
        public const string InvoiceNumber = "InvoiceNumber";
        public const string DealValue = "DealValue";
        public const string PipelineStage = "PipelineStage";
        public const string QRCodeId = "QRCodeId";
        public const string FormId = "FormId";
        public const string CampaignId = "CampaignId";
        public const string AiOutput = "AiOutput";
        public const string ExecutionUserId = "ExecutionUserId";
        public const string TriggeredAt = "TriggeredAt";
    }
}
