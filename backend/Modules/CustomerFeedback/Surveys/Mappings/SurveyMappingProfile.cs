using AutoMapper;
using backend.Modules.CustomerFeedback.Entities;
using backend.Modules.CustomerFeedback.Surveys.DTOs;

namespace backend.Modules.CustomerFeedback.Surveys.Mappings;

public class SurveyMappingProfile : Profile
{
    public SurveyMappingProfile()
    {
        CreateMap<Survey, SurveyDto>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()))
            .ForMember(dest => dest.Questions, opt => opt.Ignore());

        CreateMap<SurveyQuestion, SurveyQuestionDto>()
            .ForMember(dest => dest.QuestionType, opt => opt.MapFrom(src => src.QuestionType.ToString()));

        CreateMap<SurveyResponse, SurveyResponseDto>();
    }
}
