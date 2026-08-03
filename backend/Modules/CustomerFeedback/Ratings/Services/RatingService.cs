using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using backend.Modules.CustomerFeedback.Entities;
using backend.Modules.CustomerFeedback.Ratings.DTOs;
using backend.Modules.CustomerFeedback.Ratings.Interfaces;
using backend.Modules.CustomerFeedback.Sentiment.Interfaces;

namespace backend.Modules.CustomerFeedback.Ratings.Services;

public class RatingService : IRatingService
{
    private readonly IRatingRepository _repository;
    private readonly ISentimentAnalysisEngine _sentimentEngine;
    private readonly ISentimentRepository _sentimentRepository;
    private readonly IMapper _mapper;

    public RatingService(
        IRatingRepository repository,
        ISentimentAnalysisEngine sentimentEngine,
        ISentimentRepository sentimentRepository,
        IMapper mapper)
    {
        _repository = repository;
        _sentimentEngine = sentimentEngine;
        _sentimentRepository = sentimentRepository;
        _mapper = mapper;
    }

    public async Task<RatingDto> SubmitRatingAsync(SubmitRatingRequestDto request)
    {
        Enum.TryParse<RatingEntityType>(request.EntityType, true, out var entityType);

        var entity = new Rating
        {
            OrganizationId = request.OrganizationId,
            EntityType = entityType,
            EntityId = request.EntityId,
            CustomerId = request.CustomerId,
            CustomerEmail = request.CustomerEmail,
            RatingScore = request.RatingScore,
            MaxScore = request.MaxScore,
            ReviewText = request.ReviewText,
            VerifiedPurchase = request.VerifiedPurchase
        };

        entity = await _repository.AddAsync(entity);

        // Run sentiment engine on review text if provided
        if (!string.IsNullOrWhiteSpace(entity.ReviewText))
        {
            var analysis = await _sentimentEngine.AnalyzeAsync(entity.OrganizationId, SentimentTargetType.Rating, entity.Id, entity.ReviewText);
            await _sentimentRepository.AddAsync(analysis);
        }

        return _mapper.Map<RatingDto>(entity);
    }

    public async Task<RatingSummaryDto> GetSummaryAsync(Guid organizationId, string entityType, string entityId)
    {
        Enum.TryParse<RatingEntityType>(entityType, true, out var parsedType);
        var ratings = (await _repository.GetByEntityAsync(organizationId, parsedType, entityId)).ToList();

        var summary = new RatingSummaryDto
        {
            EntityType = entityType,
            EntityId = entityId,
            TotalRatings = ratings.Count,
            AverageRating = ratings.Count > 0 ? Math.Round(ratings.Average(r => r.RatingScore), 2) : 0m
        };

        foreach (var r in ratings)
        {
            int rounded = (int)Math.Round(r.RatingScore);
            if (rounded == 5) summary.FiveStarCount++;
            else if (rounded == 4) summary.FourStarCount++;
            else if (rounded == 3) summary.ThreeStarCount++;
            else if (rounded == 2) summary.TwoStarCount++;
            else if (rounded == 1) summary.OneStarCount++;
        }

        return summary;
    }

    public async Task<IEnumerable<RatingDto>> GetRecentRatingsAsync(Guid organizationId, int limit = 20)
    {
        var list = await _repository.GetRecentAsync(organizationId, limit);
        return _mapper.Map<IEnumerable<RatingDto>>(list);
    }
}
