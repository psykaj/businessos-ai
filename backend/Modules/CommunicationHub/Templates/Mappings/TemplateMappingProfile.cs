using AutoMapper;
using backend.Modules.CommunicationHub.Entities;
using backend.Modules.CommunicationHub.Templates.DTOs;

namespace backend.Modules.CommunicationHub.Templates.Mappings;

public class TemplateMappingProfile : Profile
{
    public TemplateMappingProfile()
    {
        CreateMap<MessageTemplate, MessageTemplateDto>();
    }
}
