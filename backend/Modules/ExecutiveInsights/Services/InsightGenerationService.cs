using System;
using System.Threading.Tasks;
using backend.Modules.ExecutiveInsights.Entities;
using backend.Modules.ExecutiveInsights.Repositories;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace backend.Modules.ExecutiveInsights.Services;

public class InsightGenerationService : IInsightGenerationService
{
    private readonly ApplicationDbContext _context;
    private readonly IExecutiveInsightRepository _insightRepository;
    private readonly ILogger<InsightGenerationService> _logger;

    public InsightGenerationService(ApplicationDbContext context, IExecutiveInsightRepository insightRepository, ILogger<InsightGenerationService> logger)
    {
        _context = context;
        _insightRepository = insightRepository;
        _logger = logger;
    }

    public async Task GenerateInsightsAsync(Guid organizationId)
    {
        // 1. Analyze Revenue vs Expenses (Mock example based on available entities)
        var lastMonthStart = DateTime.UtcNow.AddMonths(-1);
        
        var revenueThisMonth = await _context.Invoices
            .Where(i => i.OrganizationId == organizationId && i.Status == "Paid" && i.CreatedAt.Month == DateTime.UtcNow.Month)
            .SumAsync(i => i.Amount);

        // Normally we check expenses, but assuming some logic here for demonstration
        if (revenueThisMonth < 1000)
        {
            await _insightRepository.AddAsync(new ExecutiveInsight
            {
                OrganizationId = organizationId,
                Title = "Revenue Drop Detected",
                Description = "Paid invoices over the last 30 days are significantly lower than usual.",
                Category = "Financial",
                Priority = "High",
                BusinessImpact = 5000,
                ConfidenceLevel = 90,
                SuggestedAction = "Review sales pipeline and pending invoices to accelerate cash flow.",
                CreatedAt = DateTime.UtcNow
            });
        }
        else
        {
             await _insightRepository.AddAsync(new ExecutiveInsight
            {
                OrganizationId = organizationId,
                Title = "Strong Revenue Performance",
                Description = "Revenue generation is healthy over the last 30 days.",
                Category = "Financial",
                Priority = "Low",
                BusinessImpact = revenueThisMonth,
                ConfidenceLevel = 95,
                SuggestedAction = "Maintain current sales velocity.",
                CreatedAt = DateTime.UtcNow
            });
        }

        _logger.LogInformation("Generated insights for Org: {OrgId}", organizationId);
    }
}
