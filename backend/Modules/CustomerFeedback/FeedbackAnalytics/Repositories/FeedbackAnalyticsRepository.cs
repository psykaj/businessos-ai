using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Modules.CustomerFeedback.Entities;
using backend.Modules.CustomerFeedback.FeedbackAnalytics.Interfaces;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.CustomerFeedback.FeedbackAnalytics.Repositories;

public class FeedbackAnalyticsRepository : IFeedbackAnalyticsRepository
{
    private readonly ApplicationDbContext _context;

    public FeedbackAnalyticsRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<SentimentAnalysis>> GetRecentSentimentsAsync(Guid organizationId, int days)
    {
        var cutoff = DateTime.UtcNow.AddDays(-days);
        return await _context.SentimentAnalyses
            .AsNoTracking()
            .Where(s => s.OrganizationId == organizationId && s.CreatedAt >= cutoff && !s.IsDeleted)
            .ToListAsync();
    }

    public async Task<IEnumerable<Entities.Feedback>> GetRecentFeedbacksAsync(Guid organizationId, int days)
    {
        var cutoff = DateTime.UtcNow.AddDays(-days);
        return await _context.Feedbacks
            .AsNoTracking()
            .Where(f => f.OrganizationId == organizationId && f.CreatedAt >= cutoff && !f.IsDeleted)
            .ToListAsync();
    }
}
