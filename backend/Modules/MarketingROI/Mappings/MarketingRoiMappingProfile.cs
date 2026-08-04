using AutoMapper;
using backend.Modules.MarketingROI.DTOs;
using backend.Modules.MarketingROI.Entities;

namespace backend.Modules.MarketingROI.Mappings;

public class MarketingRoiMappingProfile : Profile
{
    public MarketingRoiMappingProfile()
    {
        CreateMap<MarketingPerformance, MarketingPerformanceDto>();
    }
}
