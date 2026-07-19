using System.Text.Json;
using backend.Entities;
using backend.Modules.Automation.Interfaces;
using backend.Modules.Email.Interfaces;
using backend.Modules.Notifications.Interfaces;
using backend.Interfaces;
using Microsoft.Extensions.Logging;

namespace backend.Modules.Automation.Services;

public class AutomationEngineService : IAutomationEngineService
{
    private readonly IAutomationRepository _repository;
    private readonly IEmailService _emailService;
    private readonly INotificationService _notificationService;
    private readonly IWhatsAppService _whatsAppService;
    private readonly ILogger<AutomationEngineService> _logger;

    public AutomationEngineService(
        IAutomationRepository repository,
        IEmailService emailService,
        INotificationService notificationService,
        IWhatsAppService whatsAppService,
        ILogger<AutomationEngineService> logger)
    {
        _repository = repository;
        _emailService = emailService;
        _notificationService = notificationService;
        _whatsAppService = whatsAppService;
        _logger = logger;
    }

    public async Task ExecuteTriggerAsync(Guid organizationId, string trigger, Dictionary<string, string> payload)
    {
        var rules = await _repository.GetRulesByTriggerAsync(organizationId, trigger);
        
        foreach (var rule in rules)
        {
            try
            {
                // Parse Conditions (Placeholder for condition evaluation logic)
                bool conditionsMet = true; 
                
                if (conditionsMet)
                {
                    // Parse Actions
                    var actions = JsonSerializer.Deserialize<List<AutomationAction>>(rule.Actions);
                    if (actions != null)
                    {
                        foreach (var action in actions)
                        {
                            await ExecuteActionAsync(organizationId, action, payload);
                        }
                    }

                    await _repository.AddLogAsync(new AutomationLog
                    {
                        OrganizationId = organizationId,
                        RuleId = rule.Id,
                        Status = "Success"
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to execute automation rule {rule.Id}");
                await _repository.AddLogAsync(new AutomationLog
                {
                    OrganizationId = organizationId,
                    RuleId = rule.Id,
                    Status = "Failed",
                    ErrorMessage = ex.Message
                });
            }
        }
    }

    private async Task ExecuteActionAsync(Guid organizationId, AutomationAction action, Dictionary<string, string> payload)
    {
        switch (action.Type)
        {
            case "SendEmail":
                var to = action.Parameters.ContainsKey("To") ? ReplaceVariables(action.Parameters["To"], payload) : "";
                var subject = action.Parameters.ContainsKey("Subject") ? ReplaceVariables(action.Parameters["Subject"], payload) : "";
                var body = action.Parameters.ContainsKey("Body") ? ReplaceVariables(action.Parameters["Body"], payload) : "";
                if (!string.IsNullOrEmpty(to))
                {
                    await _emailService.SendEmailAsync(organizationId, to, subject, body);
                }
                break;
                
            case "SendNotification":
                var userId = action.Parameters.ContainsKey("UserId") ? ReplaceVariables(action.Parameters["UserId"], payload) : "";
                var title = action.Parameters.ContainsKey("Title") ? ReplaceVariables(action.Parameters["Title"], payload) : "";
                var message = action.Parameters.ContainsKey("Message") ? ReplaceVariables(action.Parameters["Message"], payload) : "";
                if (!string.IsNullOrEmpty(userId))
                {
                    await _notificationService.SendNotificationAsync(organizationId, userId, title, message);
                }
                break;
                
            case "CallWebhook":
                // Webhook logic
                break;
                
            default:
                _logger.LogWarning($"Unknown automation action type: {action.Type}");
                break;
        }
    }
    
    private string ReplaceVariables(string template, Dictionary<string, string> payload)
    {
        var result = template;
        foreach(var item in payload)
        {
            result = result.Replace($"{{{{{item.Key}}}}}", item.Value);
        }
        return result;
    }
}

public class AutomationAction
{
    public string Type { get; set; } = string.Empty;
    public Dictionary<string, string> Parameters { get; set; } = new();
}
