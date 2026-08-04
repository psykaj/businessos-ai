using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using backend.Modules.BusinessPerformance.Interfaces;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace backend.Modules.BusinessPerformance.BackgroundJobs;

public class BusinessPerformanceCalculationJob : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<BusinessPerformanceCalculationJob> _logger;
    private static readonly TimeSpan Interval = TimeSpan.FromHours(12);

    public BusinessPerformanceCalculationJob(IServiceScopeFactory scopeFactory, ILogger<BusinessPerformanceCalculationJob> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("BusinessPerformanceCalculationJob started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _logger.LogDebug("Running scheduled Business Performance Engine metric calculations.");
                using (var scope = _scopeFactory.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    var service = scope.ServiceProvider.GetRequiredService<IBusinessPerformanceService>();

                    var orgIds = await db.Organizations
                        .AsNoTracking()
                        .Where(o => !o.IsDeleted)
                        .Select(o => o.Id)
                        .Take(50)
                        .ToListAsync(stoppingToken);

                    foreach (var orgId in orgIds)
                    {
                        try
                        {
                            await service.CalculateAndRefreshEngineAsync(orgId);
                        }
                        catch (Exception orgEx)
                        {
                            _logger.LogError(orgEx, "Error calculating business performance for Org {OrgId}", orgId);
                        }
                    }
                }

                await Task.Delay(Interval, stoppingToken);
            }
            catch (TaskCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandley error in BusinessPerformanceCalculationJob.");
                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }

        _logger.LogInformation("BusinessPerformanceCalculationJob stopped.");
    }
}
