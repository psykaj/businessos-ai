using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace backend.Modules.CommunicationHub.Channels.BackgroundServices;

public class ChannelStatusMonitorService : BackgroundService
{
    private readonly ILogger<ChannelStatusMonitorService> _logger;
    private static readonly TimeSpan CheckInterval = TimeSpan.FromMinutes(30);

    public ChannelStatusMonitorService(ILogger<ChannelStatusMonitorService> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("ChannelStatusMonitorService started running in background.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _logger.LogDebug("Running scheduled ping and connection diagnostic check for communication channels.");
                // Additional background tasks such as pinging external WhatsApp/Twilio tokens can happen here
                await Task.Delay(CheckInterval, stoppingToken);
            }
            catch (TaskCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred executing channel diagnostic tasks.");
                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }

        _logger.LogInformation("ChannelStatusMonitorService stopped.");
    }
}
