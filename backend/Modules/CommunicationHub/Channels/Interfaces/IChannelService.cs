using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Modules.CommunicationHub.Channels.DTOs;
using backend.Modules.CommunicationHub.Entities;

namespace backend.Modules.CommunicationHub.Channels.Interfaces;

public interface IChannelService
{
    Task<IEnumerable<ChannelDto>> GetChannelsAsync(Guid organizationId);
    Task<ChannelDto?> GetChannelByIdAsync(Guid id, Guid organizationId);
    Task<ChannelDto> CreateChannelAsync(Guid organizationId, CreateChannelRequest request, string baseWebhookDomain);
    Task<ChannelDto?> UpdateChannelAsync(Guid id, Guid organizationId, UpdateChannelRequest request);
    Task<bool> DeleteChannelAsync(Guid id, Guid organizationId);
    Task<OutboundMessageResult> SendOutboundMessageAsync(OutboundMessageRequest request);
}
