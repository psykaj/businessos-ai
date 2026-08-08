using Microsoft.Extensions.DependencyInjection;
using backend.Modules.Automation.Application.Interfaces;
using backend.Modules.Automation.Infrastructure.Services;

namespace backend.Modules.Automation.Extensions;

public static class AutomationEngineExtensions
{
    public static IServiceCollection AddAutomationEngine(this IServiceCollection services)
    {
        services.AddScoped<IWorkflowTriggerService, WorkflowTriggerService>();
        services.AddScoped<IWorkflowEngine, WorkflowEngine>();
        services.AddScoped<IWorkflowActionExecutor, WorkflowActionExecutor>();
        services.AddScoped<IWorkflowConditionEvaluator, WorkflowConditionEvaluator>();
        services.AddScoped<IAIDecisionService, AIDecisionService>();
        services.AddScoped<IWorkflowApprovalService, WorkflowApprovalService>();
        services.AddScoped<IWorkflowExecutionLogger, WorkflowExecutionLogger>();

        return services;
    }
}
