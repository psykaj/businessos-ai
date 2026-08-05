using AutoMapper;
using backend.Modules.Branches.Entities;
using backend.Modules.BranchSettings.DTOs;

namespace backend.Modules.BranchSettings.Mappings;

public class BranchSettingsMappingProfile : Profile
{
    public BranchSettingsMappingProfile()
    {
        CreateMap<Branch, BranchConfigurationResponseDto>()
            .ForCtorParam("BranchId", opt => opt.MapFrom(src => src.Id))
            .ForCtorParam("BranchName", opt => opt.MapFrom(src => src.Name))
            .ForCtorParam("BranchCode", opt => opt.MapFrom(src => src.Code))
            .ForCtorParam("WorkingHoursJson", opt => opt.MapFrom(src => src.WorkingHoursJson))
            .ForCtorParam("OperationalSettingsJson", opt => opt.MapFrom(src => src.OperationalSettingsJson));
    }
}
