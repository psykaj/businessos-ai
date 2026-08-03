using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Modules.CustomerFeedback.Entities;
using backend.Modules.CustomerFeedback.Sentiment.DTOs;

namespace backend.Modules.CustomerFeedback.Sentiment.Interfaces;

public interface ISentimentAnalysisEngine
{
    Task<SentimentAnalysis> AnalyzeAsync(Guid organizationId, SentimentTargetType targetType, Guid targetId, string text);
}

public interface ISentimentRepository
{
    Task<SentimentAnalysis?> GetByIdAsync(Guid id, Guid organizationId);
    Task<IEnumerable<SentimentAnalysis>> GetByTargetAsync(Guid organizationId, SentimentTargetType targetType, Guid targetId);
    Task<IEnumerable<SentimentAnalysis>> GetUnprocessedBatchAsync(int batchSize);
    Task<SentimentAnalysis> AddAsync(SentimentAnalysis entity);
    Task UpdateAsync(SentimentAnalysis entity);
}

public interface ISentimentService
{
    Task<SentimentResultDto> AnalyzeTextAsync(SentimentAnalyzeRequestDto request);
    Task<IEnumerable<SentimentResultDto>> GetByTargetAsync(Guid organizationId, string targetType, Guid targetId);
}
