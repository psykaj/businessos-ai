using AutoMapper;
using backend.Modules.CustomerAnalytics.DTOs;
using backend.Modules.CustomerAnalytics.Entities;

namespace backend.Modules.CustomerAnalytics.Mappings;

public class CustomerAnalyticsMappingProfile : Profile
{
    public CustomerAnalyticsMappingProfile()
    {
        CreateMap<CustomerPerformance, CustomerPerformanceDto>();
    }
}
