using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using backend.Modules.BusinessIntelligence.Interfaces;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace backend.Modules.BusinessIntelligence.BackgroundServices;

public class BriefingSchedulerBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<BriefingSchedulerBackgroundService> _logger;

    public BriefingSchedulerBackgroundService(
        IServiceProvider serviceProvider,
        ILogger<BriefingSchedulerBackgroundService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("BriefingSchedulerBackgroundService starting.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessScheduledBriefingsAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during briefing generation scheduling");
            }

            // Run every 6 hours
            await Task.Delay(TimeSpan.FromHours(6), stoppingToken);
        }
    }

    private async Task ProcessScheduledBriefingsAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var briefingService = scope.ServiceProvider.GetRequiredService<IBusinessBriefingService>();
        
        var today = DateTime.UtcNow.Date;

        // In a real multi-tenant SaaS, you'd only generate for "Active" businesses that have
        // logged in recently. Here, we fetch all organizations.
        var activeOrgs = await dbContext.Organizations
            .Select(o => o.Id)
            .Take(100) // Safety limit for MVP
            .ToListAsync(cancellationToken);

        foreach (var orgId in activeOrgs)
        {
            if (cancellationToken.IsCancellationRequested) break;

            try
            {
                // Check if already generated for today
                var existingBriefing = await dbContext.BusinessBriefings
                    .AnyAsync(b => b.OrganizationId == orgId && b.Date == today, cancellationToken);
                    
                if (!existingBriefing)
                {
                    _logger.LogInformation("Generating background briefing for organization {OrgId} for {Date}", orgId, today);
                    await briefingService.GenerateBriefingAsync(orgId, today, forceRegeneration: false, cancellationToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to process background briefing for organization {OrgId}", orgId);
            }
        }
    }
}
