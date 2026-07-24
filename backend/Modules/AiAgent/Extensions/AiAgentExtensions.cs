using backend.Modules.AiAgent.CommandEngine.Interfaces;
using backend.Modules.AiAgent.CommandEngine.Services;
using backend.Modules.AiAgent.Context.Interfaces;
using backend.Modules.AiAgent.Context.Services;
using backend.Modules.AiAgent.Conversation.Interfaces;
using backend.Modules.AiAgent.Conversation.Services;
using backend.Modules.AiAgent.Executions.Interfaces;
using backend.Modules.AiAgent.Executions.Services;
using backend.Modules.AiAgent.Permissions.Interfaces;
using backend.Modules.AiAgent.Permissions.Services;
using backend.Modules.AiAgent.Recommendations.Interfaces;
using backend.Modules.AiAgent.Recommendations.Services;
using backend.Modules.AiAgent.Repositories;
using backend.Modules.AiAgent.ToolRegistry.Interfaces;
using backend.Modules.AiAgent.ToolRegistry.Tools;
using Microsoft.Extensions.DependencyInjection;

namespace backend.Modules.AiAgent.Extensions;

public static class AiAgentExtensions
{
    public static IServiceCollection AddAiAgentModule(this IServiceCollection services)
    {
        // Repositories
        services.AddScoped<IAiConversationRepository, AiConversationRepository>();
        services.AddScoped<IMessageRepository, MessageRepository>();
        services.AddScoped<ICommandExecutionRepository, CommandExecutionRepository>();
        services.AddScoped<IToolRepository, ToolRepository>();
        services.AddScoped<IRecommendationRepository, RecommendationRepository>();

        // Tools
        services.AddScoped<ITool, CrmTool>();
        services.AddScoped<ITool, InvoiceTool>();
        services.AddScoped<ITool, QRTool>();
        services.AddScoped<ITool, EmailTool>();
        services.AddScoped<ITool, WhatsAppTool>();
        services.AddScoped<ITool, ReportTool>();
        services.AddScoped<ITool, WorkflowTool>();
        services.AddScoped<ITool, AnalyticsTool>();
        services.AddScoped<ITool, CustomerTool>();

        // Tool Registry
        services.AddScoped<IToolRegistry, backend.Modules.AiAgent.ToolRegistry.Services.ToolRegistry>();

        // Engines & Services
        services.AddScoped<IAiContextEngine, AiContextEngine>();
        services.AddScoped<IAiPermissionSafetyService, AiPermissionSafetyService>();
        services.AddScoped<IAiCommandEngine, AiCommandEngine>();
        services.AddScoped<IAiExecutionEngine, AiExecutionEngine>();
        services.AddScoped<IAiRecommendationEngine, AiRecommendationEngine>();
        services.AddScoped<IAiConversationService, AiConversationService>();

        return services;
    }
}
