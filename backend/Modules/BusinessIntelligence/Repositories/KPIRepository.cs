using backend.Modules.BusinessIntelligence.Entities;
using backend.Modules.BusinessIntelligence.Interfaces;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.BusinessIntelligence.Repositories;

public class KPIRepository : IKPIRepository
{
    private readonly ApplicationDbContext _context;

    public KPIRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<KPI?> GetByIdAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default)
    {
        return await _context.KPIs
            .FirstOrDefaultAsync(k => k.Id == id && k.OrganizationId == organizationId, cancellationToken);
    }

    public async Task<KPI?> GetByNameAsync(string name, Guid organizationId, CancellationToken cancellationToken = default)
    {
        return await _context.KPIs
            .FirstOrDefaultAsync(k => k.OrganizationId == organizationId && k.Name.ToLower() == name.ToLower(), cancellationToken);
    }

    public async Task<List<KPI>> GetAllByOrganizationIdAsync(Guid organizationId, string? category = null, CancellationToken cancellationToken = default)
    {
        var query = _context.KPIs.AsNoTracking().Where(k => k.OrganizationId == organizationId);
        
        if (!string.IsNullOrWhiteSpace(category))
        {
            query = query.Where(k => k.Category.ToLower() == category.ToLower());
        }

        return await query.OrderBy(k => k.Name).ToListAsync(cancellationToken);
    }

    public async Task<(List<KPI> Items, int TotalCount)> GetPagedAsync(
        Guid organizationId, 
        int page, 
        int pageSize, 
        string? search = null, 
        string? category = null, 
        string? sortBy = null, 
        bool sortDescending = false, 
        CancellationToken cancellationToken = default)
    {
        var query = _context.KPIs.AsNoTracking().Where(k => k.OrganizationId == organizationId);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var searchPattern = $"%{search.ToLower()}%";
            query = query.Where(k => EF.Functions.ILike(k.Name, searchPattern) || EF.Functions.ILike(k.Category, searchPattern));
        }

        if (!string.IsNullOrWhiteSpace(category))
        {
            query = query.Where(k => k.Category.ToLower() == category.ToLower());
        }

        var totalCount = await query.CountAsync(cancellationToken);

        query = sortBy?.ToLower() switch
        {
            "name" => sortDescending ? query.OrderByDescending(k => k.Name) : query.OrderBy(k => k.Name),
            "value" => sortDescending ? query.OrderByDescending(k => k.CurrentValue) : query.OrderBy(k => k.CurrentValue),
            "category" => sortDescending ? query.OrderByDescending(k => k.Category) : query.OrderBy(k => k.Category),
            _ => sortDescending ? query.OrderByDescending(k => k.LastCalculatedAt) : query.OrderBy(k => k.LastCalculatedAt)
        };

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task UpsertKPIAsync(KPI kpi, CancellationToken cancellationToken = default)
    {
        var existing = await _context.KPIs
            .FirstOrDefaultAsync(k => k.OrganizationId == kpi.OrganizationId && k.Name == kpi.Name, cancellationToken);

        if (existing == null)
        {
            await _context.KPIs.AddAsync(kpi, cancellationToken);
        }
        else
        {
            existing.CurrentValue = kpi.CurrentValue;
            existing.TargetValue = kpi.TargetValue ?? existing.TargetValue;
            existing.Unit = kpi.Unit;
            existing.Category = kpi.Category;
            existing.Trend = kpi.Trend;
            existing.LastCalculatedAt = DateTime.UtcNow;
            _context.KPIs.Update(existing);
        }
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
