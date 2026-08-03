using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using backend.Modules.CustomerFeedback.Entities;
using backend.Modules.CustomerFeedback.Feedback.DTOs;
using backend.Modules.CustomerFeedback.Feedback.Interfaces;
using backend.Modules.CustomerFeedback.Sentiment.Interfaces;
using Microsoft.Extensions.Logging;

namespace backend.Modules.CustomerFeedback.Feedback.Services;

public class FeedbackService : IFeedbackService
{
    private readonly IFeedbackRepository _repository;
    private readonly ISentimentAnalysisEngine _sentimentEngine;
    private readonly ISentimentRepository _sentimentRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<FeedbackService> _logger;

    public FeedbackService(
        IFeedbackRepository repository,
        ISentimentAnalysisEngine sentimentEngine,
        ISentimentRepository sentimentRepository,
        IMapper mapper,
        ILogger<FeedbackService> logger)
    {
        _repository = repository;
        _sentimentEngine = sentimentEngine;
        _sentimentRepository = sentimentRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<FeedbackDto> SubmitFeedbackAsync(SubmitFeedbackRequestDto request)
    {
        Enum.TryParse<FeedbackSource>(request.Source, true, out var source);

        var entity = new Entities.Feedback
        {
            OrganizationId = request.OrganizationId,
            CustomerId = request.CustomerId,
            CustomerName = request.CustomerName,
            CustomerEmail = request.CustomerEmail,
            Source = source,
            Type = string.IsNullOrWhiteSpace(request.Type) ? "General" : request.Type,
            Comment = request.Comment,
            RatingValue = request.RatingValue,
            Status = FeedbackStatus.New
        };

        // Save initial feedback entity
        entity = await _repository.AddAsync(entity);

        // Instant automated AI sentiment analysis to save business owners triage time
        var analysis = await _sentimentEngine.AnalyzeAsync(entity.OrganizationId, SentimentTargetType.Feedback, entity.Id, entity.Comment);
        await _sentimentRepository.AddAsync(analysis);

        entity.IsUrgent = analysis.UrgencyScore >= 7.0m || (entity.RatingValue.HasValue && entity.RatingValue <= 2);
        entity.HasBeenAnalyzed = true;
        await _repository.UpdateAsync(entity);

        var dto = _mapper.Map<FeedbackDto>(entity);
        dto.SentimentClassification = analysis.Sentiment.ToString();
        dto.UrgencyScore = analysis.UrgencyScore;
        dto.SuggestedAction = analysis.SuggestedImprovement;
        return dto;
    }

    public async Task<FeedbackDto?> GetByIdAsync(Guid id, Guid organizationId)
    {
        var entity = await _repository.GetByIdAsync(id, organizationId);
        if (entity == null) return null;

        var dto = _mapper.Map<FeedbackDto>(entity);
        var sentimentList = await _sentimentRepository.GetByTargetAsync(organizationId, SentimentTargetType.Feedback, entity.Id);
        var latestSentiment = sentimentList.FirstOrDefault();
        if (latestSentiment != null)
        {
            dto.SentimentClassification = latestSentiment.Sentiment.ToString();
            dto.UrgencyScore = latestSentiment.UrgencyScore;
            dto.SuggestedAction = latestSentiment.SuggestedImprovement;
        }

        return dto;
    }

    public async Task<FeedbackPagedResultDto> SearchAsync(FeedbackSearchFilterDto filter)
    {
        var (items, total) = await _repository.SearchAsync(filter);
        var dtos = _mapper.Map<IEnumerable<FeedbackDto>>(items).ToList();

        return new FeedbackPagedResultDto
        {
            Items = dtos,
            TotalCount = total,
            PageNumber = filter.PageNumber,
            PageSize = filter.PageSize
        };
    }

    public async Task<string> ExportToCsvAsync(Guid organizationId, DateTime? startDate, DateTime? endDate)
    {
        var feedbacks = await _repository.GetForExportAsync(organizationId, startDate, endDate);
        var sb = new StringBuilder();
        sb.AppendLine("Id,Created,CustomerName,Email,Source,Type,Rating,Status,Urgent,Comment,ResolvedAt,Notes");

        foreach (var f in feedbacks)
        {
            var comment = f.Comment.Replace("\"", "\"\"");
            var notes = f.ResolutionNotes?.Replace("\"", "\"\"") ?? "";
            sb.AppendLine($"{f.Id},{f.CreatedAt:yyyy-MM-dd HH:mm:ss},\"{f.CustomerName}\",\"{f.CustomerEmail}\",{f.Source},{f.Type},{f.RatingValue},{f.Status},{f.IsUrgent},\"{comment}\",{f.ResolvedAt:yyyy-MM-dd HH:mm:ss},\"{notes}\"");
        }

        return sb.ToString();
    }

    public async Task<FeedbackDto?> UpdateStatusAsync(Guid id, Guid organizationId, string status, string? resolutionNotes)
    {
        var entity = await _repository.GetByIdAsync(id, organizationId);
        if (entity == null) return null;

        if (Enum.TryParse<FeedbackStatus>(status, true, out var newStatus))
        {
            entity.Status = newStatus;
            if (newStatus == FeedbackStatus.Resolved)
            {
                entity.ResolvedAt = DateTime.UtcNow;
                entity.ResolutionNotes = resolutionNotes;
            }
            await _repository.UpdateAsync(entity);
        }

        return _mapper.Map<FeedbackDto>(entity);
    }
}
