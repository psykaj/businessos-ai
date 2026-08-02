using AutoMapper;
using backend.Modules.CommunicationHub.Conversations.DTOs;
using backend.Modules.CommunicationHub.Entities;

namespace backend.Modules.CommunicationHub.Conversations.Mappings;

public class ConversationMappingProfile : Profile
{
    public ConversationMappingProfile()
    {
        CreateMap<Conversation, ConversationDto>();
    }
}
