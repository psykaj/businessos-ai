using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Modules.CustomerFeedback.Entities;
using backend.Modules.CustomerFeedback.Surveys.DTOs;

namespace backend.Modules.CustomerFeedback.Surveys.Interfaces;

public interface ISurveyRepository
{
    Task<Survey?> GetByIdAsync(Guid id, Guid organizationId);
    Task<IEnumerable<Survey>> GetAllAsync(Guid organizationId, bool onlyActive = false);
    Task<IEnumerable<SurveyQuestion>> GetQuestionsAsync(Guid surveyId);
    Task<IEnumerable<SurveyResponse>> GetResponsesAsync(Guid surveyId, Guid organizationId);
    Task<Survey> AddSurveyAsync(Survey survey, IEnumerable<SurveyQuestion> questions);
    Task<SurveyResponse> AddResponseAsync(SurveyResponse response);
    Task UpdateSurveyAsync(Survey survey);
}

public interface ISurveyService
{
    Task<SurveyDto> CreateSurveyAsync(CreateSurveyRequestDto request);
    Task<SurveyDto?> GetSurveyAsync(Guid id, Guid organizationId);
    Task<IEnumerable<SurveyDto>> GetSurveysAsync(Guid organizationId, bool onlyActive = false);
    Task<SurveyResponseDto> SubmitResponseAsync(Guid surveyId, SubmitSurveyResponseDto request);
    Task<IEnumerable<SurveyResponseDto>> GetResponsesAsync(Guid surveyId, Guid organizationId);
}
