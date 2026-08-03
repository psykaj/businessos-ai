using AutoMapper;
using backend.Modules.CustomerFeedback.Entities;
using backend.Modules.CustomerFeedback.Sentiment.DTOs;

namespace backend.Modules.CustomerFeedback.Sentiment.Mappings;

public class SentimentMappingProfile : Profile
{
    public SentimentMappingProfile()
    {
        CreateMap<SentimentAnalysis, SentimentResultDto>()
            .ForMember(dest => dest.Sentiment, opt => opt.MapFrom(src => src.Sentiment.ToString()))
            .ForMember(dest => dest.TargetEntityType, opt => opt.MapFrom(src => src.TargetEntityType.ToString()));
    }
}
