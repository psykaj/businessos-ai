using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Modules.ExecutiveInsights.Entities;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.ExecutiveInsights.Repositories;

public class ExecutiveInsightRepository : IExecutiveInsightRepository
{
    private readonly ApplicationDbContext _context;

    public ExecutiveInsightRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ExecutiveInsight>> GetAllAsync(Guid organizationId)
    {
        return await _context.ExecutiveInsights
            .Where(e => e.OrganizationId == organizationId && !e.IsDeleted)
            .OrderByDescending(e => e.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<ExecutiveInsight>> GetUnreadAsync(Guid organizationId)
    {
        return await _context.ExecutiveInsights
            .Where(e => e.OrganizationId == organizationId && !e.IsRead && !e.IsDeleted)
            .OrderByDescending(e => e.Priority == "High" ? 3 : e.Priority == "Medium" ? 2 : 1)
            .ThenByDescending(e => e.CreatedAt)
            .ToListAsync();
    }

    public async Task<ExecutiveInsight?> GetByIdAsync(Guid id, Guid organizationId)
    {
        return await _context.ExecutiveInsights
            .FirstOrDefaultAsync(e => e.Id == id && e.OrganizationId == organizationId && !e.IsDeleted);
    }

    public async Task<ExecutiveInsight> AddAsync(ExecutiveInsight insight)
    {
        _context.ExecutiveInsights.Add(insight);
        await _context.SaveChangesAsync();
        return insight;
    }

    public async Task UpdateAsync(ExecutiveInsight insight)
    {
        _context.ExecutiveInsights.Update(insight);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(ExecutiveInsight insight)
    {
        insight.IsDeleted = true;
        _context.ExecutiveInsights.Update(insight);
        await _context.SaveChangesAsync();
    }
}
