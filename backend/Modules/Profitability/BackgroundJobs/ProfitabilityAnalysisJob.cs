using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using backend.Modules.Profitability.Interfaces;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace backend.Modules.Profitability.BackgroundJobs;

public class ProfitabilityAnalysisJob : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<ProfitabilityAnalysisJob> _logger;
    private static readonly TimeSpan Interval = TimeSpan.FromHours(24);

    public ProfitabilityAnalysisJob(IServiceScopeFactory scopeFactory, ILogger<ProfitabilityAnalysisJob> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("ProfitabilityAnalysisJob worker started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var service = scope.ServiceProvider.GetRequiredService<IProfitabilityService>();
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                var orgIds = await db.Organizations.AsNoTracking().Where(o => !o.IsDeleted).Select(o => o.Id).ToListAsync(stoppingToken);
                foreach (var orgId in orgIds)
                {
                    await service.GetAnalysisAsync(orgId); // triggers background analysis and seeding
                }

                await Task.Delay(Interval, stoppingToken);
            }
            catch (TaskCanceledException) { break; }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing ProfitabilityAnalysisJob.");
                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }
    }
}
