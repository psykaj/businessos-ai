using AutoMapper;
using backend.Modules.BusinessPerformance.DTOs;
using backend.Modules.BusinessPerformance.Entities;

namespace backend.Modules.BusinessPerformance.Mappings;

public class BusinessPerformanceMappingProfile : Profile
{
    public BusinessPerformanceMappingProfile()
    {
        CreateMap<BusinessMetric, BusinessMetricDto>();
    }
}
