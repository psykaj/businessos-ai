using System;
using System.Threading.Tasks;
using backend.Modules.CustomerFeedback.Entities;
using backend.Modules.CustomerFeedback.Sentiment.Interfaces;
using Microsoft.Extensions.Logging;

namespace backend.Modules.CustomerFeedback.Sentiment.Services;

/// <summary>
/// Provider-independent AI Sentiment Engine foundation.
/// Designed for zero latency offline heuristics with architectural readiness for LLM ingestion (Gemini / OpenAI / Claude).
/// Saves business owners time by automatically scoring urgency, sorting feedback, and drafting actionable resolutions.
/// </summary>
public class ProviderIndependentSentimentEngine : ISentimentAnalysisEngine
{
    private readonly ILogger<ProviderIndependentSentimentEngine> _logger;

    public ProviderIndependentSentimentEngine(ILogger<ProviderIndependentSentimentEngine> logger)
    {
        _logger = logger;
    }

    public Task<SentimentAnalysis> AnalyzeAsync(Guid organizationId, SentimentTargetType targetType, Guid targetId, string text)
    {
        _logger.LogDebug("Analyzing sentiment for {TargetType} ({TargetId}) for org {OrgId}", targetType, targetId, organizationId);

        string cleanText = (text ?? string.Empty).ToLowerInvariant();

        // Heuristic fallback & instant baseline scoring
        var analysis = new SentimentAnalysis
        {
            OrganizationId = organizationId,
            TargetEntityType = targetType,
            TargetEntityId = targetId,
            EngineProvider = "ProviderIndependentAI_v1"
        };

        bool hasUrgentWords = cleanText.Contains("cancel") || cleanText.Contains("refund") || 
                              cleanText.Contains("lawyer") || cleanText.Contains("unacceptable") || 
                              cleanText.Contains("immediately") || cleanText.Contains("worst") ||
                              cleanText.Contains("broken") || cleanText.Contains("scam");

        bool hasPositiveWords = cleanText.Contains("great") || cleanText.Contains("love") || 
                                cleanText.Contains("awesome") || cleanText.Contains("excellent") || 
                                cleanText.Contains("happy") || cleanText.Contains("helpful") ||
                                cleanText.Contains("amazing") || cleanText.Contains("thank");

        if (hasUrgentWords && !hasPositiveWords)
        {
            analysis.Sentiment = SentimentClassification.Negative;
            analysis.ConfidenceScore = 0.945m;
            analysis.UrgencyScore = cleanText.Contains("lawyer") || cleanText.Contains("cancel") ? 9.5m : 7.8m;
            analysis.MainComplaint = ExtractComplaintSnippet(text);
            analysis.SuggestedImprovement = GenerateActionableSuggestion(cleanText);
        }
        else if (hasPositiveWords && !hasUrgentWords)
        {
            analysis.Sentiment = SentimentClassification.Positive;
            analysis.ConfidenceScore = 0.962m;
            analysis.UrgencyScore = 1.0m;
            analysis.MainComplaint = null;
            analysis.SuggestedImprovement = "Send personalized follow-up thanking customer and inviting referral review to boost retention & revenue.";
        }
        else
        {
            analysis.Sentiment = SentimentClassification.Neutral;
            analysis.ConfidenceScore = 0.850m;
            analysis.UrgencyScore = 4.0m;
            analysis.MainComplaint = string.IsNullOrWhiteSpace(text) ? "No comment provided" : "Routine feedback or mixed sentiment";
            analysis.SuggestedImprovement = "Monitor customer satisfaction trend and schedule routine check-in.";
        }

        analysis.RawModelOutputJson = $"{{\"provider\":\"{analysis.EngineProvider}\", \"word_count\":{text?.Split(' ').Length ?? 0}, \"timestamp\":\"{DateTime.UtcNow:O}\"}}";
        
        return Task.FromResult(analysis);
    }

    private static string ExtractComplaintSnippet(string? text)
    {
        if (string.IsNullOrWhiteSpace(text)) return "Unspecified customer friction point";
        if (text.Length <= 100) return text;
        return text.Substring(0, 97) + "...";
    }

    private static string GenerateActionableSuggestion(string cleanText)
    {
        if (cleanText.Contains("refund") || cleanText.Contains("billing") || cleanText.Contains("charge"))
        {
            return "Prioritize billing verification and offer expedited credit or discount to prevent immediate customer churn.";
        }
        if (cleanText.Contains("slow") || cleanText.Contains("wait") || cleanText.Contains("delay"))
        {
            return "SLA bottleneck detected. Route directly to senior technical support agent for immediate resolution.";
        }
        if (cleanText.Contains("bug") || cleanText.Contains("broken") || cleanText.Contains("error"))
        {
            return "Log critical QA defect report and notify customer upon release patch deployment.";
        }
        return "Reach out via proactive concierge telephone call within 2 hours to remediate relationship.";
    }
}
