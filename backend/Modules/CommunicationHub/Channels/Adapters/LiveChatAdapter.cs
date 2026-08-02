using System;
using System.Text.Json;
using System.Threading.Tasks;
using backend.Modules.CommunicationHub.Channels.DTOs;
using backend.Modules.CommunicationHub.Channels.Interfaces;
using backend.Modules.CommunicationHub.Entities;
using Microsoft.Extensions.Logging;

namespace backend.Modules.CommunicationHub.Channels.Adapters;

public class LiveChatAdapter : IChannelAdapter
{
    private readonly ILogger<LiveChatAdapter> _logger;

    public CommunicationChannelType SupportedChannelType => CommunicationChannelType.LiveChat;

    public LiveChatAdapter(ILogger<LiveChatAdapter> logger)
    {
        _logger = logger;
    }

    public Task<OutboundMessageResult> SendMessageAsync(OutboundMessageRequest request)
    {
        _logger.LogInformation("LiveChatAdapter: Sent message to live chat guest {Id}", request.RecipientIdentifier);

        var result = new OutboundMessageResult
        {
            Success = true,
            ExternalMessageId = $"lc-{Guid.NewGuid()}",
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
            var sessionId = root.TryGetProperty("sessionId", out var sEl) ? sEl.GetString() ?? "Visitor" : "Visitor";
            var message = root.TryGetProperty("message", out var mEl) ? mEl.GetString() ?? "" : payload;
            var visitorName = root.TryGetProperty("name", out var nEl) ? nEl.GetString() ?? $"Visitor #{sessionId.Substring(0, Math.Min(4, sessionId.Length))}" : $"Visitor #{sessionId.Substring(0, Math.Min(4, sessionId.Length))}";

            return Task.FromResult<InboundMessageResult?>(new InboundMessageResult
            {
                SenderIdentifier = sessionId,
                SenderName = visitorName,
                Content = message,
                ExternalMessageId = $"in-lc-{Guid.NewGuid()}",
                ChannelType = CommunicationChannelType.LiveChat
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to parse LiveChat inbound payload");
            return Task.FromResult<InboundMessageResult?>(null);
        }
    }
}
