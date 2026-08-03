using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace backend.Modules.CustomerFeedback.FeedbackAnalytics.BackgroundServices;

public class FeedbackTrendAggregationWorker : BackgroundService
{
    private readonly ILogger<FeedbackTrendAggregationWorker> _logger;
    private static readonly TimeSpan Interval = TimeSpan.FromHours(4);

    public FeedbackTrendAggregationWorker(ILogger<FeedbackTrendAggregationWorker> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("FeedbackTrendAggregationWorker started.");
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogDebug("Running scheduled aggregation of sentiment volume trends.");
            await Task.Delay(Interval, stoppingToken);
        }
    }
}
