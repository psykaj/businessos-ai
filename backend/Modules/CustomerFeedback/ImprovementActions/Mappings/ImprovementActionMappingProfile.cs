using AutoMapper;
using backend.Modules.CustomerFeedback.Entities;
using backend.Modules.CustomerFeedback.ImprovementActions.DTOs;

namespace backend.Modules.CustomerFeedback.ImprovementActions.Mappings;

public class ImprovementActionMappingProfile : Profile
{
    public ImprovementActionMappingProfile()
    {
        CreateMap<ImprovementRecommendation, ImprovementRecommendationDto>()
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.ToString()))
            .ForMember(dest => dest.Priority, opt => opt.MapFrom(src => src.Priority.ToString()))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
    }
}
