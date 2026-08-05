using AutoMapper;
using backend.Modules.RegionalReports.DTOs;
using backend.Modules.RegionalReports.Entities;

namespace backend.Modules.RegionalReports.Mappings;

public class RegionalReportMappingProfile : Profile
{
    public RegionalReportMappingProfile()
    {
        CreateMap<RegionalSummary, RegionalOverviewResponseDto>()
            .ForCtorParam("Id", opt => opt.MapFrom(src => src.Id))
            .ForCtorParam("Region", opt => opt.MapFrom(src => src.Region))
            .ForCtorParam("Month", opt => opt.MapFrom(src => src.Month))
            .ForCtorParam("Year", opt => opt.MapFrom(src => src.Year))
            .ForCtorParam("TotalBranches", opt => opt.MapFrom(src => src.TotalBranches))
            .ForCtorParam("TotalRevenue", opt => opt.MapFrom(src => src.TotalRevenue))
            .ForCtorParam("TotalProfit", opt => opt.MapFrom(src => src.TotalProfit))
            .ForCtorParam("TotalInventoryValue", opt => opt.MapFrom(src => src.TotalInventoryValue))
            .ForCtorParam("TotalCustomers", opt => opt.MapFrom(src => src.TotalCustomers))
            .ForCtorParam("TotalEmployees", opt => opt.MapFrom(src => src.TotalEmployees))
            .ForCtorParam("TopPerformingBranchName", opt => opt.MapFrom(src => src.TopPerformingBranchName));
    }
}
