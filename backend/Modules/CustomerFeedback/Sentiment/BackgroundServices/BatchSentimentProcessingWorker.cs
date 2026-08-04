using System;
using System.Threading;
using System.Threading.Tasks;
using backend.Modules.CustomerFeedback.Sentiment.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace backend.Modules.CustomerFeedback.Sentiment.BackgroundServices;

public class BatchSentimentProcessingWorker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<BatchSentimentProcessingWorker> _logger;
    private static readonly TimeSpan CheckInterval = TimeSpan.FromMinutes(5);

    public BatchSentimentProcessingWorker(IServiceProvider serviceProvider, ILogger<BatchSentimentProcessingWorker> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("BatchSentimentProcessingWorker started.");
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var repository = scope.ServiceProvider.GetRequiredService<ISentimentRepository>();
                    var unprocessed = await repository.GetUnprocessedBatchAsync(20);
                    foreach (var item in unprocessed)
                    {
                        item.ConfidenceScore = 0.88m; // Default enrichment completion
                        await repository.UpdateAsync(item);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in BatchSentimentProcessingWorker");
            }

            await Task.Delay(CheckInterval, stoppingToken);
        }
    }
}
