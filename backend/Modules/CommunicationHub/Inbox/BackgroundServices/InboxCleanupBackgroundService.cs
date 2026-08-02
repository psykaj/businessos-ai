using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace backend.Modules.CommunicationHub.Inbox.BackgroundServices;

public class InboxCleanupBackgroundService : BackgroundService
{
    private readonly ILogger<InboxCleanupBackgroundService> _logger;
    private static readonly TimeSpan Interval = TimeSpan.FromHours(24);

    public InboxCleanupBackgroundService(ILogger<InboxCleanupBackgroundService> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("InboxCleanupBackgroundService started running in background.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _logger.LogDebug("Running daily maintenance to clean up stale temporary inbox attachments and sessions.");
                await Task.Delay(Interval, stoppingToken);
            }
            catch (TaskCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing inbox maintenance task.");
                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }

        _logger.LogInformation("InboxCleanupBackgroundService stopped.");
    }
}
