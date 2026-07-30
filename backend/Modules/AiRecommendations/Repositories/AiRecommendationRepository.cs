using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Modules.AiRecommendations.Entities;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.AiRecommendations.Repositories;

public class AiRecommendationRepository : IAiRecommendationRepository
{
    private readonly ApplicationDbContext _context;

    public AiRecommendationRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<AiRecommendation>> GetAllAsync(Guid organizationId)
    {
        return await _context.AiRecommendations
            .Where(r => r.OrganizationId == organizationId && !r.IsDeleted)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<AiRecommendation>> GetPendingAsync(Guid organizationId)
    {
        return await _context.AiRecommendations
            .Where(r => r.OrganizationId == organizationId && !r.IsApplied && !r.IsDeleted)
            .OrderByDescending(r => r.Priority == "High" ? 3 : r.Priority == "Medium" ? 2 : 1)
            .ThenByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task<AiRecommendation?> GetByIdAsync(Guid id, Guid organizationId)
    {
        return await _context.AiRecommendations
            .FirstOrDefaultAsync(r => r.Id == id && r.OrganizationId == organizationId && !r.IsDeleted);
    }

    public async Task<AiRecommendation> AddAsync(AiRecommendation recommendation)
    {
        _context.AiRecommendations.Add(recommendation);
        await _context.SaveChangesAsync();
        return recommendation;
    }

    public async Task UpdateAsync(AiRecommendation recommendation)
    {
        _context.AiRecommendations.Update(recommendation);
        await _context.SaveChangesAsync();
    }
}
