using backend.Modules.BusinessIntelligence.Entities;
using backend.Modules.BusinessIntelligence.Interfaces;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.BusinessIntelligence.Repositories;

public class ReportRepository : IReportRepository
{
    private readonly ApplicationDbContext _context;

    public ReportRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Report?> GetByIdAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default)
    {
        return await _context.Reports
            .FirstOrDefaultAsync(r => r.Id == id && r.OrganizationId == organizationId, cancellationToken);
    }

    public async Task<List<Report>> GetAllByOrganizationIdAsync(Guid organizationId, string? reportType = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Reports.AsNoTracking().Where(r => r.OrganizationId == organizationId);

        if (!string.IsNullOrWhiteSpace(reportType))
        {
            query = query.Where(r => r.ReportType.ToLower() == reportType.ToLower());
        }

        return await query.OrderByDescending(r => r.GeneratedAt).ToListAsync(cancellationToken);
    }

    public async Task<(List<Report> Items, int TotalCount)> GetPagedAsync(
        Guid organizationId, 
        int page, 
        int pageSize, 
        string? search = null, 
        string? reportType = null, 
        string? sortBy = null, 
        bool sortDescending = false, 
        CancellationToken cancellationToken = default)
    {
        var query = _context.Reports.AsNoTracking().Where(r => r.OrganizationId == organizationId);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var searchPattern = $"%{search.ToLower()}%";
            query = query.Where(r => EF.Functions.ILike(r.Name, searchPattern) || EF.Functions.ILike(r.ReportType, searchPattern));
        }

        if (!string.IsNullOrWhiteSpace(reportType))
        {
            query = query.Where(r => r.ReportType.ToLower() == reportType.ToLower());
        }

        var totalCount = await query.CountAsync(cancellationToken);

        query = sortBy?.ToLower() switch
        {
            "name" => sortDescending ? query.OrderByDescending(r => r.Name) : query.OrderBy(r => r.Name),
            "type" => sortDescending ? query.OrderByDescending(r => r.ReportType) : query.OrderBy(r => r.ReportType),
            _ => sortDescending ? query.OrderByDescending(r => r.GeneratedAt) : query.OrderBy(r => r.GeneratedAt)
        };

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task AddAsync(Report report, CancellationToken cancellationToken = default)
    {
        await _context.Reports.AddAsync(report, cancellationToken);
    }

    public Task DeleteAsync(Report report, CancellationToken cancellationToken = default)
    {
        _context.Reports.Remove(report);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
