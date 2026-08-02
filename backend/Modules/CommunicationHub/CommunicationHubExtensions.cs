using Microsoft.Extensions.DependencyInjection;
using backend.Modules.CommunicationHub.Assignments.BackgroundServices;
using backend.Modules.CommunicationHub.Assignments.Interfaces;
using backend.Modules.CommunicationHub.Assignments.Repositories;
using backend.Modules.CommunicationHub.Assignments.Services;
using backend.Modules.CommunicationHub.Channels.Adapters;
using backend.Modules.CommunicationHub.Channels.BackgroundServices;
using backend.Modules.CommunicationHub.Channels.Interfaces;
using backend.Modules.CommunicationHub.Channels.Repositories;
using backend.Modules.CommunicationHub.Channels.Services;
using backend.Modules.CommunicationHub.CommunicationAnalytics.BackgroundServices;
using backend.Modules.CommunicationHub.CommunicationAnalytics.Interfaces;
using backend.Modules.CommunicationHub.CommunicationAnalytics.Repositories;
using backend.Modules.CommunicationHub.CommunicationAnalytics.Services;
using backend.Modules.CommunicationHub.Conversations.BackgroundServices;
using backend.Modules.CommunicationHub.Conversations.Interfaces;
using backend.Modules.CommunicationHub.Conversations.Repositories;
using backend.Modules.CommunicationHub.Conversations.Services;
using backend.Modules.CommunicationHub.Inbox.BackgroundServices;
using backend.Modules.CommunicationHub.Inbox.Interfaces;
using backend.Modules.CommunicationHub.Inbox.Repositories;
using backend.Modules.CommunicationHub.Inbox.Services;
using backend.Modules.CommunicationHub.Messages.BackgroundServices;
using backend.Modules.CommunicationHub.Messages.Interfaces;
using backend.Modules.CommunicationHub.Messages.Repositories;
using backend.Modules.CommunicationHub.Messages.Services;
using backend.Modules.CommunicationHub.Notifications.BackgroundServices;
using backend.Modules.CommunicationHub.Notifications.Interfaces;
using backend.Modules.CommunicationHub.Notifications.Repositories;
using backend.Modules.CommunicationHub.Notifications.Services;
using backend.Modules.CommunicationHub.Templates.BackgroundServices;
using backend.Modules.CommunicationHub.Templates.Interfaces;
using backend.Modules.CommunicationHub.Templates.Repositories;
using backend.Modules.CommunicationHub.Templates.Services;

namespace backend.Modules.CommunicationHub;

public static class CommunicationHubExtensions
{
    public static IServiceCollection AddCommunicationHubModule(this IServiceCollection services)
    {
        // ─── Channel Engine & Adapters ─────────────────────────────────────────────
        services.AddSingleton<EmailAdapter>();
        services.AddSingleton<WhatsAppAdapter>();
        services.AddSingleton<SmsAdapter>();
        services.AddSingleton<LiveChatAdapter>();
        services.AddSingleton<FacebookMessengerAdapter>();
        services.AddSingleton<InstagramDmAdapter>();
        services.AddSingleton<IChannelAdapterFactory, ChannelAdapterFactory>();
        
        services.AddScoped<IChannelRepository, ChannelRepository>();
        services.AddScoped<IChannelService, ChannelService>();
        services.AddHostedService<ChannelStatusMonitorService>();

        // ─── Unified Inbox & Messaging Engine ──────────────────────────────────────
        services.AddScoped<IConversationRepository, ConversationRepository>();
        services.AddScoped<IConversationService, ConversationService>();
        services.AddHostedService<SlaMonitorBackgroundService>();

        services.AddScoped<IMessageRepository, MessageRepository>();
        services.AddScoped<IMessageService, MessageService>();
        services.AddHostedService<MessageDeliveryWorker>();

        services.AddScoped<IInboxRepository, InboxRepository>();
        services.AddScoped<IInboxService, InboxService>();
        services.AddHostedService<InboxCleanupBackgroundService>();

        // ─── Templates, Assignments & Notifications ────────────────────────────────
        services.AddScoped<ITemplateRepository, TemplateRepository>();
        services.AddScoped<ITemplateService, TemplateService>();
        services.AddHostedService<TemplateUsageOptimizationWorker>();

        services.AddScoped<IAssignmentRepository, AssignmentRepository>();
        services.AddScoped<IAssignmentService, AssignmentService>();
        services.AddHostedService<AssignmentHandoffWorker>();

        services.AddScoped<ICommunicationNotificationRepository, CommunicationNotificationRepository>();
        services.AddScoped<ICommunicationNotificationService, CommunicationNotificationService>();
        services.AddHostedService<NotificationCleanupService>();

        // ─── Analytics & Caching ───────────────────────────────────────────────────
        services.AddScoped<ICommunicationAnalyticsRepository, CommunicationAnalyticsRepository>();
        services.AddScoped<ICommunicationAnalyticsService, CommunicationAnalyticsService>();
        services.AddHostedService<AnalyticsAggregationWorker>();

        return services;
    }
}
