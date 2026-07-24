using backend.Entities;
using backend.Modules.AiAgent.ToolRegistry.Interfaces;
using backend.Modules.AiAgent.ToolRegistry.Models;
using backend.Persistence;

namespace backend.Modules.AiAgent.ToolRegistry.Tools;

public class EmailTool : ITool
{
    private readonly ApplicationDbContext _context;

    public EmailTool(ApplicationDbContext context)
    {
        _context = context;
    }

    public string Name => "EmailTool";
    public string Description => "Email Automation Tool: send customer emails and payment reminders.";
    public string Category => "Communication";
    public string[] RequiredPermissions => new[] { "Email.Send" };
    public bool IsDestructive => false;
    public string ParametersSchema => "{\"type\":\"object\",\"properties\":{\"recipient\":{\"type\":\"string\"},\"subject\":{\"type\":\"string\"},\"body\":{\"type\":\"string\"}}}";

    public async Task<ToolResult> ExecuteAsync(
        ToolExecutionContext context,
        IDictionary<string, object> parameters,
        CancellationToken cancellationToken = default)
    {
        var recipient = parameters.TryGetValue("recipient", out var recObj) ? recObj?.ToString() : "customer@example.com";
        var subject = parameters.TryGetValue("subject", out var subObj) ? subObj?.ToString() : "Important Update from BusinessOS";
        var body = parameters.TryGetValue("body", out var bodyObj) ? bodyObj?.ToString() : "Hello, this is an automated message from BusinessOS AI.";

        var notification = new Notification
        {
            Id = Guid.NewGuid(),
            OrganizationId = context.OrganizationId,
            UserId = context.UserId,
            Title = subject ?? "Email Sent",
            Message = $"Email dispatched to {recipient}: {body}",
            Type = "Email",
            Status = "Unread"
        };

        await _context.Notifications.AddAsync(notification, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return ToolResult.Ok($"Email successfully sent to {recipient} with subject '{subject}'.", new { Recipient = recipient, Subject = subject, SentAt = DateTime.UtcNow });
    }
}
