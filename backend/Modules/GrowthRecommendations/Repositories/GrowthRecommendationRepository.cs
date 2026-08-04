using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Common;
using backend.Modules.GrowthRecommendations.Entities;
using backend.Modules.GrowthRecommendations.Interfaces;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.GrowthRecommendations.Repositories;

public class GrowthRecommendationRepository : IGrowthRecommendationRepository
{
    private readonly ApplicationDbContext _context;

    public GrowthRecommendationRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<GrowthRecommendation?> GetByIdAsync(Guid id, Guid organizationId)
    {
        return await _context.GrowthRecommendations
            .FirstOrDefaultAsync(r => r.Id == id && r.OrganizationId == organizationId && !r.IsDeleted);
    }

    public async Task<PagedResult<GrowthRecommendation>> GetRecommendationsAsync(Guid organizationId, string? status, string? priority, string? category, int pageNumber, int pageSize)
    {
        var query = _context.GrowthRecommendations
            .AsNoTracking()
            .Where(r => r.OrganizationId == organizationId && !r.IsDeleted);

        if (!string.IsNullOrWhiteSpace(status))
            query = query.Where(r => r.Status == status);
        else
            query = query.Where(r => r.Status != "Dismissed" && r.Status != "Actioned");

        if (!string.IsNullOrWhiteSpace(priority))
            query = query.Where(r => r.Priority == priority);

        if (!string.IsNullOrWhiteSpace(category))
            query = query.Where(r => r.Category == category);

        var total = await query.CountAsync();
        var items = await query
            .OrderByDescending(r => r.EstimatedFinancialImpact)
            .ThenByDescending(r => r.ConfidenceScore)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<GrowthRecommendation>(items, total, pageNumber, pageSize);
    }

    public async Task<List<GrowthRecommendation>> GetActiveRecommendationsAsync(Guid organizationId)
    {
        return await _context.GrowthRecommendations
            .AsNoTracking()
            .Where(r => r.OrganizationId == organizationId && !r.IsDeleted && (r.Status == "Open" || r.Status == "In-Progress" || r.Status == "Pending"))
            .OrderByDescending(r => r.EstimatedFinancialImpact)
            .ToListAsync();
    }

    public async Task<GrowthRecommendation> CreateAsync(GrowthRecommendation recommendation)
    {
        _context.GrowthRecommendations.Add(recommendation);
        await _context.SaveChangesAsync();
        return recommendation;
    }

    public async Task<GrowthRecommendation> UpdateAsync(GrowthRecommendation recommendation)
    {
        _context.GrowthRecommendations.Update(recommendation);
        await _context.SaveChangesAsync();
        return recommendation;
    }

    public async Task<bool> DeleteAsync(Guid id, Guid organizationId)
    {
        var item = await _context.GrowthRecommendations
            .FirstOrDefaultAsync(r => r.Id == id && r.OrganizationId == organizationId && !r.IsDeleted);
        if (item == null) return false;

        item.IsDeleted = true;
        item.DeletedAt = DateTime.UtcNow;
        _context.GrowthRecommendations.Update(item);
        await _context.SaveChangesAsync();
        return true;
    }
}
