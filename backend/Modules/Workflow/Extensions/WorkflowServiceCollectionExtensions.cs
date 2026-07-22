using backend.Modules.Workflow.Actions;
using backend.Modules.Workflow.Actions.Handlers;
using backend.Modules.Workflow.Conditions;
using backend.Modules.Workflow.Executions;
using backend.Modules.Workflow.Integrations;
using backend.Modules.Workflow.Integrations.Providers;
using backend.Modules.Workflow.Interfaces;
using backend.Modules.Workflow.Repositories;
using backend.Modules.Workflow.Services;
using backend.Modules.Workflow.Triggers;
using backend.Modules.Workflow.Triggers.Handlers;
using backend.Modules.Workflow.Variables;
using Microsoft.Extensions.DependencyInjection;

namespace backend.Modules.Workflow.Extensions;

public static class WorkflowServiceCollectionExtensions
{
    public static IServiceCollection AddWorkflowModule(this IServiceCollection services)
    {
        // Core Security & Utilities
        services.AddSingleton<IEncryptionService, EncryptionService>();
        services.AddSingleton<IVariableResolver, VariableResolver>();
        services.AddScoped<IConditionEvaluator, ConditionEvaluator>();

        // Repositories
        services.AddScoped<IWorkflowRepository, WorkflowRepository>();
        services.AddScoped<ITriggerRepository, TriggerRepository>();
        services.AddScoped<IActionRepository, ActionRepository>();
        services.AddScoped<IExecutionRepository, ExecutionRepository>();
        services.AddScoped<IIntegrationRepository, IntegrationRepository>();
        services.AddScoped<ILogRepository, LogRepository>();

        // Triggers Handlers & Registry
        services.AddScoped<ITriggerHandler, LeadCreatedTriggerHandler>();
        services.AddScoped<ITriggerHandler, LeadUpdatedTriggerHandler>();
        services.AddScoped<ITriggerHandler, LeadQualifiedTriggerHandler>();
        services.AddScoped<ITriggerHandler, QRCodeScannedTriggerHandler>();
        services.AddScoped<ITriggerHandler, CustomerRegisteredTriggerHandler>();
        services.AddScoped<ITriggerHandler, InvoicePaidTriggerHandler>();
        services.AddScoped<ITriggerHandler, PaymentFailedTriggerHandler>();
        services.AddScoped<ITriggerHandler, SubscriptionExpiringTriggerHandler>();
        services.AddScoped<ITriggerHandler, FormSubmittedTriggerHandler>();
        services.AddScoped<ITriggerHandler, CampaignCompletedTriggerHandler>();
        services.AddScoped<ITriggerHandler, TeamMemberAddedTriggerHandler>();
        services.AddScoped<ITriggerHandler, ScheduledTriggerHandler>();
        services.AddScoped<ITriggerHandler, ManualTriggerHandler>();
        services.AddScoped<TriggerRegistry>();

        // Action Handlers & Registry
        services.AddScoped<IActionHandler, CreateCrmLeadActionHandler>();
        services.AddScoped<IActionHandler, UpdateCrmActionHandler>();
        services.AddScoped<IActionHandler, AssignSalespersonActionHandler>();
        services.AddScoped<IActionHandler, SendEmailActionHandler>();
        services.AddScoped<IActionHandler, SendWhatsAppActionHandler>();
        services.AddScoped<IActionHandler, SendNotificationActionHandler>();
        services.AddScoped<IActionHandler, GenerateInvoiceActionHandler>();
        services.AddScoped<IActionHandler, GenerateQRActionHandler>();
        services.AddScoped<IActionHandler, CallAiAssistantActionHandler>();
        services.AddScoped<IActionHandler, CallWebhookActionHandler>();
        services.AddScoped<IActionHandler, DelayExecutionActionHandler>();
        services.AddScoped<IActionHandler, UpdateCustomerJourneyActionHandler>();
        services.AddScoped<IActionHandler, LogActivityActionHandler>();
        services.AddScoped<ActionRegistry>();

        // Integration Providers & Registry
        services.AddScoped<IIntegrationProvider, GoogleSheetsProvider>();
        services.AddScoped<IIntegrationProvider, SlackProvider>();
        services.AddScoped<IIntegrationProvider, MicrosoftTeamsProvider>();
        services.AddScoped<IIntegrationProvider, DiscordProvider>();
        services.AddScoped<IIntegrationProvider, GoogleCalendarProvider>();
        services.AddScoped<IIntegrationProvider, OutlookCalendarProvider>();
        services.AddScoped<IIntegrationProvider, ResendProvider>();
        services.AddScoped<IIntegrationProvider, TwilioProvider>();
        services.AddScoped<IIntegrationProvider, StripeProvider>();
        services.AddScoped<IIntegrationProvider, RazorpayProvider>();
        services.AddScoped<IIntegrationProvider, WebhookProvider>();
        services.AddScoped<IIntegrationProvider, RestApiProvider>();
        services.AddScoped<IntegrationRegistry>();

        // Engines & Services
        services.AddScoped<IWorkflowExecutionEngine, WorkflowExecutionEngine>();
        services.AddScoped<IWorkflowTriggerDispatcher, WorkflowTriggerDispatcher>();
        services.AddScoped<IWorkflowService, WorkflowService>();
        services.AddScoped<IIntegrationService, IntegrationService>();
        services.AddScoped<IWorkflowLogService, WorkflowLogService>();

        return services;
    }
}
