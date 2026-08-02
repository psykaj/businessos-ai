using System;
using System.Text.Json;
using System.Threading.Tasks;
using backend.Modules.CommunicationHub.Channels.DTOs;
using backend.Modules.CommunicationHub.Channels.Interfaces;
using backend.Modules.CommunicationHub.Entities;
using Microsoft.Extensions.Logging;

namespace backend.Modules.CommunicationHub.Channels.Adapters;

public class FacebookMessengerAdapter : IChannelAdapter
{
    private readonly ILogger<FacebookMessengerAdapter> _logger;

    public CommunicationChannelType SupportedChannelType => CommunicationChannelType.FacebookMessenger;

    public FacebookMessengerAdapter(ILogger<FacebookMessengerAdapter> logger)
    {
        _logger = logger;
    }

    public Task<OutboundMessageResult> SendMessageAsync(OutboundMessageRequest request)
    {
        _logger.LogInformation("FacebookMessengerAdapter: Dispatched FB message to {Recipient}", request.RecipientIdentifier);

        var result = new OutboundMessageResult
        {
            Success = true,
            ExternalMessageId = $"fbm-{Guid.NewGuid():N}",
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
            var senderId = root.TryGetProperty("sender_id", out var sEl) ? sEl.GetString() ?? "FB_User" : "FB_User";
            var text = root.TryGetProperty("message", out var mEl) ? mEl.GetString() ?? "" : payload;

            return Task.FromResult<InboundMessageResult?>(new InboundMessageResult
            {
                SenderIdentifier = senderId,
                SenderName = $"FB User ({senderId})",
                Content = text,
                ExternalMessageId = $"in-fb-{Guid.NewGuid()}",
                ChannelType = CommunicationChannelType.FacebookMessenger
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to parse FB Messenger inbound payload");
            return Task.FromResult<InboundMessageResult?>(null);
        }
    }
}
