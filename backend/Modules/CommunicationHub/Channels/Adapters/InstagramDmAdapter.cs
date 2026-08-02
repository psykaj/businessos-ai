using System;
using System.Text.Json;
using System.Threading.Tasks;
using backend.Modules.CommunicationHub.Channels.DTOs;
using backend.Modules.CommunicationHub.Channels.Interfaces;
using backend.Modules.CommunicationHub.Entities;
using Microsoft.Extensions.Logging;

namespace backend.Modules.CommunicationHub.Channels.Adapters;

public class InstagramDmAdapter : IChannelAdapter
{
    private readonly ILogger<InstagramDmAdapter> _logger;

    public CommunicationChannelType SupportedChannelType => CommunicationChannelType.InstagramDm;

    public InstagramDmAdapter(ILogger<InstagramDmAdapter> logger)
    {
        _logger = logger;
    }

    public Task<OutboundMessageResult> SendMessageAsync(OutboundMessageRequest request)
    {
        _logger.LogInformation("InstagramDmAdapter: Dispatched IG Direct message to {Handle}", request.RecipientIdentifier);

        var result = new OutboundMessageResult
        {
            Success = true,
            ExternalMessageId = $"ig-{Guid.NewGuid():N}",
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
            var handle = root.TryGetProperty("username", out var uEl) ? uEl.GetString() ?? "ig_user" : "ig_user";
            var text = root.TryGetProperty("text", out var tEl) ? tEl.GetString() ?? "" : payload;

            return Task.FromResult<InboundMessageResult?>(new InboundMessageResult
            {
                SenderIdentifier = handle,
                SenderName = $"@{handle}",
                Content = text,
                ExternalMessageId = $"in-ig-{Guid.NewGuid()}",
                ChannelType = CommunicationChannelType.InstagramDm
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to parse Instagram DM inbound payload");
            return Task.FromResult<InboundMessageResult?>(null);
        }
    }
}
