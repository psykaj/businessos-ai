using AutoMapper;
using backend.Modules.Transfers.DTOs;
using backend.Modules.Transfers.Entities;

namespace backend.Modules.Transfers.Mappings;

public class TransferMappingProfile : Profile
{
    public TransferMappingProfile()
    {
        CreateMap<WarehouseTransfer, WarehouseTransferResponseDto>();
    }
}
