using System;
using System.Threading;
using System.Threading.Tasks;
using backend.Modules.KpiEngine.Repositories;
using backend.Modules.KpiEngine.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace backend.Modules.KpiEngine.Jobs;

public class KpiCalculationBackgroundJob : BackgroundService
{
    private readonly ILogger<KpiCalculationBackgroundJob> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly TimeSpan _period = TimeSpan.FromHours(1);

    public KpiCalculationBackgroundJob(ILogger<KpiCalculationBackgroundJob> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Add a delay at startup so it doesn't block app launch
        await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
        
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _logger.LogInformation("Starting background KPI calculation run...");
                using var scope = _serviceProvider.CreateScope();
                var kpiRepository = scope.ServiceProvider.GetRequiredService<IKpiRepository>();
                var calculationService = scope.ServiceProvider.GetRequiredService<IKpiCalculationService>();

                var allKpis = await kpiRepository.GetAllActiveAsync();
                
                foreach (var kpi in allKpis)
                {
                    try
                    {
                        await calculationService.CalculateKpiAsync(kpi.OrganizationId, kpi.Id);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to calculate KPI {KpiId}", kpi.Id);
                    }
                }

                _logger.LogInformation("Completed background KPI calculation run.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred executing KPI background job.");
            }

            await Task.Delay(_period, stoppingToken);
        }
    }
}
