using System;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using backend.Modules.AI.Interfaces;
using backend.Modules.BusinessIntelligence.Entities;
using backend.Modules.BusinessIntelligence.Interfaces;
using Microsoft.Extensions.Logging;

namespace backend.Modules.BusinessIntelligence.Services;

public class BriefingGenerator : IBriefingGenerator
{
    private readonly IBusinessInsightAggregator _aggregator;
    private readonly IInsightPrioritizer _prioritizer;
    private readonly IAIService _aiService;
    private readonly ILogger<BriefingGenerator> _logger;

    public BriefingGenerator(
        IBusinessInsightAggregator aggregator,
        IInsightPrioritizer prioritizer,
        IAIService aiService,
        ILogger<BriefingGenerator> logger)
    {
        _aggregator = aggregator;
        _prioritizer = prioritizer;
        _aiService = aiService;
        _logger = logger;
    }

    public async Task<BusinessBriefing> GenerateBriefingAsync(Guid organizationId, DateTime date, CancellationToken cancellationToken = default)
    {
        var briefing = new BusinessBriefing
        {
            Id = Guid.NewGuid(),
            OrganizationId = organizationId,
            Date = date,
            PeriodStart = date.Date,
            PeriodEnd = date.Date.AddDays(1).AddTicks(-1),
            GeneratedAt = DateTime.UtcNow,
            Status = BriefingStatus.Draft
        };

        try
        {
            var rawInsights = await _aggregator.GatherInsightsAsync(organizationId, date, cancellationToken);
            var prioritizedInsights = _prioritizer.Prioritize(rawInsights);
            
            foreach (var item in prioritizedInsights)
            {
                item.BriefingId = briefing.Id;
                briefing.Items.Add(item);
            }
            
            briefing.RiskCount = briefing.Items.Count(i => i.Type == BriefingItemType.Risk);
            briefing.OpportunityCount = briefing.Items.Count(i => i.Type == BriefingItemType.Opportunity);
            briefing.ActionCount = briefing.Items.Count(i => i.Type == BriefingItemType.Action || i.Type == BriefingItemType.Recommendation);

            var systemPrompt = @"You are a top-tier Business Operations AI Assistant. 
Analyze the provided business insights and generate a concise executive summary for the daily business briefing.
Output MUST be in JSON format:
{
  ""summary"": ""A 2-3 sentence overview of today's business state"",
  ""businessHealthScore"": 85,
  ""revenueSummary"": ""Short summary of revenue performance"",
  ""customerSummary"": ""Short summary of customer activity"",
  ""financeSummary"": ""Short summary of finance/invoices"",
  ""inventorySummary"": ""Short summary of inventory""
}
Do NOT invent numbers. Use only the provided data. If there is no data for a section, write 'No significant activity reported.'";

            var insightsJson = JsonSerializer.Serialize(prioritizedInsights.Select(i => new { i.Type, i.Title, i.Description, i.Priority }));
            
            try
            {
                var aiResponse = await _aiService.ChatCompletionAsync(organizationId, systemPrompt, $"Insights: {insightsJson}");
                
                // Parse AI Response
                // Simple cleanup to extract json if wrapped in markdown
                aiResponse = aiResponse.Trim();
                if (aiResponse.StartsWith("```json")) aiResponse = aiResponse.Substring(7);
                if (aiResponse.StartsWith("```")) aiResponse = aiResponse.Substring(3);
                if (aiResponse.EndsWith("```")) aiResponse = aiResponse.Substring(0, aiResponse.Length - 3);
                
                var aiSummary = JsonSerializer.Deserialize<AiBriefingSummary>(aiResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                
                if (aiSummary != null)
                {
                    briefing.Summary = aiSummary.Summary ?? "Summary generated.";
                    briefing.BusinessHealthScore = aiSummary.BusinessHealthScore > 0 ? aiSummary.BusinessHealthScore : 80;
                    briefing.RevenueSummary = aiSummary.RevenueSummary ?? "No data.";
                    briefing.CustomerSummary = aiSummary.CustomerSummary ?? "No data.";
                    briefing.FinanceSummary = aiSummary.FinanceSummary ?? "No data.";
                    briefing.InventorySummary = aiSummary.InventorySummary ?? "No data.";
                }
            }
            catch (Exception aiEx)
            {
                _logger.LogWarning(aiEx, "AI Summary Generation failed. Falling back to deterministic summary.");
                ApplyFallbackSummary(briefing);
            }

            briefing.Status = BriefingStatus.Generated;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to generate briefing for organization {OrgId}", organizationId);
            briefing.Status = BriefingStatus.Failed;
            briefing.Summary = "Briefing generation failed due to an internal error.";
        }

        return briefing;
    }

    private void ApplyFallbackSummary(BusinessBriefing briefing)
    {
        briefing.Summary = $"You have {briefing.Items.Count} insights today. " +
                           $"{briefing.RiskCount} require attention. " +
                           $"{briefing.OpportunityCount} opportunities identified.";
        briefing.BusinessHealthScore = 80; // Default
        briefing.RevenueSummary = "Calculated manually due to AI unavailability.";
        briefing.CustomerSummary = "Please review the highlights section.";
        briefing.FinanceSummary = briefing.RiskCount > 0 ? "Check overdue invoices." : "All good.";
        briefing.InventorySummary = "Check inventory risks.";
    }

    private class AiBriefingSummary
    {
        public string Summary { get; set; } = string.Empty;
        public int BusinessHealthScore { get; set; }
        public string RevenueSummary { get; set; } = string.Empty;
        public string CustomerSummary { get; set; } = string.Empty;
        public string FinanceSummary { get; set; } = string.Empty;
        public string InventorySummary { get; set; } = string.Empty;
    }
}
