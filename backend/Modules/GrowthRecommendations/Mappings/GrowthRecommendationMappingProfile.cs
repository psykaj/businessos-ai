using AutoMapper;
using backend.Modules.GrowthRecommendations.DTOs;
using backend.Modules.GrowthRecommendations.Entities;

namespace backend.Modules.GrowthRecommendations.Mappings;

public class GrowthRecommendationMappingProfile : Profile
{
    public GrowthRecommendationMappingProfile()
    {
        CreateMap<GrowthRecommendation, GrowthRecommendationDto>();
    }
}
