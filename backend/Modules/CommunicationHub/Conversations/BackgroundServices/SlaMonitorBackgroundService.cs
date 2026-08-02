using System;
using System.Threading;
using System.Threading.Tasks;
using backend.Modules.CommunicationHub.Conversations.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace backend.Modules.CommunicationHub.Conversations.BackgroundServices;

public class SlaMonitorBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<SlaMonitorBackgroundService> _logger;
    private static readonly TimeSpan Interval = TimeSpan.FromMinutes(10);

    public SlaMonitorBackgroundService(IServiceProvider serviceProvider, ILogger<SlaMonitorBackgroundService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("SlaMonitorBackgroundService started running in background.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var conversationService = scope.ServiceProvider.GetRequiredService<IConversationService>();
                await conversationService.CheckAndMarkSlaBreachesAsync();

                await Task.Delay(Interval, stoppingToken);
            }
            catch (TaskCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing SLA monitoring scan.");
                await Task.Delay(TimeSpan.FromMinutes(2), stoppingToken);
            }
        }

        _logger.LogInformation("SlaMonitorBackgroundService stopped.");
    }
}
