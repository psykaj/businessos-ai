using AutoMapper;
using backend.Modules.Branches.DTOs;
using backend.Modules.Branches.Entities;

namespace backend.Modules.Branches.Mappings;

public class BranchMappingProfile : Profile
{
    public BranchMappingProfile()
    {
        CreateMap<Branch, BranchResponseDto>();
        CreateMap<CreateBranchDto, Branch>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.OrganizationId, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => BranchStatus.Active))
            .ForMember(dest => dest.WorkingHoursJson, opt => opt.MapFrom(src => src.WorkingHoursJson ?? "{\"Monday\":\"09:00-17:00\",\"Tuesday\":\"09:00-17:00\",\"Wednesday\":\"09:00-17:00\",\"Thursday\":\"09:00-17:00\",\"Friday\":\"09:00-17:00\",\"Saturday\":\"Closed\",\"Sunday\":\"Closed\"}"))
            .ForMember(dest => dest.OperationalSettingsJson, opt => opt.MapFrom(src => src.OperationalSettingsJson ?? "{\"AllowAutoStockTransfer\":true,\"MaxTransferApprovalAmount\":10000,\"EnablePosSync\":true,\"AutoReorderEnabled\":false}"))
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
            .ForMember(dest => dest.DeletedAt, opt => opt.Ignore())
            .ForMember(dest => dest.RowVersion, opt => opt.Ignore());

        CreateMap<UpdateBranchDto, Branch>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.OrganizationId, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.Ignore())
            .ForMember(dest => dest.WorkingHoursJson, opt => opt.Condition(src => src.WorkingHoursJson != null))
            .ForMember(dest => dest.OperationalSettingsJson, opt => opt.Condition(src => src.OperationalSettingsJson != null))
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
            .ForMember(dest => dest.DeletedAt, opt => opt.Ignore())
            .ForMember(dest => dest.RowVersion, opt => opt.Ignore());

        CreateMap<BranchManager, BranchManagerResponseDto>();
    }
}
