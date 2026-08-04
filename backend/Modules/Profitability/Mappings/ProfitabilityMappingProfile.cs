using AutoMapper;
using backend.Modules.Profitability.DTOs;
using backend.Modules.Profitability.Entities;

namespace backend.Modules.Profitability.Mappings;

public class ProfitabilityMappingProfile : Profile
{
    public ProfitabilityMappingProfile()
    {
        CreateMap<ProfitSnapshot, ProfitSnapshotDto>();
    }
}
