using Microsoft.Extensions.DependencyInjection;
using backend.Modules.AutomationStudio.Triggers.Interfaces;
using backend.Modules.AutomationStudio.Triggers.Services;
using backend.Modules.AutomationStudio.Conditions.Interfaces;
using backend.Modules.AutomationStudio.Conditions.Services;
using backend.Modules.AutomationStudio.Actions.Interfaces;
using backend.Modules.AutomationStudio.Actions.Services;
using backend.Modules.AutomationStudio.ExecutionEngine.Interfaces;
using backend.Modules.AutomationStudio.ExecutionEngine.Services;
using backend.Modules.AutomationStudio.Schedules.Services;

namespace backend.Modules.AutomationStudio.Extensions;

public static class AutomationStudioExtensions
{
    public static IServiceCollection AddAutomationStudio(this IServiceCollection services)
    {
        services.AddScoped<ITriggerService, TriggerService>();
        services.AddScoped<IConditionService, ConditionService>();
        services.AddScoped<IActionService, ActionService>();
        services.AddScoped<IWorkflowExecutionEngine, WorkflowExecutionEngine>();
        services.AddHostedService<ScheduleWorker>();

        return services;
    }
}
