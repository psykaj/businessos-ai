using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using backend.Modules.BranchAnalytics.Interfaces;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.BranchAnalytics.BackgroundJobs;

public class BranchPerformanceWorkerJob : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<BranchPerformanceWorkerJob> _logger;
    private readonly TimeSpan _checkInterval = TimeSpan.FromHours(6);

    public BranchPerformanceWorkerJob(IServiceProvider serviceProvider, ILogger<BranchPerformanceWorkerJob> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Branch Performance Calculation Worker Job started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var engineService = scope.ServiceProvider.GetRequiredService<IBranchPerformanceEngineService>();

                var orgIds = await dbContext.Organizations
                    .AsNoTracking()
                    .Select(o => o.Id)
                    .ToListAsync(stoppingToken);

                int currentYear = DateTime.UtcNow.Year;
                int currentMonth = DateTime.UtcNow.Month;

                foreach (var orgId in orgIds)
                {
                    await engineService.CalculatePerformanceAsync(orgId, currentYear, currentMonth, stoppingToken);
                }

                _logger.LogInformation("Branch Performance calculation completed for {Count} tenants.", orgIds.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while running Branch Performance calculation worker.");
            }

            await Task.Delay(_checkInterval, stoppingToken);
        }
    }
}
