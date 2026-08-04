using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Modules.CustomerFeedback.Entities;
using backend.Modules.CustomerFeedback.ImprovementActions.Interfaces;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.CustomerFeedback.ImprovementActions.Repositories;

public class ImprovementActionRepository : IImprovementActionRepository
{
    private readonly ApplicationDbContext _context;

    public ImprovementActionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ImprovementRecommendation>> GetAllAsync(Guid organizationId, string? status = null)
    {
        var query = _context.ImprovementRecommendations
            .AsNoTracking()
            .Where(r => r.OrganizationId == organizationId && !r.IsDeleted);

        if (!string.IsNullOrWhiteSpace(status) && Enum.TryParse<RecommendationStatus>(status, true, out var parsed))
        {
            query = query.Where(r => r.Status == parsed);
        }

        return await query.OrderByDescending(r => r.CreatedAt).ToListAsync();
    }

    public async Task<ImprovementRecommendation?> GetByIdAsync(Guid id, Guid organizationId)
    {
        return await _context.ImprovementRecommendations
            .FirstOrDefaultAsync(r => r.Id == id && r.OrganizationId == organizationId && !r.IsDeleted);
    }

    public async Task<ImprovementRecommendation> AddAsync(ImprovementRecommendation entity)
    {
        await _context.ImprovementRecommendations.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(ImprovementRecommendation entity)
    {
        _context.ImprovementRecommendations.Update(entity);
        await _context.SaveChangesAsync();
    }
}
