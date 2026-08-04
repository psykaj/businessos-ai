using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace backend.Modules.CustomerFeedback.ImprovementActions.BackgroundServices;

public class RecommendationDiscoveryWorker : BackgroundService
{
    private readonly ILogger<RecommendationDiscoveryWorker> _logger;
    private static readonly TimeSpan Interval = TimeSpan.FromHours(12);

    public RecommendationDiscoveryWorker(ILogger<RecommendationDiscoveryWorker> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("RecommendationDiscoveryWorker started.");
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogDebug("Running automated AI discovery for retention improvement opportunities.");
            await Task.Delay(Interval, stoppingToken);
        }
    }
}
