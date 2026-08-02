using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using backend.Modules.CommunicationHub.Channels.DTOs;
using backend.Modules.CommunicationHub.Channels.Interfaces;
using backend.Modules.CommunicationHub.Entities;
using Microsoft.Extensions.Logging;

namespace backend.Modules.CommunicationHub.Channels.Services;

public class ChannelService : IChannelService
{
    private readonly IChannelRepository _repository;
    private readonly IChannelAdapterFactory _adapterFactory;
    private readonly IMapper _mapper;
    private readonly ILogger<ChannelService> _logger;

    public ChannelService(
        IChannelRepository repository,
        IChannelAdapterFactory adapterFactory,
        IMapper mapper,
        ILogger<ChannelService> logger)
    {
        _repository = repository;
        _adapterFactory = adapterFactory;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<ChannelDto>> GetChannelsAsync(Guid organizationId)
    {
        var channels = await _repository.GetByOrganizationIdAsync(organizationId);
        return _mapper.Map<IEnumerable<ChannelDto>>(channels);
    }

    public async Task<ChannelDto?> GetChannelByIdAsync(Guid id, Guid organizationId)
    {
        var channel = await _repository.GetByIdAsync(id, organizationId);
        if (channel == null) return null;
        return _mapper.Map<ChannelDto>(channel);
    }

    public async Task<ChannelDto> CreateChannelAsync(Guid organizationId, CreateChannelRequest request, string baseWebhookDomain)
    {
        var entity = new CommunicationChannel
        {
            OrganizationId = organizationId,
            Name = request.Name,
            ChannelType = request.ChannelType,
            ProviderIdentifier = request.ProviderIdentifier,
            ConfigurationJson = request.ConfigurationJson,
            IsDefault = request.IsDefault,
            IsActive = true
        };

        // Generate dynamic secure webhook ingestion URL for this channel provider
        entity.WebhookUrl = $"{baseWebhookDomain.TrimEnd('/')}/api/v1/inbox/webhooks/{entity.ChannelType.ToString().ToLower()}/{organizationId}";

        await _repository.CreateAsync(entity);
        _logger.LogInformation("Created communication channel {Name} [{Type}] for organization {OrgId}", entity.Name, entity.ChannelType, organizationId);

        return _mapper.Map<ChannelDto>(entity);
    }

    public async Task<ChannelDto?> UpdateChannelAsync(Guid id, Guid organizationId, UpdateChannelRequest request)
    {
        var channel = await _repository.GetByIdAsync(id, organizationId);
        if (channel == null) return null;

        channel.Name = request.Name;
        channel.ProviderIdentifier = request.ProviderIdentifier;
        channel.ConfigurationJson = request.ConfigurationJson;
        channel.IsActive = request.IsActive;
        channel.IsDefault = request.IsDefault;

        await _repository.UpdateAsync(channel);
        return _mapper.Map<ChannelDto>(channel);
    }

    public async Task<bool> DeleteChannelAsync(Guid id, Guid organizationId)
    {
        var channel = await _repository.GetByIdAsync(id, organizationId);
        if (channel == null) return false;

        await _repository.DeleteAsync(channel);
        return true;
    }

    public async Task<OutboundMessageResult> SendOutboundMessageAsync(OutboundMessageRequest request)
    {
        var adapter = _adapterFactory.GetAdapter(request.ChannelType);
        return await adapter.SendMessageAsync(request);
    }
}
