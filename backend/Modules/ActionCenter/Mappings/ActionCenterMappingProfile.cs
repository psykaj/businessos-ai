using AutoMapper;
using backend.Modules.ActionCenter.DTOs;
using backend.Modules.ActionCenter.Entities;

namespace backend.Modules.ActionCenter.Mappings;

public class ActionCenterMappingProfile : Profile
{
    public ActionCenterMappingProfile()
    {
        CreateMap<AiAction, AiActionDto>();
    }
}
