using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace backend.Modules.CustomerFeedback.CustomerSatisfaction.BackgroundServices;

public class CsatCalculationWorker : BackgroundService
{
    private readonly ILogger<CsatCalculationWorker> _logger;
    private static readonly TimeSpan Interval = TimeSpan.FromHours(12);

    public CsatCalculationWorker(ILogger<CsatCalculationWorker> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("CsatCalculationWorker started.");
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogDebug("Running automated background CSAT and NPS score re-computations.");
            await Task.Delay(Interval, stoppingToken);
        }
    }
}
