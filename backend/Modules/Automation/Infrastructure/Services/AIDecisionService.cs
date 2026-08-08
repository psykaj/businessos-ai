using System;
using System.Text.Json;
using System.Threading.Tasks;
using backend.Modules.AI.Interfaces;
using backend.Modules.Automation.Application.Interfaces;
using backend.Modules.Automation.Domain.Entities;

namespace backend.Modules.Automation.Infrastructure.Services;

public class AIDecisionService : IAIDecisionService
{
    private readonly IAIService _aiService;

    public AIDecisionService(IAIService aiService)
    {
        _aiService = aiService;
    }

    public async Task<AIDecisionResult> MakeDecisionAsync(WorkflowStep step, Guid organizationId, object eventData)
    {
        var systemPrompt = @"You are an AI Business Decision Engine. 
You must analyze the business event data and the workflow step configuration.
Respond strictly in valid JSON format matching this schema:
{
  ""Decision"": ""Approve|Reject|Hold|Proceed|etc"",
  ""Reason"": ""Detailed reason for the decision"",
  ""Confidence"": 0.95,
  ""RecommendedAction"": ""CreateTask|SendEmail|Discount|etc"",
  ""RiskLevel"": ""Low|Medium|High"",
  ""ExpectedBusinessImpact"": ""Description of impact""
}";

        var userPrompt = $@"
Workflow Step: {step.Name}
Configuration: {step.Configuration ?? "{}"}
Event Data: {JsonSerializer.Serialize(eventData)}
Evaluate the event data according to the workflow step configuration and make a decision.";

        try
        {
            var response = await _aiService.ChatCompletionAsync(organizationId, systemPrompt, userPrompt);
            
            // Try to extract JSON if it's wrapped in markdown
            var jsonString = response.Trim();
            if (jsonString.StartsWith("```json"))
            {
                jsonString = jsonString.Substring(7, jsonString.Length - 10).Trim();
            }

            var result = JsonSerializer.Deserialize<AIDecisionResult>(jsonString, new JsonSerializerOptions 
            { 
                PropertyNameCaseInsensitive = true 
            });

            return result ?? CreateFallbackResult("Failed to deserialize AI response");
        }
        catch (Exception ex)
        {
            return CreateFallbackResult($"AI Error: {ex.Message}");
        }
    }

    private AIDecisionResult CreateFallbackResult(string reason)
    {
        return new AIDecisionResult
        {
            Decision = "Error",
            Reason = reason,
            Confidence = 0,
            RecommendedAction = "None",
            RiskLevel = "High",
            ExpectedBusinessImpact = "Unknown due to error"
        };
    }
}
