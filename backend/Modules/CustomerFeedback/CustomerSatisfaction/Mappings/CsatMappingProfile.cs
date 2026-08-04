using AutoMapper;
using backend.Modules.CustomerFeedback.Entities;
using backend.Modules.CustomerFeedback.CustomerSatisfaction.DTOs;

namespace backend.Modules.CustomerFeedback.CustomerSatisfaction.Mappings;

public class CsatMappingProfile : Profile
{
    public CsatMappingProfile()
    {
        CreateMap<CustomerSatisfactionScore, CustomerCsatDto>()
            .ForMember(dest => dest.ChurnRiskScore, opt => opt.MapFrom(src => src.ChurnRiskScore.ToString()));
    }
}
