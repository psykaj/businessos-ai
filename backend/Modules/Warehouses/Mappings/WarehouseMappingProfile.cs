using AutoMapper;
using backend.Modules.Warehouses.DTOs;
using backend.Modules.Warehouses.Entities;

namespace backend.Modules.Warehouses.Mappings;

public class WarehouseMappingProfile : Profile
{
    public WarehouseMappingProfile()
    {
        CreateMap<Warehouse, WarehouseResponseDto>();
        CreateMap<CreateWarehouseDto, Warehouse>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.OrganizationId, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => WarehouseStatus.Operational))
            .ForMember(dest => dest.CurrentUtilizationPercentage, opt => opt.MapFrom(src => 0m))
            .ForMember(dest => dest.TotalStockItemsCount, opt => opt.MapFrom(src => 0))
            .ForMember(dest => dest.EstimatedStockValue, opt => opt.MapFrom(src => 0m))
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
            .ForMember(dest => dest.DeletedAt, opt => opt.Ignore())
            .ForMember(dest => dest.RowVersion, opt => opt.Ignore());

        CreateMap<UpdateWarehouseDto, Warehouse>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.OrganizationId, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
            .ForMember(dest => dest.DeletedAt, opt => opt.Ignore())
            .ForMember(dest => dest.RowVersion, opt => opt.Ignore());
    }
}
