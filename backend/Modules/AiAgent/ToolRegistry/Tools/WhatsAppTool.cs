using backend.Interfaces;
using backend.Modules.AiAgent.ToolRegistry.Interfaces;
using backend.Modules.AiAgent.ToolRegistry.Models;

namespace backend.Modules.AiAgent.ToolRegistry.Tools;

public class WhatsAppTool : ITool
{
    private readonly IWhatsAppService _whatsAppService;

    public WhatsAppTool(IWhatsAppService whatsAppService)
    {
        _whatsAppService = whatsAppService;
    }

    public string Name => "WhatsAppTool";
    public string Description => "WhatsApp Communication Tool: send instant WhatsApp messages to contacts and leads.";
    public string Category => "Communication";
    public string[] RequiredPermissions => new[] { "WhatsApp.Send" };
    public bool IsDestructive => false;
    public string ParametersSchema => "{\"type\":\"object\",\"properties\":{\"to\":{\"type\":\"string\"},\"message\":{\"type\":\"string\"}}}";

    public async Task<ToolResult> ExecuteAsync(
        ToolExecutionContext context,
        IDictionary<string, object> parameters,
        CancellationToken cancellationToken = default)
    {
        var to = parameters.TryGetValue("to", out var toObj) ? toObj?.ToString() : "+1234567890";
        var templateName = parameters.TryGetValue("templateName", out var tmplObj) ? tmplObj?.ToString() : "general_notification";

        if (string.IsNullOrWhiteSpace(to))
        {
            return ToolResult.Fail("Recipient phone number ('to') is required for WhatsApp execution.");
        }

        var sent = await _whatsAppService.SendMessageAsync(context.OrganizationId, to, templateName ?? "general_notification", "en");

        if (sent)
        {
            return ToolResult.Ok($"WhatsApp notification template sent successfully to {to}.", new { Recipient = to, Template = templateName });
        }

        return ToolResult.Fail($"Failed to send WhatsApp message to {to}. Please verify WhatsApp configuration.");
    }
}
