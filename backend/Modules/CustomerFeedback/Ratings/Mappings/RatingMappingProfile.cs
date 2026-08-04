using AutoMapper;
using backend.Modules.CustomerFeedback.Entities;
using backend.Modules.CustomerFeedback.Ratings.DTOs;

namespace backend.Modules.CustomerFeedback.Ratings.Mappings;

public class RatingMappingProfile : Profile
{
    public RatingMappingProfile()
    {
        CreateMap<Rating, RatingDto>()
            .ForMember(dest => dest.EntityType, opt => opt.MapFrom(src => src.EntityType.ToString()));
    }
}
