using AutoMapper;
using backend.Modules.BusinessIntelligence.DTOs;
using backend.Modules.BusinessIntelligence.Entities;

namespace backend.Modules.BusinessIntelligence.Mappings;

public class BusinessIntelligenceMappingProfile : Profile
{
    public BusinessIntelligenceMappingProfile()
    {
        CreateMap<KPI, KPIDto>();
        CreateMap<Goal, GoalDto>()
            .ForMember(dest => dest.ProgressPercentage, opt => opt.MapFrom(src => src.TargetValue > 0 ? (double)(src.CurrentValue / src.TargetValue * 100m) : 0.0));
        CreateMap<Report, ReportDto>();
        CreateMap<Insight, InsightDto>();
        CreateMap<Forecast, ForecastDto>();
    }
}
