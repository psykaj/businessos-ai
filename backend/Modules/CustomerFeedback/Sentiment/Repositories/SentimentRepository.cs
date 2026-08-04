using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Modules.CustomerFeedback.Entities;
using backend.Modules.CustomerFeedback.Sentiment.Interfaces;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.CustomerFeedback.Sentiment.Repositories;

public class SentimentRepository : ISentimentRepository
{
    private readonly ApplicationDbContext _context;

    public SentimentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SentimentAnalysis?> GetByIdAsync(Guid id, Guid organizationId)
    {
        return await _context.SentimentAnalyses
            .FirstOrDefaultAsync(s => s.Id == id && s.OrganizationId == organizationId && !s.IsDeleted);
    }

    public async Task<IEnumerable<SentimentAnalysis>> GetByTargetAsync(Guid organizationId, SentimentTargetType targetType, Guid targetId)
    {
        return await _context.SentimentAnalyses
            .Where(s => s.OrganizationId == organizationId && s.TargetEntityType == targetType && s.TargetEntityId == targetId && !s.IsDeleted)
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<SentimentAnalysis>> GetUnprocessedBatchAsync(int batchSize)
    {
        return await _context.SentimentAnalyses
            .Where(s => !s.IsDeleted && s.ConfidenceScore == 0.0m)
            .Take(batchSize)
            .ToListAsync();
    }

    public async Task<SentimentAnalysis> AddAsync(SentimentAnalysis entity)
    {
        await _context.SentimentAnalyses.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(SentimentAnalysis entity)
    {
        _context.SentimentAnalyses.Update(entity);
        await _context.SaveChangesAsync();
    }
}
