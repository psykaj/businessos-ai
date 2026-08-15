using backend.Common;
using backend.Entities;
using backend.Modules.QRCode.Models;
using backend.Modules.CRM.Entities;
using backend.Modules.Forms.Entities;
using backend.Modules.LeadCapture.Entities;
using backend.Modules.CustomerJourney.Entities;
using backend.Modules.Webhooks.Entities;
using backend.Modules.OAuth.Entities;
using backend.Modules.Connectors.Entities;
using backend.Modules.ApiPlatform.Entities;
using backend.Modules.EventBus.Entities;
using backend.Modules.KpiEngine.Entities;
using backend.Modules.Forecasting.Entities;
using backend.Modules.ExecutiveInsights.Entities;
using backend.Modules.BusinessGoals.Entities;
using backend.Modules.Scorecards.Entities;
using backend.Modules.BusinessHealth.Entities;
using backend.Modules.AiAgent.Entities;
using backend.Modules.AiRecommendations.Entities;
using backend.Modules.Benchmarks.Entities;
using backend.Modules.DecisionCenter.Entities;
using backend.Modules.ActionCenter.Entities;
using backend.Modules.BusinessIntelligence.Entities;
using backend.Modules.BusinessMemory.Entities;
using backend.Modules.Outcomes.Entities;
using Microsoft.EntityFrameworkCore;

namespace backend.Persistence;

