using AutoMapper;
using backend.Modules.CustomerFeedback.Entities;
using backend.Modules.CustomerFeedback.Feedback.DTOs;

namespace backend.Modules.CustomerFeedback.Feedback.Mappings;

public class FeedbackMappingProfile : Profile
{
    public FeedbackMappingProfile()
    {
        CreateMap<Entities.Feedback, FeedbackDto>()
            .ForMember(dest => dest.Source, opt => opt.MapFrom(src => src.Source.ToString()))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
    }
}
