using System.Text.Json;
using backend.Interfaces;
using backend.Modules.Email.Interfaces;
using backend.Modules.Notifications.Interfaces;
using backend.Modules.Workflow.Constants;
using backend.Modules.Workflow.Interfaces;
using Microsoft.Extensions.Logging;

namespace backend.Modules.Workflow.Actions.Handlers;

public class CreateCrmLeadActionHandler : IActionHandler
{
    public ActionType ActionType => ActionType.CreateCrmLead;

    public Task<ActionResult> ExecuteAsync(Guid organizationId, string actionConfiguration, Dictionary<string, string> context, CancellationToken cancellationToken = default)
    {
        var output = new Dictionary<string, string>
        {
            ["LeadId"] = Guid.NewGuid().ToString(),
            ["Status"] = "Created"
        };
        return Task.FromResult(new ActionResult(true, JsonSerializer.Serialize(output), null, output));
    }
}

public class UpdateCrmActionHandler : IActionHandler
{
    public ActionType ActionType => ActionType.UpdateCrm;

    public Task<ActionResult> ExecuteAsync(Guid organizationId, string actionConfiguration, Dictionary<string, string> context, CancellationToken cancellationToken = default)
    {
        var output = new Dictionary<string, string> { ["Status"] = "Updated" };
        return Task.FromResult(new ActionResult(true, JsonSerializer.Serialize(output), null, output));
    }
}

public class AssignSalespersonActionHandler : IActionHandler
{
    public ActionType ActionType => ActionType.AssignSalesperson;

    public Task<ActionResult> ExecuteAsync(Guid organizationId, string actionConfiguration, Dictionary<string, string> context, CancellationToken cancellationToken = default)
    {
        var output = new Dictionary<string, string> { ["AssignedSalesperson"] = "Sales Agent" };
        return Task.FromResult(new ActionResult(true, JsonSerializer.Serialize(output), null, output));
    }
}

public class SendEmailActionHandler : IActionHandler
{
    private readonly IEmailService _emailService;
    private readonly IVariableResolver _variableResolver;

    public SendEmailActionHandler(IEmailService emailService, IVariableResolver variableResolver)
    {
        _emailService = emailService;
        _variableResolver = variableResolver;
    }

    public ActionType ActionType => ActionType.SendEmail;

    public async Task<ActionResult> ExecuteAsync(Guid organizationId, string actionConfiguration, Dictionary<string, string> context, CancellationToken cancellationToken = default)
    {
        try
        {
            var config = JsonSerializer.Deserialize<Dictionary<string, string>>(actionConfiguration) ?? new();
            var to = config.TryGetValue("To", out var t) ? _variableResolver.Resolve(t, context) : context.GetValueOrDefault("CustomerEmail", "");
            var subject = config.TryGetValue("Subject", out var s) ? _variableResolver.Resolve(s, context) : "Notification from BusinessOS AI";
            var body = config.TryGetValue("Body", out var b) ? _variableResolver.Resolve(b, context) : "Hello!";

            if (!string.IsNullOrEmpty(to))
            {
                await _emailService.SendEmailAsync(organizationId, to, subject, body);
            }

            var output = new Dictionary<string, string> { ["To"] = to, ["Status"] = "Sent" };
            return new ActionResult(true, JsonSerializer.Serialize(output), null, output);
        }
        catch (Exception ex)
        {
            return new ActionResult(false, "{}", ex.Message);
        }
    }
}

public class SendWhatsAppActionHandler : IActionHandler
{
    private readonly IWhatsAppService _whatsAppService;
    private readonly IVariableResolver _variableResolver;

    public SendWhatsAppActionHandler(IWhatsAppService whatsAppService, IVariableResolver variableResolver)
    {
        _whatsAppService = whatsAppService;
        _variableResolver = variableResolver;
    }

    public ActionType ActionType => ActionType.SendWhatsApp;

