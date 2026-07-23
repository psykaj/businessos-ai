using backend.Modules.BusinessIntelligence.Entities;
using backend.Modules.BusinessIntelligence.Interfaces;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.BusinessIntelligence.Repositories;

public class InsightRepository : IInsightRepository
{
    private readonly ApplicationDbContext _context;

    public InsightRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Insight?> GetByIdAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default)
    {
        return await _context.Insights
            .FirstOrDefaultAsync(i => i.Id == id && i.OrganizationId == organizationId, cancellationToken);
    }

    public async Task<List<Insight>> GetAllByOrganizationIdAsync(Guid organizationId, string? category = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Insights.AsNoTracking().Where(i => i.OrganizationId == organizationId);

        if (!string.IsNullOrWhiteSpace(category))
        {
            query = query.Where(i => i.Category.ToLower() == category.ToLower());
        }

        return await query.OrderByDescending(i => i.CreatedAt).ToListAsync(cancellationToken);
    }

    public async Task<(List<Insight> Items, int TotalCount)> GetPagedAsync(
        Guid organizationId, 
        int page, 
        int pageSize, 
        string? search = null, 
        string? priority = null, 
        string? category = null, 
        string? sortBy = null, 
        bool sortDescending = false, 
        CancellationToken cancellationToken = default)
    {
        var query = _context.Insights.AsNoTracking().Where(i => i.OrganizationId == organizationId);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var searchPattern = $"%{search.ToLower()}%";
            query = query.Where(i => EF.Functions.ILike(i.Title, searchPattern) || EF.Functions.ILike(i.Description, searchPattern));
        }

        if (!string.IsNullOrWhiteSpace(priority))
        {
            query = query.Where(i => i.Priority.ToLower() == priority.ToLower());
        }

        if (!string.IsNullOrWhiteSpace(category))
        {
            query = query.Where(i => i.Category.ToLower() == category.ToLower());
        }

        var totalCount = await query.CountAsync(cancellationToken);

        query = sortBy?.ToLower() switch
        {
            "title" => sortDescending ? query.OrderByDescending(i => i.Title) : query.OrderBy(i => i.Title),
            "priority" => sortDescending ? query.OrderByDescending(i => i.Priority) : query.OrderBy(i => i.Priority),
            _ => sortDescending ? query.OrderByDescending(i => i.CreatedAt) : query.OrderBy(i => i.CreatedAt)
        };

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task AddRangeAsync(IEnumerable<Insight> insights, CancellationToken cancellationToken = default)
    {
        await _context.Insights.AddRangeAsync(insights, cancellationToken);
    }

    public async Task ClearAndAddAsync(Guid organizationId, IEnumerable<Insight> newInsights, CancellationToken cancellationToken = default)
    {
        var existing = await _context.Insights.Where(i => i.OrganizationId == organizationId).ToListAsync(cancellationToken);
        _context.Insights.RemoveRange(existing);
        await _context.Insights.AddRangeAsync(newInsights, cancellationToken);
    }

    public Task DeleteAsync(Insight insight, CancellationToken cancellationToken = default)
    {
        _context.Insights.Remove(insight);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
