using AutoMapper;
using backend.Modules.CommunicationHub.Entities;
using backend.Modules.CommunicationHub.Messages.DTOs;

namespace backend.Modules.CommunicationHub.Messages.Mappings;

public class MessageMappingProfile : Profile
{
    public MessageMappingProfile()
    {
        CreateMap<Message, MessageDto>();
    }
}