    public async Task<ActionResult> ExecuteAsync(Guid organizationId, string actionConfiguration, Dictionary<string, string> context, CancellationToken cancellationToken = default)
    {
        try
        {
            var config = JsonSerializer.Deserialize<Dictionary<string, string>>(actionConfiguration) ?? new();
            var phone = config.TryGetValue("Phone", out var p) ? _variableResolver.Resolve(p, context) : context.GetValueOrDefault("CustomerPhone", "");
            var message = config.TryGetValue("Message", out var m) ? _variableResolver.Resolve(m, context) : "";

            if (!string.IsNullOrEmpty(phone) && !string.IsNullOrEmpty(message))
            {
                await _whatsAppService.SendMessageAsync(organizationId, phone, message, "en");
            }

            var output = new Dictionary<string, string> { ["Phone"] = phone, ["Status"] = "Sent" };
            return new ActionResult(true, JsonSerializer.Serialize(output), null, output);
        }
        catch (Exception ex)
        {
            return new ActionResult(false, "{}", ex.Message);
        }
    }
}

public class SendNotificationActionHandler : IActionHandler
{
    private readonly INotificationService _notificationService;
    private readonly IVariableResolver _variableResolver;

    public SendNotificationActionHandler(INotificationService notificationService, IVariableResolver variableResolver)
    {
        _notificationService = notificationService;
        _variableResolver = variableResolver;
    }

    public ActionType ActionType => ActionType.SendNotification;

    public async Task<ActionResult> ExecuteAsync(Guid organizationId, string actionConfiguration, Dictionary<string, string> context, CancellationToken cancellationToken = default)
    {
        try
        {
            var config = JsonSerializer.Deserialize<Dictionary<string, string>>(actionConfiguration) ?? new();
            var userId = config.TryGetValue("UserId", out var u) ? _variableResolver.Resolve(u, context) : context.GetValueOrDefault("UserId", "");
            var title = config.TryGetValue("Title", out var t) ? _variableResolver.Resolve(t, context) : "Workflow Alert";
            var message = config.TryGetValue("Message", out var m) ? _variableResolver.Resolve(m, context) : "Action triggered";

            if (!string.IsNullOrEmpty(userId))
            {
                await _notificationService.SendNotificationAsync(organizationId, userId, title, message);
            }

            var output = new Dictionary<string, string> { ["Status"] = "Sent" };
            return new ActionResult(true, JsonSerializer.Serialize(output), null, output);
        }
        catch (Exception ex)
        {
            return new ActionResult(false, "{}", ex.Message);
        }
    }
}

public class GenerateInvoiceActionHandler : IActionHandler
{
    public ActionType ActionType => ActionType.GenerateInvoice;

    public Task<ActionResult> ExecuteAsync(Guid organizationId, string actionConfiguration, Dictionary<string, string> context, CancellationToken cancellationToken = default)
    {
        var output = new Dictionary<string, string>
        {
            ["InvoiceId"] = Guid.NewGuid().ToString(),
            ["InvoiceNumber"] = $"INV-{DateTime.UtcNow:yyyyMMddHHmmss}"
        };
        return Task.FromResult(new ActionResult(true, JsonSerializer.Serialize(output), null, output));
    }
}

public class GenerateQRActionHandler : IActionHandler
{
    public ActionType ActionType => ActionType.GenerateQR;

    public Task<ActionResult> ExecuteAsync(Guid organizationId, string actionConfiguration, Dictionary<string, string> context, CancellationToken cancellationToken = default)
    {
        var output = new Dictionary<string, string>
        {
            ["QRCodeId"] = Guid.NewGuid().ToString(),
            ["QRCodeUrl"] = "https://businessos.ai/qr/" + Guid.NewGuid()
        };
        return Task.FromResult(new ActionResult(true, JsonSerializer.Serialize(output), null, output));
    }
}

public class CallAiAssistantActionHandler : IActionHandler
{
    private readonly IVariableResolver _variableResolver;

    public CallAiAssistantActionHandler(IVariableResolver variableResolver)
    {
        _variableResolver = variableResolver;
    }

    public ActionType ActionType => ActionType.CallAiAssistant;

