using backend.Modules.AiAgent.Entities;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.AiAgent.Repositories;

public class RecommendationRepository : IRecommendationRepository
{
    private readonly ApplicationDbContext _context;

    public RecommendationRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Recommendation?> GetByIdAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default)
    {
        return await _context.Recommendations
            .FirstOrDefaultAsync(r => r.Id == id && r.OrganizationId == organizationId && !r.IsDeleted, cancellationToken);
    }

    public async Task<List<Recommendation>> GetAllByOrganizationIdAsync(Guid organizationId, bool includeApplied = false, CancellationToken cancellationToken = default)
    {
        var query = _context.Recommendations.AsNoTracking()
            .Where(r => r.OrganizationId == organizationId && !r.IsDeleted);

        if (!includeApplied)
        {
            query = query.Where(r => !r.IsApplied);
        }

        return await query.OrderByDescending(r => r.CreatedAt).ToListAsync(cancellationToken);
    }

    public async Task<(List<Recommendation> Items, int TotalCount)> GetPagedAsync(
        Guid organizationId,
        int page = 1,
        int pageSize = 20,
        string? category = null,
        string? priority = null,
        bool? isApplied = null,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Recommendations.AsNoTracking()
            .Where(r => r.OrganizationId == organizationId && !r.IsDeleted);

        if (!string.IsNullOrWhiteSpace(category))
        {
            query = query.Where(r => r.Category.ToLower() == category.ToLower());
        }

        if (!string.IsNullOrWhiteSpace(priority))
        {
            query = query.Where(r => r.Priority.ToLower() == priority.ToLower());
        }

        if (isApplied.HasValue)
        {
            query = query.Where(r => r.IsApplied == isApplied.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(r => r.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task AddAsync(Recommendation recommendation, CancellationToken cancellationToken = default)
    {
        await _context.Recommendations.AddAsync(recommendation, cancellationToken);
    }

    public async Task AddRangeAsync(IEnumerable<Recommendation> recommendations, CancellationToken cancellationToken = default)
    {
        await _context.Recommendations.AddRangeAsync(recommendations, cancellationToken);
    }

    public void Update(Recommendation recommendation)
    {
        _context.Recommendations.Update(recommendation);
    }

    public void Delete(Recommendation recommendation)
    {
        recommendation.IsDeleted = true;
        _context.Recommendations.Update(recommendation);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
