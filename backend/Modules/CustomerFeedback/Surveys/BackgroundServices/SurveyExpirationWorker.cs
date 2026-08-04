using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace backend.Modules.CustomerFeedback.Surveys.BackgroundServices;

public class SurveyExpirationWorker : BackgroundService
{
    private readonly ILogger<SurveyExpirationWorker> _logger;
    private static readonly TimeSpan Interval = TimeSpan.FromHours(6);

    public SurveyExpirationWorker(ILogger<SurveyExpirationWorker> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("SurveyExpirationWorker started.");
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogDebug("Running automated expired survey completion audit.");
            await Task.Delay(Interval, stoppingToken);
        }
    }
}
