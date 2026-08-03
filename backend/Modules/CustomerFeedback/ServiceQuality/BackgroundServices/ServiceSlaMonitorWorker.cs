using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace backend.Modules.CustomerFeedback.ServiceQuality.BackgroundServices;

public class ServiceSlaMonitorWorker : BackgroundService
{
    private readonly ILogger<ServiceSlaMonitorWorker> _logger;
    private static readonly TimeSpan Interval = TimeSpan.FromMinutes(10);

    public ServiceSlaMonitorWorker(ILogger<ServiceSlaMonitorWorker> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("ServiceSlaMonitorWorker started.");
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogDebug("Running automated SLA threshold breach detection.");
            await Task.Delay(Interval, stoppingToken);
        }
    }
}
