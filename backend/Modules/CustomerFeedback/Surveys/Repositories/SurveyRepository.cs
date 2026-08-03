using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Modules.CustomerFeedback.Entities;
using backend.Modules.CustomerFeedback.Surveys.Interfaces;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.CustomerFeedback.Surveys.Repositories;

public class SurveyRepository : ISurveyRepository
{
    private readonly ApplicationDbContext _context;

    public SurveyRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Survey?> GetByIdAsync(Guid id, Guid organizationId)
    {
        return await _context.Surveys
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id && s.OrganizationId == organizationId && !s.IsDeleted);
    }

    public async Task<IEnumerable<Survey>> GetAllAsync(Guid organizationId, bool onlyActive = false)
    {
        var query = _context.Surveys
            .AsNoTracking()
            .Where(s => s.OrganizationId == organizationId && !s.IsDeleted);

        if (onlyActive)
        {
            query = query.Where(s => s.IsActive && (s.ValidUntil == null || s.ValidUntil > DateTime.UtcNow));
        }

        return await query.OrderByDescending(s => s.CreatedAt).ToListAsync();
    }

    public async Task<IEnumerable<SurveyQuestion>> GetQuestionsAsync(Guid surveyId)
    {
        return await _context.SurveyQuestions
            .AsNoTracking()
            .Where(q => q.SurveyId == surveyId && !q.IsDeleted)
            .OrderBy(q => q.OrderIndex)
            .ToListAsync();
    }

    public async Task<IEnumerable<SurveyResponse>> GetResponsesAsync(Guid surveyId, Guid organizationId)
    {
        return await _context.SurveyResponses
            .AsNoTracking()
            .Where(r => r.SurveyId == surveyId && r.OrganizationId == organizationId && !r.IsDeleted)
            .OrderByDescending(r => r.SubmittedAt)
            .ToListAsync();
    }

    public async Task<Survey> AddSurveyAsync(Survey survey, IEnumerable<SurveyQuestion> questions)
    {
        await _context.Surveys.AddAsync(survey);
        await _context.SurveyQuestions.AddRangeAsync(questions);
        await _context.SaveChangesAsync();
        return survey;
    }

    public async Task<SurveyResponse> AddResponseAsync(SurveyResponse response)
    {
        await _context.SurveyResponses.AddAsync(response);
        await _context.SaveChangesAsync();
        return response;
    }

    public async Task UpdateSurveyAsync(Survey survey)
    {
        _context.Surveys.Update(survey);
        await _context.SaveChangesAsync();
    }
}
