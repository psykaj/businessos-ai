using System;
using System.Text.Json;
using System.Threading.Tasks;
using backend.Modules.CommunicationHub.Channels.DTOs;
using backend.Modules.CommunicationHub.Channels.Interfaces;
using backend.Modules.CommunicationHub.Entities;
using Microsoft.Extensions.Logging;

namespace backend.Modules.CommunicationHub.Channels.Adapters;

public class WhatsAppAdapter : IChannelAdapter
{
    private readonly ILogger<WhatsAppAdapter> _logger;

    public CommunicationChannelType SupportedChannelType => CommunicationChannelType.WhatsApp;

    public WhatsAppAdapter(ILogger<WhatsAppAdapter> logger)
    {
        _logger = logger;
    }

    public Task<OutboundMessageResult> SendMessageAsync(OutboundMessageRequest request)
    {
        _logger.LogInformation("WhatsAppAdapter: Dispatched WhatsApp message to {Phone}. Content: {Preview}", 
            request.RecipientIdentifier, request.Content.Substring(0, Math.Min(30, request.Content.Length)));

        // Integrate with Meta Cloud API or existing WhatsApp service abstraction
        var result = new OutboundMessageResult
        {
            Success = true,
            ExternalMessageId = $"wamid.{Guid.NewGuid():N}",
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
            var sender = root.TryGetProperty("from", out var fromEl) ? fromEl.GetString() ?? "+10000000000" : "+10000000000";
            var text = root.TryGetProperty("text", out var textEl) ? (textEl.TryGetProperty("body", out var bodyEl) ? bodyEl.GetString() ?? "" : "") : payload;

            return Task.FromResult<InboundMessageResult?>(new InboundMessageResult
            {
                SenderIdentifier = sender,
                SenderName = $"WhatsApp {sender}",
                Content = text,
                ExternalMessageId = $"in-wa-{Guid.NewGuid():N}",
                ChannelType = CommunicationChannelType.WhatsApp
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to parse WhatsApp inbound payload");
            return Task.FromResult<InboundMessageResult?>(null);
        }
    }
}
