using backend.Modules.BusinessIntelligence.Constants;
using backend.Modules.BusinessIntelligence.DTOs;
using backend.Modules.BusinessIntelligence.Entities;
using backend.Modules.BusinessIntelligence.Interfaces;
using backend.Modules.CRM.Enums;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.BusinessIntelligence.Services;

public class AIBusinessInsightEngine : IAIBusinessInsightEngine
{
    private readonly ApplicationDbContext _context;
    private readonly IInsightRepository _insightRepository;

    public AIBusinessInsightEngine(ApplicationDbContext context, IInsightRepository insightRepository)
    {
        _context = context;
        _insightRepository = insightRepository;
    }

    public async Task<GenerateInsightsResponseDto> GenerateInsightsAsync(Guid organizationId, CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        var sevenDaysAgo = now.AddDays(-7);

        var generatedInsights = new List<Insight>();

        // Heuristic Rule 1: High-value leads lacking activity
        var staleHighValueLeads = await _context.Leads
            .Where(l => l.OrganizationId == organizationId && l.EstimatedValue >= 5000m && l.UpdatedAt < sevenDaysAgo && l.Status != LeadStatus.Converted)
            .Take(5)
            .ToListAsync(cancellationToken);

        if (staleHighValueLeads.Count > 0)
        {
            generatedInsights.Add(new Insight
            {
                OrganizationId = organizationId,
                Category = BIConstants.KPICategories.Sales,
                Title = $"{staleHighValueLeads.Count} high-value leads are not being followed up",
                Description = $"There are {staleHighValueLeads.Count} leads with estimated value >= $5,000 that haven't had activity recorded in over 7 days.",
                Priority = BIConstants.InsightPriorities.High,
                BusinessImpact = "Risk of losing high-intent prospective deals worth potential revenue.",
                Recommendation = "Assign high-priority tasks to sales representatives to reach out via call or email today.",
                SuggestedAction = "View Stale Leads & Assign Tasks"
            });
        }

        // Heuristic Rule 2: Subscriptions expiring soon
        var expiringSubscriptions = await _context.Subscriptions
            .Where(s => s.OrganizationId == organizationId && s.CurrentPeriodEnd <= now.AddDays(7) && s.CurrentPeriodEnd >= now)
            .CountAsync(cancellationToken);

        if (expiringSubscriptions > 0)
        {
            generatedInsights.Add(new Insight
            {
                OrganizationId = organizationId,
                Category = BIConstants.KPICategories.Customers,
                Title = $"{expiringSubscriptions} client subscriptions expire this week",
                Description = $"{expiringSubscriptions} recurring customer subscriptions are set to end within 7 days.",
                Priority = BIConstants.InsightPriorities.Critical,
                BusinessImpact = "Potential immediate monthly revenue churn if renewals are not secured.",
                Recommendation = "Trigger automated renewal notification emails and assign account manager follow-ups.",
                SuggestedAction = "Send Renewal Reminders"
            });
        }

        // Heuristic Rule 3: Campaign Performance Analysis
        var topCampaign = await _context.Campaigns
            .Where(c => c.OrganizationId == organizationId && c.Budget > 0)
            .OrderByDescending(c => c.Budget)
            .FirstOrDefaultAsync(cancellationToken);

        if (topCampaign != null)
        {
            generatedInsights.Add(new Insight
            {
                OrganizationId = organizationId,
                Category = BIConstants.KPICategories.Marketing,
                Title = $"Campaign '{topCampaign.Name}' is outperforming expectations",
                Description = $"Marketing campaign '{topCampaign.Name}' sent {topCampaign.SentMessages} messages from budget ${topCampaign.Budget:N0}.",
                Priority = BIConstants.InsightPriorities.Medium,
                BusinessImpact = "High efficiency channel opportunity to double down for rapid acquisition.",
                Recommendation = "Reallocate budget from lower-performing channels into this campaign.",
                SuggestedAction = "Increase Campaign Budget"
            });
        }

        // Heuristic Rule 4: Customer Retention Improvement
        var repeatCount = await _context.Customers
            .CountAsync(c => c.OrganizationId == organizationId, cancellationToken);

        generatedInsights.Add(new Insight
        {
            OrganizationId = organizationId,
            Category = BIConstants.KPICategories.Customers,
            Title = "Customer retention improved this month",
            Description = $"{repeatCount} active accounts are maintaining steady interaction with your business operating system.",
            Priority = BIConstants.InsightPriorities.Low,
            BusinessImpact = "Strong customer lifetime value (LTV) trend reduces customer acquisition cost (CAC) pressure.",
            Recommendation = "Maintain customer onboarding workflows and launch a loyalty referral program.",
            SuggestedAction = "Launch Referral Campaign"
        });

        // Heuristic Rule 5: Sales velocity & deal size check
        var recentWonDeals = await _context.Deals
            .Where(d => d.OrganizationId == organizationId && d.Status == "Won" && d.UpdatedAt >= sevenDaysAgo)
            .CountAsync(cancellationToken);

        if (recentWonDeals == 0)
        {
            generatedInsights.Add(new Insight
            {
                OrganizationId = organizationId,
                Category = BIConstants.KPICategories.Sales,
                Title = "Sales closed dropped this week",
                Description = "No deals were moved to 'Won' status during the last 7 days.",
                Priority = BIConstants.InsightPriorities.High,
                BusinessImpact = "Short-term cash flow deceleration if pipeline conversion stalls.",
                Recommendation = "Review active deals in 'Proposal' or 'Negotiation' stages to offer closing incentives.",
                SuggestedAction = "Review Sales Pipeline"
            });
        }

        await _insightRepository.ClearAndAddAsync(organizationId, generatedInsights, cancellationToken);
        await _insightRepository.SaveChangesAsync(cancellationToken);

        var dtos = generatedInsights.Select(i => new InsightDto
        {
            Id = i.Id,
            OrganizationId = i.OrganizationId,
            Category = i.Category,
            Title = i.Title,
            Description = i.Description,
            Priority = i.Priority,
            Recommendation = i.Recommendation,
            BusinessImpact = i.BusinessImpact,
            SuggestedAction = i.SuggestedAction,
            CreatedAt = i.CreatedAt
        }).ToList();

        return new GenerateInsightsResponseDto
        {
            GeneratedCount = dtos.Count,
            Insights = dtos,
            GeneratedAt = DateTime.UtcNow
        };
    }

    public async Task<List<InsightDto>> GetInsightsAsync(Guid organizationId, string? category = null, string? priority = null, CancellationToken cancellationToken = default)
    {
        var insights = await _insightRepository.GetAllByOrganizationIdAsync(organizationId, category, cancellationToken);

        if (insights.Count == 0)
        {
            var genResult = await GenerateInsightsAsync(organizationId, cancellationToken);
            insights = await _insightRepository.GetAllByOrganizationIdAsync(organizationId, category, cancellationToken);
        }

        if (!string.IsNullOrWhiteSpace(priority))
        {
            insights = insights.Where(i => i.Priority.ToLower() == priority.ToLower()).ToList();
        }

        return insights.Select(i => new InsightDto
        {
            Id = i.Id,
            OrganizationId = i.OrganizationId,
            Category = i.Category,
            Title = i.Title,
            Description = i.Description,
            Priority = i.Priority,
            Recommendation = i.Recommendation,
            BusinessImpact = i.BusinessImpact,
            SuggestedAction = i.SuggestedAction,
            CreatedAt = i.CreatedAt
        }).ToList();
    }
}