    public Task<ActionResult> ExecuteAsync(Guid organizationId, string actionConfiguration, Dictionary<string, string> context, CancellationToken cancellationToken = default)
    {
        var config = JsonSerializer.Deserialize<Dictionary<string, string>>(actionConfiguration) ?? new();
        var prompt = config.TryGetValue("Prompt", out var p) ? _variableResolver.Resolve(p, context) : "Summarize workflow context";

        var aiOutput = $"AI Insights for prompt: {prompt} - Lead high conversion probability.";
        var output = new Dictionary<string, string>
        {
            ["AiOutput"] = aiOutput
        };
        return Task.FromResult(new ActionResult(true, JsonSerializer.Serialize(output), null, output));
    }
}

public class CallWebhookActionHandler : IActionHandler
{
    private readonly HttpClient _httpClient;
    private readonly IVariableResolver _variableResolver;

    public CallWebhookActionHandler(IHttpClientFactory httpClientFactory, IVariableResolver variableResolver)
    {
        _httpClient = httpClientFactory.CreateClient();
        _variableResolver = variableResolver;
    }

    public ActionType ActionType => ActionType.CallWebhook;

    public async Task<ActionResult> ExecuteAsync(Guid organizationId, string actionConfiguration, Dictionary<string, string> context, CancellationToken cancellationToken = default)
    {
        try
        {
            var config = JsonSerializer.Deserialize<Dictionary<string, string>>(actionConfiguration) ?? new();
            var url = config.TryGetValue("Url", out var u) ? _variableResolver.Resolve(u, context) : "";

            if (string.IsNullOrEmpty(url))
                return new ActionResult(false, "{}", "Webhook URL is missing");

            var payload = JsonSerializer.Serialize(context);
            var content = new StringContent(payload, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content, cancellationToken);
            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

            var output = new Dictionary<string, string>
            {
                ["StatusCode"] = ((int)response.StatusCode).ToString(),
                ["ResponseBody"] = responseBody.Length > 500 ? responseBody.Substring(0, 500) : responseBody
            };

            return new ActionResult(response.IsSuccessStatusCode, JsonSerializer.Serialize(output), response.IsSuccessStatusCode ? null : $"HTTP {(int)response.StatusCode}", output);
        }
        catch (Exception ex)
        {
            return new ActionResult(false, "{}", ex.Message);
        }
    }
}

public class DelayExecutionActionHandler : IActionHandler
{
    public ActionType ActionType => ActionType.DelayExecution;

    public async Task<ActionResult> ExecuteAsync(Guid organizationId, string actionConfiguration, Dictionary<string, string> context, CancellationToken cancellationToken = default)
    {
        var config = JsonSerializer.Deserialize<Dictionary<string, string>>(actionConfiguration) ?? new();
        int seconds = config.TryGetValue("Seconds", out var s) && int.TryParse(s, out var sec) ? Math.Min(sec, 30) : 1;

        await Task.Delay(TimeSpan.FromSeconds(seconds), cancellationToken);

        var output = new Dictionary<string, string> { ["DelayedSeconds"] = seconds.ToString() };
        return new ActionResult(true, JsonSerializer.Serialize(output), null, output);
    }
}

public class UpdateCustomerJourneyActionHandler : IActionHandler
{
    public ActionType ActionType => ActionType.UpdateCustomerJourney;

    public Task<ActionResult> ExecuteAsync(Guid organizationId, string actionConfiguration, Dictionary<string, string> context, CancellationToken cancellationToken = default)
    {
        var output = new Dictionary<string, string> { ["Stage"] = "UpdatedJourneyStage" };
        return Task.FromResult(new ActionResult(true, JsonSerializer.Serialize(output), null, output));
    }
}

public class LogActivityActionHandler : IActionHandler
{
    private readonly ILogger<LogActivityActionHandler> _logger;

    public LogActivityActionHandler(ILogger<LogActivityActionHandler> logger)
    {
        _logger = logger;
    }

    public ActionType ActionType => ActionType.LogActivity;

    public Task<ActionResult> ExecuteAsync(Guid organizationId, string actionConfiguration, Dictionary<string, string> context, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Workflow LogActivity for Org {OrgId}: {Context}", organizationId, JsonSerializer.Serialize(context));
        var output = new Dictionary<string, string> { ["LoggedAt"] = DateTime.UtcNow.ToString("O") };
        return Task.FromResult(new ActionResult(true, JsonSerializer.Serialize(output), null, output));
    }
}
