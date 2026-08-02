using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace backend.Modules.CommunicationHub.Assignments.BackgroundServices;

public class AssignmentHandoffWorker : BackgroundService
{
    private readonly ILogger<AssignmentHandoffWorker> _logger;
    private static readonly TimeSpan Interval = TimeSpan.FromHours(1);

    public AssignmentHandoffWorker(ILogger<AssignmentHandoffWorker> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("AssignmentHandoffWorker started running in background.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _logger.LogDebug("Evaluating unacknowledged agent assignments for automated queue redirection.");
                await Task.Delay(Interval, stoppingToken);
            }
            catch (TaskCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing assignment handoff check.");
                await Task.Delay(TimeSpan.FromMinutes(10), stoppingToken);
            }
        }

        _logger.LogInformation("AssignmentHandoffWorker stopped.");
    }
}
