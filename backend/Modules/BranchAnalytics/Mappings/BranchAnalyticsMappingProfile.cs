using AutoMapper;
using backend.Modules.BranchAnalytics.DTOs;
using backend.Modules.BranchAnalytics.Entities;

namespace backend.Modules.BranchAnalytics.Mappings;

public class BranchAnalyticsMappingProfile : Profile
{
    public BranchAnalyticsMappingProfile()
    {
        CreateMap<BranchPerformance, BranchPerformanceResponseDto>();
    }
}
