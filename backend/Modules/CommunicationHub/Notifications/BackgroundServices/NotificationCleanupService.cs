using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace backend.Modules.CommunicationHub.Notifications.BackgroundServices;

public class NotificationCleanupService : BackgroundService
{
    private readonly ILogger<NotificationCleanupService> _logger;
    private static readonly TimeSpan Interval = TimeSpan.FromDays(1);

    public NotificationCleanupService(ILogger<NotificationCleanupService> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("NotificationCleanupService started running in background.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _logger.LogDebug("Purging resolved and archived notification records older than 30 days.");
                await Task.Delay(Interval, stoppingToken);
            }
            catch (TaskCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred running notification archival cleanup.");
                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }

        _logger.LogInformation("NotificationCleanupService stopped.");
    }
}
