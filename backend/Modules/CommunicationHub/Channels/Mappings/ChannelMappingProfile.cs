using AutoMapper;
using backend.Modules.CommunicationHub.Channels.DTOs;
using backend.Modules.CommunicationHub.Entities;

namespace backend.Modules.CommunicationHub.Channels.Mappings;

public class ChannelMappingProfile : Profile
{
    public ChannelMappingProfile()
    {
        CreateMap<CommunicationChannel, ChannelDto>();
    }
}
