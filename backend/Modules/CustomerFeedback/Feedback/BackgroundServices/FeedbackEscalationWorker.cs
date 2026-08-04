using System;
using System.Threading;
using System.Threading.Tasks;
using backend.Modules.CustomerFeedback.Feedback.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace backend.Modules.CustomerFeedback.Feedback.BackgroundServices;

public class FeedbackEscalationWorker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<FeedbackEscalationWorker> _logger;
    private static readonly TimeSpan CheckInterval = TimeSpan.FromMinutes(15);

    public FeedbackEscalationWorker(IServiceProvider serviceProvider, ILogger<FeedbackEscalationWorker> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("FeedbackEscalationWorker started.");
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var repository = scope.ServiceProvider.GetRequiredService<IFeedbackRepository>();
                    var urgentUnassigned = await repository.GetUnassignedUrgentAsync(50);
                    foreach (var feedback in urgentUnassigned)
                    {
                        // Auto-flag or add escalation notification tag to ensure SLA adherence
                        feedback.Tags = feedback.Tags.Contains("Escalated") ? feedback.Tags : $"{feedback.Tags},Escalated";
                        await repository.UpdateAsync(feedback);
                        _logger.LogWarning("Escalating unassigned high-urgency feedback ID {FeedbackId} for org {OrgId}", feedback.Id, feedback.OrganizationId);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in FeedbackEscalationWorker");
            }

            await Task.Delay(CheckInterval, stoppingToken);
        }
    }
}
