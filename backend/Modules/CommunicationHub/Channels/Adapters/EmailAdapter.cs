using System;
using System.Text.Json;
using System.Threading.Tasks;
using backend.Modules.CommunicationHub.Channels.DTOs;
using backend.Modules.CommunicationHub.Channels.Interfaces;
using backend.Modules.CommunicationHub.Entities;
using Microsoft.Extensions.Logging;

namespace backend.Modules.CommunicationHub.Channels.Adapters;

public class EmailAdapter : IChannelAdapter
{
    private readonly ILogger<EmailAdapter> _logger;

    public CommunicationChannelType SupportedChannelType => CommunicationChannelType.Email;

    public EmailAdapter(ILogger<EmailAdapter> logger)
    {
        _logger = logger;
    }

    public Task<OutboundMessageResult> SendMessageAsync(OutboundMessageRequest request)
    {
        _logger.LogInformation("EmailAdapter: Dispatched email to {Recipient}. Subject/Content length: {Len}", 
            request.RecipientIdentifier, request.Content.Length);

        // Simulate SMTP / SendGrid / Amazon SES delivery
        var result = new OutboundMessageResult
        {
            Success = true,
            ExternalMessageId = $"email-{Guid.NewGuid()}",
            Status = DeliveryStatus.Delivered
        };
        return Task.FromResult(result);
    }

    public Task<InboundMessageResult?> ParseInboundWebhookAsync(string payload, string? signature)
    {
        try
        {
            using var doc = JsonDocument.Parse(payload);
            var root = doc.RootElement;
            var sender = root.TryGetProperty("from", out var fromEl) ? fromEl.GetString() ?? "unknown@sender.com" : "unknown@sender.com";
            var content = root.TryGetProperty("body", out var bodyEl) ? bodyEl.GetString() ?? "" : payload;

            return Task.FromResult<InboundMessageResult?>(new InboundMessageResult
            {
                SenderIdentifier = sender,
                SenderName = sender.Split('@')[0],
                Content = content,
                ExternalMessageId = $"in-email-{Guid.NewGuid()}",
                ChannelType = CommunicationChannelType.Email
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to parse Email inbound payload");
            return Task.FromResult<InboundMessageResult?>(null);
        }
    }
}
