using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using backend.Persistence;
using backend.Modules.BusinessMemory.Interfaces;
using backend.Modules.BusinessMemory.Entities;

namespace backend.Modules.BusinessMemory.Services;

public class MemoryRetrievalService : IMemoryRetrievalService
{
    private readonly ApplicationDbContext _context;
    private readonly IMemoryRelevanceService _relevanceService;

    public MemoryRetrievalService(ApplicationDbContext context, IMemoryRelevanceService relevanceService)
    {
        _context = context;
        _relevanceService = relevanceService;
    }

    public async Task<IEnumerable<Entities.BusinessMemory>> RetrieveRelevantMemoriesAsync(
        Guid organizationId, 
        string question, 
        string? entityType = null, 
        string? entityId = null, 
        int limit = 10, 
        CancellationToken cancellationToken = default)
    {
        var query = _context.BusinessMemories
            .AsNoTracking()
            .Where(m => m.OrganizationId == organizationId 
                        && m.IsActive 
                        && (m.ExpiresAt == null || m.ExpiresAt > DateTime.UtcNow));

        // If entity context is provided, prioritize it
        if (!string.IsNullOrWhiteSpace(entityType) && !string.IsNullOrWhiteSpace(entityId))
        {
            // Fetch everything related to entity first
            var entityMemories = await query
                .Where(m => m.SourceModule == entityType && m.SourceEntityId == entityId)
                .ToListAsync(cancellationToken);
                
            // Also fetch other memories to see if any are globally relevant
            var otherMemoriesQuery = query.Where(m => m.SourceModule != entityType || m.SourceEntityId != entityId);
            
            // To prevent massive load, we only take a recent subset if it's large
            var otherMemories = await otherMemoriesQuery
                .OrderByDescending(m => m.CreatedAt)
                .Take(50)
                .ToListAsync(cancellationToken);

            var allCandidates = entityMemories.Concat(otherMemories);
            return _relevanceService.ScoreAndRank(allCandidates, question, limit);
        }

        // If no entity provided, fetch a broad subset and rank
        var candidates = await query
            .OrderByDescending(m => m.CreatedAt)
            .Take(100)
            .ToListAsync(cancellationToken);

        return _relevanceService.ScoreAndRank(candidates, question, limit);
    }

    public async Task<IEnumerable<Entities.BusinessMemory>> GetByEntityAsync(
        Guid organizationId, 
        string sourceModule, 
        string sourceEntityId, 
        CancellationToken cancellationToken = default)
    {
        return await _context.BusinessMemories
            .AsNoTracking()
            .Where(m => m.OrganizationId == organizationId 
                        && m.SourceModule == sourceModule 
                        && m.SourceEntityId == sourceEntityId
                        && m.IsActive
                        && (m.ExpiresAt == null || m.ExpiresAt > DateTime.UtcNow))
            .OrderByDescending(m => m.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}
