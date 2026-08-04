using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using backend.Modules.CustomerFeedback.Entities;
using backend.Modules.CustomerFeedback.Sentiment.Interfaces;
using backend.Modules.CustomerFeedback.Surveys.DTOs;
using backend.Modules.CustomerFeedback.Surveys.Interfaces;
using Microsoft.Extensions.Logging;

namespace backend.Modules.CustomerFeedback.Surveys.Services;

public class SurveyService : ISurveyService
{
    private readonly ISurveyRepository _repository;
    private readonly ISentimentAnalysisEngine _sentimentEngine;
    private readonly ISentimentRepository _sentimentRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<SurveyService> _logger;

    public SurveyService(
        ISurveyRepository repository,
        ISentimentAnalysisEngine sentimentEngine,
        ISentimentRepository sentimentRepository,
        IMapper mapper,
        ILogger<SurveyService> logger)
    {
        _repository = repository;
        _sentimentEngine = sentimentEngine;
        _sentimentRepository = sentimentRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<SurveyDto> CreateSurveyAsync(CreateSurveyRequestDto request)
    {
        Enum.TryParse<SurveyType>(request.Type, true, out var surveyType);
        var survey = new Survey
        {
            OrganizationId = request.OrganizationId,
            Title = request.Title,
            Description = request.Description,
            Type = surveyType,
            ValidUntil = request.ValidUntil,
            IsActive = true
        };

        var questions = new List<SurveyQuestion>();
        int index = 1;
        foreach (var q in request.Questions)
        {
            Enum.TryParse<SurveyQuestionType>(q.QuestionType, true, out var qType);
            questions.Add(new SurveyQuestion
            {
                OrganizationId = request.OrganizationId,
                SurveyId = survey.Id,
                QuestionText = q.QuestionText,
                QuestionType = qType,
                OrderIndex = q.OrderIndex > 0 ? q.OrderIndex : index++,
                IsRequired = q.IsRequired,
                OptionsJson = q.OptionsJson
            });
        }

        await _repository.AddSurveyAsync(survey, questions);
        return await GetSurveyAsync(survey.Id, survey.OrganizationId) ?? _mapper.Map<SurveyDto>(survey);
    }

    public async Task<SurveyDto?> GetSurveyAsync(Guid id, Guid organizationId)
    {
        var survey = await _repository.GetByIdAsync(id, organizationId);
        if (survey == null) return null;

        var dto = _mapper.Map<SurveyDto>(survey);
        var questions = await _repository.GetQuestionsAsync(id);
        dto.Questions = _mapper.Map<List<SurveyQuestionDto>>(questions);
        return dto;
    }

    public async Task<IEnumerable<SurveyDto>> GetSurveysAsync(Guid organizationId, bool onlyActive = false)
    {
        var list = await _repository.GetAllAsync(organizationId, onlyActive);
        return _mapper.Map<IEnumerable<SurveyDto>>(list);
    }

    public async Task<SurveyResponseDto> SubmitResponseAsync(Guid surveyId, SubmitSurveyResponseDto request)
    {
        var response = new SurveyResponse
        {
            OrganizationId = request.OrganizationId,
            SurveyId = surveyId,
            CustomerId = request.CustomerId,
            CustomerEmail = request.CustomerEmail,
            SubmittedAt = DateTime.UtcNow,
            Score = request.Score,
            AnswersJson = request.AnswersJson
        };

        // Automated AI Sentiment analysis on qualitative answers
        var analysis = await _sentimentEngine.AnalyzeAsync(request.OrganizationId, SentimentTargetType.SurveyResponse, response.Id, request.AnswersJson);
        response.SentimentStatus = analysis.Sentiment.ToString();
        await _sentimentRepository.AddAsync(analysis);
        await _repository.AddResponseAsync(response);

        // Update survey aggregated completion score
        var survey = await _repository.GetByIdAsync(surveyId, request.OrganizationId);
        if (survey != null)
        {
            survey.TotalCompleted++;
            if (survey.TotalSent > 0)
            {
                survey.CompletionRate = (decimal)survey.TotalCompleted / survey.TotalSent * 100m;
            }
            if (request.Score.HasValue)
            {
                // Running average update
                survey.AverageScore = ((survey.AverageScore * (survey.TotalCompleted - 1)) + request.Score.Value) / survey.TotalCompleted;
            }
            await _repository.UpdateSurveyAsync(survey);
        }

        return _mapper.Map<SurveyResponseDto>(response);
    }

    public async Task<IEnumerable<SurveyResponseDto>> GetResponsesAsync(Guid surveyId, Guid organizationId)
    {
        var items = await _repository.GetResponsesAsync(surveyId, organizationId);
        return _mapper.Map<IEnumerable<SurveyResponseDto>>(items);
    }
}
