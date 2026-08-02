using System;
using System.Collections.Generic;
using System.Linq;
using backend.Modules.CommunicationHub.Channels.Interfaces;
using backend.Modules.CommunicationHub.Entities;

namespace backend.Modules.CommunicationHub.Channels.Services;

public class ChannelAdapterFactory : IChannelAdapterFactory
{
    private readonly IEnumerable<IChannelAdapter> _adapters;

    public ChannelAdapterFactory(IEnumerable<IChannelAdapter> adapters)
    {
        _adapters = adapters;
    }

    public IChannelAdapter GetAdapter(CommunicationChannelType channelType)
    {
        var adapter = _adapters.FirstOrDefault(a => a.SupportedChannelType == channelType);
        if (adapter == null)
        {
            throw new NotSupportedException($"No adapter found registered for communication channel type: {channelType}");
        }
        return adapter;
    }
}
