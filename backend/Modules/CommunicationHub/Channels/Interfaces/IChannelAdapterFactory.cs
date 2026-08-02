using backend.Modules.CommunicationHub.Entities;

namespace backend.Modules.CommunicationHub.Channels.Interfaces;

public interface IChannelAdapterFactory
{
    IChannelAdapter GetAdapter(CommunicationChannelType channelType);
}
