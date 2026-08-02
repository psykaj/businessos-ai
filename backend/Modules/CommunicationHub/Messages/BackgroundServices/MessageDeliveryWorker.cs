using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace backend.Modules.CommunicationHub.Messages.BackgroundServices;

public class MessageDeliveryWorker : BackgroundService
{
    private readonly ILogger<MessageDeliveryWorker> _logger;
    private static readonly TimeSpan Interval = TimeSpan.FromSeconds(30);

    public MessageDeliveryWorker(ILogger<MessageDeliveryWorker> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("MessageDeliveryWorker background queue started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // Here retry logic for failed outbound messages or queue flushing can be processed
                await Task.Delay(Interval, stoppingToken);
            }
            catch (TaskCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing message retry worker queue.");
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }

        _logger.LogInformation("MessageDeliveryWorker stopped.");
    }
}