public sealed class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Organization> Organizations => Set<Organization>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<QRCode> QRCodes => Set<QRCode>();
    public DbSet<backend.Modules.Analytics.Models.QRScan> QRScans => Set<backend.Modules.Analytics.Models.QRScan>();
    public DbSet<Review> Reviews => Set<Review>();
    public DbSet<Invoice> Invoices => Set<Invoice>();
    public DbSet<Campaign> Campaigns => Set<Campaign>();
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<Contact> Contacts => Set<Contact>();
    public DbSet<LandingPage> LandingPages => Set<LandingPage>();
    public DbSet<AIConversation> AIConversations => Set<AIConversation>();
    public DbSet<AIMessage> AIMessages => Set<AIMessage>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<EmailTemplate> EmailTemplates => Set<EmailTemplate>();
    public DbSet<AutomationRule> AutomationRules => Set<AutomationRule>();
    public DbSet<AutomationLog> AutomationLogs => Set<AutomationLog>();
    public DbSet<ActivityLog> ActivityLogs => Set<ActivityLog>();
    public DbSet<BusinessCard> BusinessCards => Set<BusinessCard>();
    public DbSet<Subscription> Subscriptions => Set<Subscription>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<TeamMember> TeamMembers => Set<TeamMember>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();
    public DbSet<Invitation> Invitations => Set<Invitation>();
    public DbSet<ApiKey> ApiKeys => Set<ApiKey>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    // Day 20 - Developer Platform & Webhooks
    public DbSet<OAuthApplication> OAuthApplications => Set<OAuthApplication>();
    public DbSet<WebhookEndpoint> WebhookEndpoints => Set<WebhookEndpoint>();
    public DbSet<WebhookDeliveryLog> WebhookDeliveryLogs => Set<WebhookDeliveryLog>();
    public DbSet<Connector> Connectors => Set<Connector>();
    public DbSet<ApiLog> ApiLogs => Set<ApiLog>();
    public DbSet<EventSubscription> EventSubscriptions => Set<EventSubscription>();
    public DbSet<EventLog> EventLogs => Set<EventLog>();

    public DbSet<Brand> Brands => Set<Brand>();
    public DbSet<CustomDomain> CustomDomains => Set<CustomDomain>();
    public DbSet<Theme> Themes => Set<Theme>();
    public DbSet<LandingPageSection> LandingPageSections => Set<LandingPageSection>();
    public DbSet<SEOSettings> SEOSettings => Set<SEOSettings>();

    // CRM Entities
    public DbSet<Lead> Leads => Set<Lead>();
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<Deal> Deals => Set<Deal>();
    public DbSet<CrmActivity> CrmActivities => Set<CrmActivity>();
    public DbSet<CrmTask> CrmTasks => Set<CrmTask>();
    public DbSet<Note> Notes => Set<Note>();
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<DealStageHistory> DealStageHistories => Set<DealStageHistory>();

    public DbSet<WhatsAppSettings> WhatsAppSettings => Set<WhatsAppSettings>();
    public DbSet<WhatsAppTemplate> WhatsAppTemplates => Set<WhatsAppTemplate>();
    public DbSet<CampaignContact> CampaignContacts => Set<CampaignContact>();

    // Day 12 - Lead Capture & Marketing Automation
    public DbSet<Form> Forms => Set<Form>();
    public DbSet<FormField> FormFields => Set<FormField>();
    public DbSet<FormSubmission> FormSubmissions => Set<FormSubmission>();
    public DbSet<LeadSource> LeadSources => Set<LeadSource>();
    public DbSet<backend.Modules.CustomerJourney.Entities.CustomerJourney> CustomerJourneys => Set<backend.Modules.CustomerJourney.Entities.CustomerJourney>();

    // Day 13 - Workflow Automation, Integrations & Webhook Platform
    public DbSet<backend.Modules.Workflow.Entities.Workflow> Workflows => Set<backend.Modules.Workflow.Entities.Workflow>();
    public DbSet<backend.Modules.Workflow.Entities.WorkflowTrigger> WorkflowTriggers => Set<backend.Modules.Workflow.Entities.WorkflowTrigger>();
    public DbSet<backend.Modules.Workflow.Entities.WorkflowAction> WorkflowActions => Set<backend.Modules.Workflow.Entities.WorkflowAction>();

    // Day 16 - Action Center
    public DbSet<AiAction> AiActions => Set<AiAction>();
    public DbSet<backend.Modules.Workflow.Entities.WorkflowCondition> WorkflowConditions => Set<backend.Modules.Workflow.Entities.WorkflowCondition>();
    public DbSet<backend.Modules.Workflow.Entities.WorkflowExecution> WorkflowExecutions => Set<backend.Modules.Workflow.Entities.WorkflowExecution>();
    public DbSet<backend.Modules.Workflow.Entities.WorkflowExecutionLog> WorkflowExecutionLogs => Set<backend.Modules.Workflow.Entities.WorkflowExecutionLog>();
    public DbSet<backend.Modules.Workflow.Entities.Integration> Integrations => Set<backend.Modules.Workflow.Entities.Integration>();

    // Day 21 - Executive Decision Platform (Replaces Day 14 & Day 15 dummy entities)
    public DbSet<KPI> KPIs => Set<KPI>();
    public DbSet<KPIHistory> KPIHistories => Set<KPIHistory>();
    public DbSet<Forecast> Forecasts => Set<Forecast>();
    public DbSet<ExecutiveInsight> ExecutiveInsights => Set<ExecutiveInsight>();
    public DbSet<BusinessGoal> BusinessGoals => Set<BusinessGoal>();
    public DbSet<BusinessKPI> BusinessKPIs => Set<BusinessKPI>();
    public DbSet<Scorecard> Scorecards => Set<Scorecard>();
    public DbSet<BusinessHealthScore> BusinessHealthScores => Set<BusinessHealthScore>();
    public DbSet<Recommendation> Recommendations => Set<Recommendation>();
    public DbSet<AiRecommendation> AiRecommendations => Set<AiRecommendation>();
    public DbSet<Benchmark> Benchmarks => Set<Benchmark>();
    public DbSet<DecisionLog> DecisionLogs => Set<DecisionLog>();

    // Day 15 - AI Business Agent, Task Execution Engine & Enterprise AI Copilot
    public DbSet<backend.Modules.AiAgent.Entities.Conversation> Conversations => Set<backend.Modules.AiAgent.Entities.Conversation>();
    public DbSet<backend.Modules.AiAgent.Entities.Message> Messages => Set<backend.Modules.AiAgent.Entities.Message>();
    public DbSet<backend.Modules.AiAgent.Entities.CommandExecution> CommandExecutions => Set<backend.Modules.AiAgent.Entities.CommandExecution>();
    public DbSet<backend.Modules.AiAgent.Entities.ToolDefinition> ToolDefinitions => Set<backend.Modules.AiAgent.Entities.ToolDefinition>();

    // Day 16 - Customer Success, Retention & Loyalty Platform
    public DbSet<backend.Modules.CustomerSuccess.CustomerHealth.Entities.CustomerHealth> CustomerHealths => Set<backend.Modules.CustomerSuccess.CustomerHealth.Entities.CustomerHealth>();
    public DbSet<backend.Modules.CustomerSuccess.Loyalty.Entities.LoyaltyProgram> LoyaltyPrograms => Set<backend.Modules.CustomerSuccess.Loyalty.Entities.LoyaltyProgram>();
    public DbSet<backend.Modules.CustomerSuccess.Loyalty.Entities.LoyaltyTransaction> LoyaltyTransactions => Set<backend.Modules.CustomerSuccess.Loyalty.Entities.LoyaltyTransaction>();
    public DbSet<backend.Modules.CustomerSuccess.Referrals.Entities.Referral> Referrals => Set<backend.Modules.CustomerSuccess.Referrals.Entities.Referral>();
    public DbSet<backend.Modules.CustomerSuccess.Satisfaction.Entities.CustomerFeedback> CustomerFeedbacks => Set<backend.Modules.CustomerSuccess.Satisfaction.Entities.CustomerFeedback>();
    public DbSet<backend.Modules.CustomerSuccess.SuccessTasks.Entities.SuccessTask> SuccessTasks => Set<backend.Modules.CustomerSuccess.SuccessTasks.Entities.SuccessTask>();
    public DbSet<backend.Modules.CustomerSuccess.CustomerSegments.Entities.CustomerSegment> CustomerSegments => Set<backend.Modules.CustomerSuccess.CustomerSegments.Entities.CustomerSegment>();

    // Day 17 - Document Management System (DMS), E-Signature & Approval Workflow
    public DbSet<backend.Modules.Documents.Entities.Document> Documents => Set<backend.Modules.Documents.Entities.Document>();
    public DbSet<backend.Modules.Documents.Entities.Folder> Folders => Set<backend.Modules.Documents.Entities.Folder>();
    public DbSet<backend.Modules.Documents.Entities.DocumentVersion> DocumentVersions => Set<backend.Modules.Documents.Entities.DocumentVersion>();
    public DbSet<backend.Modules.Documents.Entities.DocumentTemplate> DocumentTemplates => Set<backend.Modules.Documents.Entities.DocumentTemplate>();
    public DbSet<backend.Modules.Documents.Entities.ApprovalRequest> ApprovalRequests => Set<backend.Modules.Documents.Entities.ApprovalRequest>();
    public DbSet<backend.Modules.Documents.Entities.ApprovalStep> ApprovalSteps => Set<backend.Modules.Documents.Entities.ApprovalStep>();
    public DbSet<backend.Modules.Documents.Entities.SignatureRequest> SignatureRequests => Set<backend.Modules.Documents.Entities.SignatureRequest>();
    public DbSet<backend.Modules.Documents.Entities.SignatureRecipient> SignatureRecipients => Set<backend.Modules.Documents.Entities.SignatureRecipient>();
    public DbSet<backend.Modules.Documents.Entities.SharedDocument> SharedDocuments => Set<backend.Modules.Documents.Entities.SharedDocument>();
    public DbSet<backend.Modules.Documents.Entities.DocumentAuditEntry> DocumentAuditEntries => Set<backend.Modules.Documents.Entities.DocumentAuditEntry>();

    // Day 18 - Inventory Management, Purchasing & Supplier Platform
    public DbSet<backend.Modules.Inventory.Entities.Product> Products => Set<backend.Modules.Inventory.Entities.Product>();
    public DbSet<backend.Modules.Inventory.Entities.ProductCategory> ProductCategories => Set<backend.Modules.Inventory.Entities.ProductCategory>();
    public DbSet<backend.Modules.Inventory.Entities.Warehouse> Warehouses => Set<backend.Modules.Inventory.Entities.Warehouse>();
    public DbSet<backend.Modules.Inventory.Entities.InventoryStock> InventoryStocks => Set<backend.Modules.Inventory.Entities.InventoryStock>();
    public DbSet<backend.Modules.Inventory.Entities.StockMovement> StockMovements => Set<backend.Modules.Inventory.Entities.StockMovement>();
    public DbSet<backend.Modules.Inventory.Entities.Supplier> Suppliers => Set<backend.Modules.Inventory.Entities.Supplier>();
    public DbSet<backend.Modules.Inventory.Entities.PurchaseOrder> PurchaseOrders => Set<backend.Modules.Inventory.Entities.PurchaseOrder>();
    public DbSet<backend.Modules.Inventory.Entities.PurchaseOrderItem> PurchaseOrderItems => Set<backend.Modules.Inventory.Entities.PurchaseOrderItem>();
    public DbSet<backend.Modules.Inventory.Entities.GoodsReceipt> GoodsReceipts => Set<backend.Modules.Inventory.Entities.GoodsReceipt>();
    public DbSet<backend.Modules.Inventory.Entities.GoodsReceiptItem> GoodsReceiptItems => Set<backend.Modules.Inventory.Entities.GoodsReceiptItem>();
    public DbSet<backend.Modules.Inventory.Entities.StockAdjustment> StockAdjustments => Set<backend.Modules.Inventory.Entities.StockAdjustment>();
    public DbSet<backend.Modules.Inventory.Entities.StockAdjustmentItem> StockAdjustmentItems => Set<backend.Modules.Inventory.Entities.StockAdjustmentItem>();

    // Day 19 - Accounting, Finance, Cash Flow & Expense Management
    public DbSet<backend.Modules.Accounting.Entities.Account> Accounts => Set<backend.Modules.Accounting.Entities.Account>();
    public DbSet<backend.Modules.Expenses.Entities.ExpenseCategory> ExpenseCategories => Set<backend.Modules.Expenses.Entities.ExpenseCategory>();
    public DbSet<backend.Modules.Expenses.Entities.Expense> Expenses => Set<backend.Modules.Expenses.Entities.Expense>();
    public DbSet<backend.Modules.Invoices.Entities.FinanceInvoice> FinanceInvoices => Set<backend.Modules.Invoices.Entities.FinanceInvoice>();
    public DbSet<backend.Modules.Invoices.Entities.FinanceInvoiceItem> FinanceInvoiceItems => Set<backend.Modules.Invoices.Entities.FinanceInvoiceItem>();
    public DbSet<backend.Modules.Payments.Entities.FinancePayment> FinancePayments => Set<backend.Modules.Payments.Entities.FinancePayment>();
    public DbSet<backend.Modules.AccountsReceivable.Entities.AccountsReceivableRecord> AccountsReceivable => Set<backend.Modules.AccountsReceivable.Entities.AccountsReceivableRecord>();
    public DbSet<backend.Modules.AccountsPayable.Entities.AccountsPayableRecord> AccountsPayable => Set<backend.Modules.AccountsPayable.Entities.AccountsPayableRecord>();
    public DbSet<backend.Modules.CashFlow.Entities.CashFlowEntry> CashFlowEntries => Set<backend.Modules.CashFlow.Entities.CashFlowEntry>();
    public DbSet<backend.Modules.Taxes.Entities.TaxRecord> TaxRecords => Set<backend.Modules.Taxes.Entities.TaxRecord>();
    public DbSet<backend.Modules.FinancialReports.Entities.FinancialReport> FinancialReports => Set<backend.Modules.FinancialReports.Entities.FinancialReport>();

    // Day 22 - AI Automation Studio
    public DbSet<AutomationWorkflow> AutomationWorkflows => Set<AutomationWorkflow>();
    public DbSet<backend.Entities.Trigger> AutomationTriggers => Set<backend.Entities.Trigger>();
    public DbSet<backend.Entities.Condition> AutomationConditions => Set<backend.Entities.Condition>();
    public DbSet<backend.Entities.Action> AutomationActions => Set<backend.Entities.Action>();
    public DbSet<backend.Entities.WorkflowExecution> StudioWorkflowExecutions => Set<backend.Entities.WorkflowExecution>();
    public DbSet<backend.Entities.ExecutionLog> StudioExecutionLogs => Set<backend.Entities.ExecutionLog>();
    public DbSet<Schedule> Schedules => Set<Schedule>();
    public DbSet<WorkflowTemplate> WorkflowTemplates => Set<WorkflowTemplate>();
    public DbSet<WorkflowVersion> WorkflowVersions => Set<WorkflowVersion>();

    // Day 23 - Customer Communication Hub & Omnichannel Messaging
    public DbSet<backend.Modules.CommunicationHub.Entities.Conversation> CommConversations => Set<backend.Modules.CommunicationHub.Entities.Conversation>();
    public DbSet<backend.Modules.CommunicationHub.Entities.Message> CommMessages => Set<backend.Modules.CommunicationHub.Entities.Message>();
    public DbSet<backend.Modules.CommunicationHub.Entities.CommunicationChannel> CommunicationChannels => Set<backend.Modules.CommunicationHub.Entities.CommunicationChannel>();
    public DbSet<backend.Modules.CommunicationHub.Entities.MessageTemplate> CommMessageTemplates => Set<backend.Modules.CommunicationHub.Entities.MessageTemplate>();
    public DbSet<backend.Modules.CommunicationHub.Entities.Assignment> CommAssignments => Set<backend.Modules.CommunicationHub.Entities.Assignment>();
    public DbSet<backend.Modules.CommunicationHub.Entities.CommunicationNotification> CommNotifications => Set<backend.Modules.CommunicationHub.Entities.CommunicationNotification>();
    public DbSet<backend.Modules.CommunicationHub.Entities.ConversationTag> ConversationTags => Set<backend.Modules.CommunicationHub.Entities.ConversationTag>();
    public DbSet<backend.Modules.CommunicationHub.Entities.ConversationStatusEntity> ConversationStatuses => Set<backend.Modules.CommunicationHub.Entities.ConversationStatusEntity>();
    public DbSet<backend.Modules.CommunicationHub.Entities.CommunicationMetric> CommunicationMetrics => Set<backend.Modules.CommunicationHub.Entities.CommunicationMetric>();

    // Day 24 - Customer Feedback & Service Quality
    public DbSet<backend.Modules.CustomerFeedback.Entities.Feedback> Feedbacks => Set<backend.Modules.CustomerFeedback.Entities.Feedback>();
    public DbSet<backend.Modules.CustomerFeedback.Entities.Survey> Surveys => Set<backend.Modules.CustomerFeedback.Entities.Survey>();
    public DbSet<backend.Modules.CustomerFeedback.Entities.SurveyQuestion> SurveyQuestions => Set<backend.Modules.CustomerFeedback.Entities.SurveyQuestion>();
    public DbSet<backend.Modules.CustomerFeedback.Entities.SurveyResponse> SurveyResponses => Set<backend.Modules.CustomerFeedback.Entities.SurveyResponse>();
    public DbSet<backend.Modules.CustomerFeedback.Entities.Rating> Ratings => Set<backend.Modules.CustomerFeedback.Entities.Rating>();
    public DbSet<backend.Modules.CustomerFeedback.Entities.SentimentAnalysis> SentimentAnalyses => Set<backend.Modules.CustomerFeedback.Entities.SentimentAnalysis>();
    public DbSet<backend.Modules.CustomerFeedback.Entities.CustomerSatisfactionScore> CustomerSatisfactionScores => Set<backend.Modules.CustomerFeedback.Entities.CustomerSatisfactionScore>();
    public DbSet<backend.Modules.CustomerFeedback.Entities.ServiceMetric> ServiceMetrics => Set<backend.Modules.CustomerFeedback.Entities.ServiceMetric>();
    public DbSet<backend.Modules.CustomerFeedback.Entities.ImprovementRecommendation> ImprovementRecommendations => Set<backend.Modules.CustomerFeedback.Entities.ImprovementRecommendation>();

    // Day 25 - AI Business Performance & Growth Intelligence
    public DbSet<backend.Modules.BusinessPerformance.Entities.BusinessMetric> BusinessMetrics => Set<backend.Modules.BusinessPerformance.Entities.BusinessMetric>();
    public DbSet<backend.Modules.RevenueAnalytics.Entities.RevenueSnapshot> RevenueSnapshots => Set<backend.Modules.RevenueAnalytics.Entities.RevenueSnapshot>();
    public DbSet<backend.Modules.Profitability.Entities.ProfitSnapshot> ProfitSnapshots => Set<backend.Modules.Profitability.Entities.ProfitSnapshot>();
    public DbSet<backend.Modules.ProductAnalytics.Entities.ProductPerformance> ProductPerformances => Set<backend.Modules.ProductAnalytics.Entities.ProductPerformance>();
    public DbSet<backend.Modules.CustomerAnalytics.Entities.CustomerPerformance> CustomerPerformances => Set<backend.Modules.CustomerAnalytics.Entities.CustomerPerformance>();
    public DbSet<backend.Modules.MarketingROI.Entities.MarketingPerformance> MarketingPerformances => Set<backend.Modules.MarketingROI.Entities.MarketingPerformance>();
    public DbSet<backend.Modules.GrowthRecommendations.Entities.GrowthRecommendation> GrowthRecommendations => Set<backend.Modules.GrowthRecommendations.Entities.GrowthRecommendation>();
    public DbSet<backend.Modules.Benchmarking.Entities.BenchmarkMetric> BenchmarkMetrics => Set<backend.Modules.Benchmarking.Entities.BenchmarkMetric>();

    // Day 26 - Multi-Branch & Multi-Location Management Platform
    public DbSet<backend.Modules.Locations.Entities.Location> Locations => Set<backend.Modules.Locations.Entities.Location>();
    public DbSet<backend.Modules.Branches.Entities.Branch> Branches => Set<backend.Modules.Branches.Entities.Branch>();
    public DbSet<backend.Modules.Branches.Entities.BranchManager> BranchManagers => Set<backend.Modules.Branches.Entities.BranchManager>();
    public DbSet<backend.Modules.Warehouses.Entities.Warehouse> BranchWarehouses => Set<backend.Modules.Warehouses.Entities.Warehouse>();
    public DbSet<backend.Modules.Transfers.Entities.WarehouseTransfer> WarehouseTransfers => Set<backend.Modules.Transfers.Entities.WarehouseTransfer>();
    public DbSet<backend.Modules.BranchAnalytics.Entities.BranchPerformance> BranchPerformances => Set<backend.Modules.BranchAnalytics.Entities.BranchPerformance>();
    public DbSet<backend.Modules.RegionalReports.Entities.RegionalSummary> RegionalSummaries => Set<backend.Modules.RegionalReports.Entities.RegionalSummary>();

    // Day 28 - AI Automation & Workflow Engine
    public DbSet<backend.Modules.Automation.Domain.Entities.AiWorkflow> AiEngineWorkflows => Set<backend.Modules.Automation.Domain.Entities.AiWorkflow>();
    public DbSet<backend.Modules.Automation.Domain.Entities.WorkflowStep> AiEngineWorkflowSteps => Set<backend.Modules.Automation.Domain.Entities.WorkflowStep>();
    public DbSet<backend.Modules.Automation.Domain.Entities.WorkflowExecution> AiEngineWorkflowExecutions => Set<backend.Modules.Automation.Domain.Entities.WorkflowExecution>();
    public DbSet<backend.Modules.Automation.Domain.Entities.WorkflowExecutionStep> AiEngineWorkflowExecutionSteps => Set<backend.Modules.Automation.Domain.Entities.WorkflowExecutionStep>();
    public DbSet<backend.Modules.Automation.Domain.Entities.WorkflowTemplate> AiEngineWorkflowTemplates => Set<backend.Modules.Automation.Domain.Entities.WorkflowTemplate>();

    // Day 29 - Business Intelligence & Alerts
    public DbSet<BusinessBriefing> BusinessBriefings => Set<BusinessBriefing>();
    public DbSet<BriefingItem> BriefingItems => Set<BriefingItem>();
    public DbSet<ProactiveAlert> ProactiveAlerts => Set<ProactiveAlert>();
    
    // Day 31 - AI Business Memory & Context Engine
    public DbSet<BusinessMemory> BusinessMemories => Set<BusinessMemory>();

    // Day 32 - AI Outcome & ROI Intelligence Engine
    public DbSet<BusinessOutcome> BusinessOutcomes => Set<BusinessOutcome>();

    // Day 34 - AI Daily Business Operating Loop
    public DbSet<backend.Modules.DailyOperatingLoop.Entities.DailyBusinessBriefing> DailyBusinessBriefings => Set<backend.Modules.DailyOperatingLoop.Entities.DailyBusinessBriefing>();
    public DbSet<backend.Modules.DailyOperatingLoop.Entities.DailyPriority> DailyPriorities => Set<backend.Modules.DailyOperatingLoop.Entities.DailyPriority>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Configure composite key for UserRole
        modelBuilder.Entity<UserRole>()
            .HasKey(ur => new { ur.UserId, ur.RoleId });

        // Day 10 - AI & Automation constraints and filters
        modelBuilder.Entity<AIConversation>().HasIndex(c => c.OrganizationId);
        modelBuilder.Entity<AIMessage>().HasIndex(m => m.ConversationId);
        modelBuilder.Entity<Notification>().HasIndex(n => n.OrganizationId);
        modelBuilder.Entity<EmailTemplate>().HasIndex(e => e.OrganizationId);
        modelBuilder.Entity<AutomationRule>().HasIndex(a => a.OrganizationId);
        modelBuilder.Entity<AutomationLog>().HasIndex(l => l.RuleId);
        modelBuilder.Entity<WhatsAppTemplate>().HasIndex(w => w.OrganizationId);
        
        // CRM Constraints
        modelBuilder.Entity<Lead>().HasIndex(l => l.OrganizationId);
        modelBuilder.Entity<Company>().HasIndex(c => c.OrganizationId);
        modelBuilder.Entity<Deal>().HasIndex(d => d.OrganizationId);
        modelBuilder.Entity<CrmActivity>().HasIndex(a => a.OrganizationId);
        modelBuilder.Entity<CrmTask>().HasIndex(t => t.OrganizationId);
        modelBuilder.Entity<Note>().HasIndex(n => n.OrganizationId);
        modelBuilder.Entity<Tag>().HasIndex(t => t.OrganizationId);
        
        // Day 12 Constraints
        modelBuilder.Entity<Form>().HasIndex(f => f.OrganizationId);
        modelBuilder.Entity<FormSubmission>().HasIndex(fs => fs.OrganizationId);
        modelBuilder.Entity<LeadSource>().HasIndex(ls => ls.OrganizationId);
        modelBuilder.Entity<backend.Modules.CustomerJourney.Entities.CustomerJourney>().HasIndex(cj => cj.OrganizationId);
        
        // Day 13 Workflow Constraints
        modelBuilder.Entity<backend.Modules.Workflow.Entities.Workflow>().HasIndex(w => w.OrganizationId);
        modelBuilder.Entity<backend.Modules.Workflow.Entities.WorkflowExecution>().HasIndex(e => e.OrganizationId);
        modelBuilder.Entity<backend.Modules.Workflow.Entities.Integration>().HasIndex(i => i.OrganizationId);
        
        // Day 21 Executive Decision Constraints
        modelBuilder.Entity<KPI>().HasIndex(k => k.OrganizationId);
        modelBuilder.Entity<KPIHistory>().HasIndex(k => k.OrganizationId);
        modelBuilder.Entity<Forecast>().HasIndex(f => f.OrganizationId);
        modelBuilder.Entity<ExecutiveInsight>().HasIndex(e => e.OrganizationId);
        modelBuilder.Entity<BusinessGoal>().HasIndex(b => b.OrganizationId);
        modelBuilder.Entity<BusinessGoal>().HasIndex(b => new { b.OrganizationId, b.Status });
        modelBuilder.Entity<BusinessKPI>().HasIndex(k => k.OrganizationId);
        modelBuilder.Entity<BusinessKPI>().HasIndex(k => k.BusinessGoalId);
        modelBuilder.Entity<Scorecard>().HasIndex(s => s.OrganizationId);
        modelBuilder.Entity<BusinessHealthScore>().HasIndex(b => b.OrganizationId);
        modelBuilder.Entity<Recommendation>().HasIndex(r => r.OrganizationId);
        modelBuilder.Entity<AiRecommendation>().HasIndex(r => r.OrganizationId);
        modelBuilder.Entity<Benchmark>().HasIndex(b => b.OrganizationId);
        modelBuilder.Entity<DecisionLog>().HasIndex(d => d.OrganizationId);

        // Day 15 AI Agent Constraints
        modelBuilder.Entity<backend.Modules.AiAgent.Entities.Conversation>().HasIndex(c => c.OrganizationId);
        modelBuilder.Entity<backend.Modules.AiAgent.Entities.Message>().HasIndex(m => m.ConversationId);
        modelBuilder.Entity<backend.Modules.AiAgent.Entities.CommandExecution>().HasIndex(ce => ce.OrganizationId);

        // Day 16 Customer Success Constraints
        modelBuilder.Entity<backend.Modules.CustomerSuccess.CustomerHealth.Entities.CustomerHealth>().HasIndex(ch => ch.OrganizationId);
        modelBuilder.Entity<backend.Modules.CustomerSuccess.CustomerHealth.Entities.CustomerHealth>().HasIndex(ch => ch.CustomerId);
        modelBuilder.Entity<backend.Modules.CustomerSuccess.Loyalty.Entities.LoyaltyProgram>().HasIndex(lp => lp.OrganizationId);
        modelBuilder.Entity<backend.Modules.CustomerSuccess.Loyalty.Entities.LoyaltyTransaction>().HasIndex(lt => lt.OrganizationId);
        modelBuilder.Entity<backend.Modules.CustomerSuccess.Loyalty.Entities.LoyaltyTransaction>().HasIndex(lt => lt.CustomerId);
        modelBuilder.Entity<backend.Modules.CustomerSuccess.Referrals.Entities.Referral>().HasIndex(r => r.OrganizationId);
        modelBuilder.Entity<backend.Modules.CustomerSuccess.Referrals.Entities.Referral>().HasIndex(r => r.ReferralCode);
        modelBuilder.Entity<backend.Modules.CustomerSuccess.Satisfaction.Entities.CustomerFeedback>().HasIndex(cf => cf.OrganizationId);
        modelBuilder.Entity<backend.Modules.CustomerSuccess.SuccessTasks.Entities.SuccessTask>().HasIndex(st => st.OrganizationId);
        modelBuilder.Entity<backend.Modules.CustomerSuccess.CustomerSegments.Entities.CustomerSegment>().HasIndex(cs => cs.OrganizationId);

        // Day 18 Inventory & Purchasing Constraints
        modelBuilder.Entity<backend.Modules.Inventory.Entities.Product>().HasIndex(p => p.OrganizationId);
        modelBuilder.Entity<backend.Modules.Inventory.Entities.Product>().HasIndex(p => p.SKU);
        modelBuilder.Entity<backend.Modules.Inventory.Entities.Product>().HasIndex(p => p.Barcode);
        modelBuilder.Entity<backend.Modules.Inventory.Entities.ProductCategory>().HasIndex(c => c.OrganizationId);
        modelBuilder.Entity<backend.Modules.Inventory.Entities.Warehouse>().HasIndex(w => w.OrganizationId);
        modelBuilder.Entity<backend.Modules.Inventory.Entities.InventoryStock>().HasIndex(s => new { s.OrganizationId, s.ProductId, s.WarehouseId }).IsUnique();
        modelBuilder.Entity<backend.Modules.Inventory.Entities.StockMovement>().HasIndex(m => m.OrganizationId);
        modelBuilder.Entity<backend.Modules.Inventory.Entities.Supplier>().HasIndex(s => s.OrganizationId);
        modelBuilder.Entity<backend.Modules.Inventory.Entities.Supplier>().HasIndex(s => s.Code);
        modelBuilder.Entity<backend.Modules.Inventory.Entities.PurchaseOrder>().HasIndex(po => po.OrganizationId);
        modelBuilder.Entity<backend.Modules.Inventory.Entities.PurchaseOrder>().HasIndex(po => po.PONumber);
        modelBuilder.Entity<backend.Modules.Inventory.Entities.GoodsReceipt>().HasIndex(gr => gr.OrganizationId);
        modelBuilder.Entity<backend.Modules.Inventory.Entities.GoodsReceipt>().HasIndex(gr => gr.ReceiptNumber);
        modelBuilder.Entity<backend.Modules.Inventory.Entities.StockAdjustment>().HasIndex(sa => sa.OrganizationId);
        modelBuilder.Entity<backend.Modules.Inventory.Entities.StockAdjustment>().HasIndex(sa => sa.AdjustmentNumber);

        // Day 19 Accounting & Finance Constraints
        modelBuilder.Entity<backend.Modules.Accounting.Entities.Account>().HasIndex(a => a.OrganizationId);
        modelBuilder.Entity<backend.Modules.Accounting.Entities.Account>().HasIndex(a => new { a.OrganizationId, a.AccountCode }).IsUnique();
        modelBuilder.Entity<backend.Modules.Expenses.Entities.ExpenseCategory>().HasIndex(ec => ec.OrganizationId);
        modelBuilder.Entity<backend.Modules.Expenses.Entities.Expense>().HasIndex(e => e.OrganizationId);
        modelBuilder.Entity<backend.Modules.Expenses.Entities.Expense>().HasIndex(e => e.ExpenseDate);
        modelBuilder.Entity<backend.Modules.Invoices.Entities.FinanceInvoice>().HasIndex(fi => fi.OrganizationId);
        modelBuilder.Entity<backend.Modules.Invoices.Entities.FinanceInvoice>().HasIndex(fi => fi.InvoiceNumber);
        modelBuilder.Entity<backend.Modules.Payments.Entities.FinancePayment>().HasIndex(fp => fp.OrganizationId);
        modelBuilder.Entity<backend.Modules.AccountsReceivable.Entities.AccountsReceivableRecord>().HasIndex(ar => ar.OrganizationId);
        modelBuilder.Entity<backend.Modules.AccountsPayable.Entities.AccountsPayableRecord>().HasIndex(ap => ap.OrganizationId);
        modelBuilder.Entity<backend.Modules.CashFlow.Entities.CashFlowEntry>().HasIndex(cf => cf.OrganizationId);
        modelBuilder.Entity<backend.Modules.CashFlow.Entities.CashFlowEntry>().HasIndex(cf => cf.EntryDate);
        modelBuilder.Entity<backend.Modules.Taxes.Entities.TaxRecord>().HasIndex(tr => tr.OrganizationId);
        modelBuilder.Entity<backend.Modules.FinancialReports.Entities.FinancialReport>().HasIndex(fr => fr.OrganizationId);

        modelBuilder.Entity<backend.Modules.DailyOperatingLoop.Entities.DailyBusinessBriefing>(entity =>
        {
            entity.HasIndex(e => new { e.OrganizationId, e.BriefingDate });
            entity.HasIndex(e => e.Status);
        });

        modelBuilder.Entity<backend.Modules.DailyOperatingLoop.Entities.DailyPriority>(entity =>
        {
            entity.HasIndex(e => new { e.OrganizationId, e.BriefingId });
            entity.HasIndex(e => new { e.OrganizationId, e.PriorityType });
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => e.RelatedEntityId);
            entity.HasIndex(e => e.PriorityScore);
            
            entity.HasOne(p => p.Briefing)
                  .WithMany(b => b.Priorities)
                  .HasForeignKey(p => p.BriefingId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Day 20 Constraints
        modelBuilder.Entity<OAuthApplication>().HasIndex(o => o.OrganizationId);

        // Day 21 - Event Sourcing / Bus
        modelBuilder.Entity<EventSubscription>().HasIndex(o => o.OrganizationId);
        modelBuilder.Entity<OAuthApplication>().HasIndex(o => o.ClientId).IsUnique();
        modelBuilder.Entity<WebhookEndpoint>().HasIndex(w => w.OrganizationId);
        modelBuilder.Entity<WebhookDeliveryLog>().HasIndex(w => w.OrganizationId);
        modelBuilder.Entity<WebhookDeliveryLog>().HasIndex(w => w.WebhookEndpointId);
        modelBuilder.Entity<Connector>().HasIndex(c => c.OrganizationId);
        modelBuilder.Entity<ApiLog>().HasIndex(a => a.OrganizationId);
        modelBuilder.Entity<ApiLog>().HasIndex(a => a.ApiKeyId);
        modelBuilder.Entity<EventSubscription>().HasIndex(e => e.OrganizationId);
        modelBuilder.Entity<EventLog>().HasIndex(e => e.OrganizationId);

        // Day 22 - AI Automation Studio Constraints
        modelBuilder.Entity<AutomationWorkflow>().HasIndex(w => w.OrganizationId);
        modelBuilder.Entity<backend.Entities.Trigger>().HasIndex(t => t.OrganizationId);
        modelBuilder.Entity<backend.Entities.Condition>().HasIndex(c => c.OrganizationId);
        modelBuilder.Entity<backend.Entities.Action>().HasIndex(a => a.OrganizationId);
        modelBuilder.Entity<backend.Entities.WorkflowExecution>().HasIndex(e => e.OrganizationId);
        modelBuilder.Entity<backend.Entities.ExecutionLog>().HasIndex(l => l.OrganizationId);
        modelBuilder.Entity<Schedule>().HasIndex(s => s.OrganizationId);
        modelBuilder.Entity<WorkflowVersion>().HasIndex(v => v.OrganizationId);

        // Day 23 - Communication Hub Constraints & Indexes
        modelBuilder.Entity<backend.Modules.CommunicationHub.Entities.Conversation>().HasIndex(c => c.OrganizationId);
        modelBuilder.Entity<backend.Modules.CommunicationHub.Entities.Conversation>().HasIndex(c => new { c.OrganizationId, c.Status });
        modelBuilder.Entity<backend.Modules.CommunicationHub.Entities.Message>().HasIndex(m => m.ConversationId);
        modelBuilder.Entity<backend.Modules.CommunicationHub.Entities.CommunicationChannel>().HasIndex(c => c.OrganizationId);
        modelBuilder.Entity<backend.Modules.CommunicationHub.Entities.MessageTemplate>().HasIndex(t => t.OrganizationId);
        modelBuilder.Entity<backend.Modules.CommunicationHub.Entities.Assignment>().HasIndex(a => a.ConversationId);
        modelBuilder.Entity<backend.Modules.CommunicationHub.Entities.CommunicationNotification>().HasIndex(n => new { n.OrganizationId, n.TargetUserId });
        modelBuilder.Entity<backend.Modules.CommunicationHub.Entities.ConversationTag>().HasIndex(t => t.OrganizationId);
        modelBuilder.Entity<backend.Modules.CommunicationHub.Entities.ConversationStatusEntity>().HasIndex(s => s.OrganizationId);
        modelBuilder.Entity<backend.Modules.CommunicationHub.Entities.CommunicationMetric>().HasIndex(m => m.OrganizationId);

        // Day 24 - Customer Feedback Indexes
        modelBuilder.Entity<backend.Modules.CustomerFeedback.Entities.Feedback>().HasIndex(e => new { e.OrganizationId, e.IsDeleted, e.Status });
        modelBuilder.Entity<backend.Modules.CustomerFeedback.Entities.Survey>().HasIndex(e => new { e.OrganizationId, e.IsActive });
        modelBuilder.Entity<backend.Modules.CustomerFeedback.Entities.SurveyQuestion>().HasIndex(e => e.SurveyId);
        modelBuilder.Entity<backend.Modules.CustomerFeedback.Entities.SurveyResponse>().HasIndex(e => new { e.OrganizationId, e.SurveyId });
        modelBuilder.Entity<backend.Modules.CustomerFeedback.Entities.Rating>().HasIndex(e => new { e.OrganizationId, e.EntityType, e.EntityId });
        modelBuilder.Entity<backend.Modules.CustomerFeedback.Entities.SentimentAnalysis>().HasIndex(e => new { e.OrganizationId, e.TargetEntityType, e.TargetEntityId });
        modelBuilder.Entity<backend.Modules.CustomerFeedback.Entities.CustomerSatisfactionScore>().HasIndex(e => new { e.OrganizationId, e.CustomerId });
        modelBuilder.Entity<backend.Modules.CustomerFeedback.Entities.ServiceMetric>().HasIndex(e => new { e.OrganizationId, e.MetricDate });
        modelBuilder.Entity<backend.Modules.CustomerFeedback.Entities.ImprovementRecommendation>().HasIndex(e => new { e.OrganizationId, e.Priority, e.Status });

        // Day 25 - AI Growth Intelligence Indexes
        modelBuilder.Entity<backend.Modules.BusinessPerformance.Entities.BusinessMetric>().HasIndex(e => new { e.OrganizationId, e.MetricType, e.Period });
        modelBuilder.Entity<backend.Modules.RevenueAnalytics.Entities.RevenueSnapshot>().HasIndex(e => new { e.OrganizationId, e.SnapshotDate });
        modelBuilder.Entity<backend.Modules.Profitability.Entities.ProfitSnapshot>().HasIndex(e => new { e.OrganizationId, e.SnapshotDate });
        modelBuilder.Entity<backend.Modules.ProductAnalytics.Entities.ProductPerformance>().HasIndex(e => new { e.OrganizationId, e.Period, e.IsTopPerformer });
        modelBuilder.Entity<backend.Modules.CustomerAnalytics.Entities.CustomerPerformance>().HasIndex(e => new { e.OrganizationId, e.CustomerSegment, e.ChurnRiskScore });
        modelBuilder.Entity<backend.Modules.MarketingROI.Entities.MarketingPerformance>().HasIndex(e => new { e.OrganizationId, e.Channel, e.Period });
        modelBuilder.Entity<backend.Modules.GrowthRecommendations.Entities.GrowthRecommendation>().HasIndex(e => new { e.OrganizationId, e.Priority, e.Status });
        modelBuilder.Entity<backend.Modules.Benchmarking.Entities.BenchmarkMetric>().HasIndex(e => new { e.OrganizationId, e.ComparisonType, e.MetricName });

        // Day 26 - Multi-Branch & Multi-Location Management Platform
        modelBuilder.Entity<backend.Modules.Locations.Entities.Location>().HasIndex(e => new { e.OrganizationId, e.IsDeleted });
        modelBuilder.Entity<backend.Modules.Branches.Entities.Branch>().HasIndex(e => new { e.OrganizationId, e.IsDeleted, e.Code });
        modelBuilder.Entity<backend.Modules.Branches.Entities.BranchManager>().HasIndex(e => new { e.OrganizationId, e.BranchId, e.UserId });
        modelBuilder.Entity<backend.Modules.Warehouses.Entities.Warehouse>().ToTable("BranchWarehouses").HasIndex(e => new { e.OrganizationId, e.BranchId, e.IsDeleted });
        modelBuilder.Entity<backend.Modules.Transfers.Entities.WarehouseTransfer>().HasIndex(e => new { e.OrganizationId, e.SourceWarehouseId, e.DestinationWarehouseId });
        modelBuilder.Entity<backend.Modules.BranchAnalytics.Entities.BranchPerformance>().HasIndex(e => new { e.OrganizationId, e.BranchId, e.Year, e.Month });
        modelBuilder.Entity<backend.Modules.RegionalReports.Entities.RegionalSummary>().HasIndex(e => new { e.OrganizationId, e.Region, e.Year, e.Month });

        // Day 28 - AI Automation & Workflow Engine Constraints
        modelBuilder.Entity<backend.Modules.Automation.Domain.Entities.AiWorkflow>()
            .ToTable("AiEngine_Workflows")
            .HasIndex(w => w.OrganizationId);
        
        modelBuilder.Entity<backend.Modules.Automation.Domain.Entities.AiWorkflow>()
            .HasIndex(w => w.Status);
        
        modelBuilder.Entity<backend.Modules.Automation.Domain.Entities.AiWorkflow>()
            .HasIndex(w => w.TriggerType);
            
        modelBuilder.Entity<backend.Modules.Automation.Domain.Entities.WorkflowStep>()
            .ToTable("AiEngine_WorkflowSteps")
            .HasIndex(ws => ws.WorkflowId);

        modelBuilder.Entity<backend.Modules.Automation.Domain.Entities.WorkflowExecution>()
            .ToTable("AiEngine_WorkflowExecutions")
            .HasIndex(e => e.OrganizationId);
            
        modelBuilder.Entity<backend.Modules.Automation.Domain.Entities.WorkflowExecution>()
            .HasIndex(e => e.WorkflowId);

        modelBuilder.Entity<backend.Modules.Automation.Domain.Entities.WorkflowExecutionStep>()
            .ToTable("AiEngine_WorkflowExecutionSteps")
            .HasIndex(es => es.ExecutionId);

        modelBuilder.Entity<backend.Modules.Automation.Domain.Entities.WorkflowTemplate>()
            .ToTable("AiEngine_WorkflowTemplates");

        // Day 29 - Business Intelligence & Alerts
        modelBuilder.Entity<BusinessBriefing>().HasIndex(b => b.OrganizationId);
        modelBuilder.Entity<BusinessBriefing>().HasIndex(b => new { b.OrganizationId, b.Date }).IsUnique();
        
        modelBuilder.Entity<BriefingItem>().HasIndex(b => b.BriefingId);
        modelBuilder.Entity<BriefingItem>().HasIndex(b => b.Priority);
        
        modelBuilder.Entity<ProactiveAlert>().HasIndex(a => a.OrganizationId);
        modelBuilder.Entity<ProactiveAlert>().HasIndex(a => new { a.OrganizationId, a.Status });
        modelBuilder.Entity<ProactiveAlert>().HasIndex(a => a.DeduplicationKey);

        // Day 31 - AI Business Memory & Context Engine Indexes
        modelBuilder.Entity<BusinessMemory>().HasIndex(m => m.OrganizationId);
        modelBuilder.Entity<BusinessMemory>().HasIndex(m => new { m.OrganizationId, m.MemoryType, m.IsActive });
        modelBuilder.Entity<BusinessMemory>().HasIndex(m => new { m.OrganizationId, m.SourceModule, m.SourceEntityId });
        modelBuilder.Entity<BusinessMemory>().HasIndex(m => m.ExpiresAt);

        // Day 32 - AI Outcomes
        modelBuilder.Entity<BusinessOutcome>().HasIndex(o => o.BusinessId);
        modelBuilder.Entity<BusinessOutcome>().HasIndex(o => new { o.BusinessId, o.OutcomeType });
        modelBuilder.Entity<BusinessOutcome>().HasIndex(o => new { o.BusinessId, o.SourceType, o.SourceId });
        modelBuilder.Entity<BusinessOutcome>().HasIndex(o => new { o.BusinessId, o.SourceType, o.SourceId, o.OutcomeType }).IsUnique();
        modelBuilder.Entity<BusinessOutcome>().HasIndex(o => o.OccurredAt);
        modelBuilder.Entity<BusinessOutcome>().HasIndex(o => o.Confidence);
        modelBuilder.Entity<BusinessOutcome>().HasIndex(o => o.Status);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }

    public override int SaveChanges()
    {
        HandleSaveChanges();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        HandleSaveChanges();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void HandleSaveChanges()
    {
        var entries = ChangeTracker.Entries<BaseEntity>();

        foreach (var entry in entries)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    // CreatedBy could be populated from a scoped service holding the CurrentUser context
                    break;

                case EntityState.Modified:
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    break;

                case EntityState.Deleted:
                    // Soft delete logic
                    entry.State = EntityState.Modified;
                    entry.Entity.IsDeleted = true;
                    entry.Entity.DeletedAt = DateTime.UtcNow;
                    break;
            }
        }
    }
}
