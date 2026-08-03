using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace backend.Modules.CustomerFeedback.Ratings.BackgroundServices;

public class RatingAggregationWorker : BackgroundService
{
    private readonly ILogger<RatingAggregationWorker> _logger;
    private static readonly TimeSpan Interval = TimeSpan.FromMinutes(30);

    public RatingAggregationWorker(ILogger<RatingAggregationWorker> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("RatingAggregationWorker started.");
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogDebug("Running scheduled rating aggregation cache maintenance.");
            await Task.Delay(Interval, stoppingToken);
        }
    }
}
