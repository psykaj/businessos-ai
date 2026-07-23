using backend.Modules.BusinessIntelligence.Entities;
using backend.Modules.BusinessIntelligence.Interfaces;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.BusinessIntelligence.Repositories;

public class GoalRepository : IGoalRepository
{
    private readonly ApplicationDbContext _context;

    public GoalRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Goal?> GetByIdAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default)
    {
        return await _context.Goals
            .FirstOrDefaultAsync(g => g.Id == id && g.OrganizationId == organizationId, cancellationToken);
    }

    public async Task<List<Goal>> GetAllByOrganizationIdAsync(Guid organizationId, string? status = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Goals.AsNoTracking().Where(g => g.OrganizationId == organizationId);

        if (!string.IsNullOrWhiteSpace(status))
        {
            query = query.Where(g => g.Status.ToLower() == status.ToLower());
        }

        return await query.OrderByDescending(g => g.CreatedAt).ToListAsync(cancellationToken);
    }

    public async Task<(List<Goal> Items, int TotalCount)> GetPagedAsync(
        Guid organizationId, 
        int page, 
        int pageSize, 
        string? search = null, 
        string? status = null, 
        string? sortBy = null, 
        bool sortDescending = false, 
        CancellationToken cancellationToken = default)
    {
        var query = _context.Goals.AsNoTracking().Where(g => g.OrganizationId == organizationId);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var searchPattern = $"%{search.ToLower()}%";
            query = query.Where(g => EF.Functions.ILike(g.Name, searchPattern));
        }

        if (!string.IsNullOrWhiteSpace(status))
        {
            query = query.Where(g => g.Status.ToLower() == status.ToLower());
        }

        var totalCount = await query.CountAsync(cancellationToken);

        query = sortBy?.ToLower() switch
        {
            "name" => sortDescending ? query.OrderByDescending(g => g.Name) : query.OrderBy(g => g.Name),
            "target" => sortDescending ? query.OrderByDescending(g => g.TargetValue) : query.OrderBy(g => g.TargetValue),
            "enddate" => sortDescending ? query.OrderByDescending(g => g.EndDate) : query.OrderBy(g => g.EndDate),
            _ => sortDescending ? query.OrderByDescending(g => g.CreatedAt) : query.OrderBy(g => g.CreatedAt)
        };

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task AddAsync(Goal goal, CancellationToken cancellationToken = default)
    {
        await _context.Goals.AddAsync(goal, cancellationToken);
    }

    public Task UpdateAsync(Goal goal, CancellationToken cancellationToken = default)
    {
        _context.Goals.Update(goal);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Goal goal, CancellationToken cancellationToken = default)
    {
        _context.Goals.Remove(goal);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
