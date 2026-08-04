using AutoMapper;
using backend.Modules.RevenueAnalytics.DTOs;
using backend.Modules.RevenueAnalytics.Entities;

namespace backend.Modules.RevenueAnalytics.Mappings;

public class RevenueAnalyticsMappingProfile : Profile
{
    public RevenueAnalyticsMappingProfile()
    {
        CreateMap<RevenueSnapshot, RevenueSnapshotDto>();
    }
}
