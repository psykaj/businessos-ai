using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using backend.Modules.CustomerFeedback.Entities;
using backend.Modules.CustomerFeedback.Sentiment.DTOs;
using backend.Modules.CustomerFeedback.Sentiment.Interfaces;

namespace backend.Modules.CustomerFeedback.Sentiment.Services;

public class SentimentService : ISentimentService
{
    private readonly ISentimentRepository _repository;
    private readonly ISentimentAnalysisEngine _engine;
    private readonly IMapper _mapper;

    public SentimentService(ISentimentRepository repository, ISentimentAnalysisEngine engine, IMapper mapper)
    {
        _repository = repository;
        _engine = engine;
        _mapper = mapper;
    }

    public async Task<SentimentResultDto> AnalyzeTextAsync(SentimentAnalyzeRequestDto request)
    {
        Enum.TryParse<SentimentTargetType>(request.TargetEntityType, true, out var targetType);

        var entity = await _engine.AnalyzeAsync(request.OrganizationId, targetType, request.TargetEntityId, request.TextToAnalyze);
        await _repository.AddAsync(entity);
        return _mapper.Map<SentimentResultDto>(entity);
    }

    public async Task<IEnumerable<SentimentResultDto>> GetByTargetAsync(Guid organizationId, string targetType, Guid targetId)
    {
        Enum.TryParse<SentimentTargetType>(targetType, true, out var parsedType);
        var items = await _repository.GetByTargetAsync(organizationId, parsedType, targetId);
        return _mapper.Map<IEnumerable<SentimentResultDto>>(items);
    }
}
