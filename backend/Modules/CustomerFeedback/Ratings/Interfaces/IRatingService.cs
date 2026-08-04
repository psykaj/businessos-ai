using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Modules.CustomerFeedback.Entities;
using backend.Modules.CustomerFeedback.Ratings.DTOs;

namespace backend.Modules.CustomerFeedback.Ratings.Interfaces;

public interface IRatingRepository
{
    Task<IEnumerable<Rating>> GetByEntityAsync(Guid organizationId, RatingEntityType entityType, string entityId);
    Task<IEnumerable<Rating>> GetRecentAsync(Guid organizationId, int limit);
    Task<Rating> AddAsync(Rating entity);
}

public interface IRatingService
{
    Task<RatingDto> SubmitRatingAsync(SubmitRatingRequestDto request);
    Task<RatingSummaryDto> GetSummaryAsync(Guid organizationId, string entityType, string entityId);
    Task<IEnumerable<RatingDto>> GetRecentRatingsAsync(Guid organizationId, int limit = 20);
}
