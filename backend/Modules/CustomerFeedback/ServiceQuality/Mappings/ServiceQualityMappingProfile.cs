using AutoMapper;
using backend.Modules.CustomerFeedback.Entities;
using backend.Modules.CustomerFeedback.ServiceQuality.DTOs;

namespace backend.Modules.CustomerFeedback.ServiceQuality.Mappings;

public class ServiceQualityMappingProfile : Profile
{
    public ServiceQualityMappingProfile()
    {
        CreateMap<ServiceMetric, ServiceMetricDto>();
    }
}
