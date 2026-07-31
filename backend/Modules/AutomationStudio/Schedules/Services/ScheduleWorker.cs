using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using backend.Persistence;
using backend.Modules.AutomationStudio.ExecutionEngine.Interfaces;

namespace backend.Modules.AutomationStudio.Schedules.Services;

public class ScheduleWorker : BackgroundService
{
    private readonly ILogger<ScheduleWorker> _logger;
    private readonly IServiceProvider _serviceProvider;

    public ScheduleWorker(ILogger<ScheduleWorker> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("AutomationStudio ScheduleWorker started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var executionEngine = scope.ServiceProvider.GetRequiredService<IWorkflowExecutionEngine>();

                var now = DateTime.UtcNow;

                var dueSchedules = await dbContext.Schedules
                    .Include(s => s.Workflow)
                    .Where(s => s.IsActive && s.Workflow.IsActive && !s.IsDeleted && !s.Workflow.IsDeleted)
                    .Where(s => s.NextRunAt == null || s.NextRunAt <= now)
                    .ToListAsync(stoppingToken);

                foreach (var schedule in dueSchedules)
                {
                    _logger.LogInformation($"Triggering workflow {schedule.WorkflowId} from schedule {schedule.Id}");
                    
                    _ = Task.Run(() => executionEngine.ExecuteWorkflowAsync(schedule.WorkflowId, "{\"trigger\": \"schedule\"}"));

                    // Simple mock for NextRunAt. In reality, parse CronExpression.
                    schedule.LastRunAt = now;
                    schedule.NextRunAt = now.AddMinutes(5); // mock 5 min recurring
                }

                if (dueSchedules.Any())
                {
                    await dbContext.SaveChangesAsync(stoppingToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing schedules.");
            }

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }
}
