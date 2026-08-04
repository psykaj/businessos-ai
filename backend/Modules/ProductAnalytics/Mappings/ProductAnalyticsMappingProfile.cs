using AutoMapper;
using backend.Modules.ProductAnalytics.DTOs;
using backend.Modules.ProductAnalytics.Entities;

namespace backend.Modules.ProductAnalytics.Mappings;

public class ProductAnalyticsMappingProfile : Profile
{
    public ProductAnalyticsMappingProfile()
    {
        CreateMap<ProductPerformance, ProductPerformanceDto>();
    }
}
