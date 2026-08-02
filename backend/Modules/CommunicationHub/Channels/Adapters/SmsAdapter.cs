using System;
using System.Text.Json;
using System.Threading.Tasks;
using backend.Modules.CommunicationHub.Channels.DTOs;
using backend.Modules.CommunicationHub.Channels.Interfaces;
using backend.Modules.CommunicationHub.Entities;
using Microsoft.Extensions.Logging;

namespace backend.Modules.CommunicationHub.Channels.Adapters;

public class SmsAdapter : IChannelAdapter
{
    private readonly ILogger<SmsAdapter> _logger;

    public CommunicationChannelType SupportedChannelType => CommunicationChannelType.SMS;

    public SmsAdapter(ILogger<SmsAdapter> logger)
    {
        _logger = logger;
    }

    public Task<OutboundMessageResult> SendMessageAsync(OutboundMessageRequest request)
    {
        _logger.LogInformation("SmsAdapter: Sent SMS to {Phone}. Length: {Len}", 
            request.RecipientIdentifier, request.Content.Length);

        // Simulate Twilio / AWS SNS delivery
        var result = new OutboundMessageResult
        {
            Success = true,
            ExternalMessageId = $"sms-{Guid.NewGuid()}",
            Status = DeliveryStatus.Sent
        };
        return Task.FromResult(result);
    }

    public Task<InboundMessageResult?> ParseInboundWebhookAsync(string payload, string? signature)
    {
        try
        {
            using var doc = JsonDocument.Parse(payload);
            var root = doc.RootElement;
            var from = root.TryGetProperty("From", out var fromEl) ? fromEl.GetString() ?? "Unknown SMS" : "Unknown SMS";
            var body = root.TryGetProperty("Body", out var bodyEl) ? bodyEl.GetString() ?? "" : payload;

            return Task.FromResult<InboundMessageResult?>(new InboundMessageResult
            {
                SenderIdentifier = from,
                SenderName = $"SMS Contact ({from})",
                Content = body,
                ExternalMessageId = $"in-sms-{Guid.NewGuid()}",
                ChannelType = CommunicationChannelType.SMS
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to parse SMS inbound payload");
            return Task.FromResult<InboundMessageResult?>(null);
        }
    }
}
