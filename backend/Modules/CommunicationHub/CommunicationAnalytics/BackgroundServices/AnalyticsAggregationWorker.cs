using System;
using System.Threading;
using System.Threading.Tasks;
using backend.Modules.CommunicationHub.CommunicationAnalytics.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace backend.Modules.CommunicationHub.CommunicationAnalytics.BackgroundServices;

public class AnalyticsAggregationWorker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<AnalyticsAggregationWorker> _logger;
    private static readonly TimeSpan Interval = TimeSpan.FromHours(24);

    public AnalyticsAggregationWorker(IServiceProvider serviceProvider, ILogger<AnalyticsAggregationWorker> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("AnalyticsAggregationWorker started running in background.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var service = scope.ServiceProvider.GetRequiredService<ICommunicationAnalyticsService>();
                await service.RunDailyAggregationAsync();

                await Task.Delay(Interval, stoppingToken);
            }
            catch (TaskCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred executing analytics aggregation worker.");
                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }

        _logger.LogInformation("AnalyticsAggregationWorker stopped.");
    }
}
