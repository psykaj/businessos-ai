using System.Threading.Tasks;
using backend.Modules.CommunicationHub.Channels.DTOs;
using backend.Modules.CommunicationHub.Entities;

namespace backend.Modules.CommunicationHub.Channels.Interfaces;

public interface IChannelAdapter
{
    CommunicationChannelType SupportedChannelType { get; }
    Task<OutboundMessageResult> SendMessageAsync(OutboundMessageRequest request);
    Task<InboundMessageResult?> ParseInboundWebhookAsync(string payload, string? signature);
}
