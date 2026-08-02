using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace backend.Modules.CommunicationHub.Templates.BackgroundServices;

public class TemplateUsageOptimizationWorker : BackgroundService
{
    private readonly ILogger<TemplateUsageOptimizationWorker> _logger;
    private static readonly TimeSpan Interval = TimeSpan.FromDays(7);

    public TemplateUsageOptimizationWorker(ILogger<TemplateUsageOptimizationWorker> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("TemplateUsageOptimizationWorker started running in background.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _logger.LogDebug("Analyzing message template usage frequency to suggest automated quick-replies.");
                await Task.Delay(Interval, stoppingToken);
            }
            catch (TaskCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing template optimization tasks.");
                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }

        _logger.LogInformation("TemplateUsageOptimizationWorker stopped.");
    }
}
