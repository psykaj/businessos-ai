using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Modules.DecisionCenter.Entities;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.DecisionCenter.Repositories;

public class DecisionLogRepository : IDecisionLogRepository
{
    private readonly ApplicationDbContext _context;

    public DecisionLogRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<DecisionLog>> GetAllAsync(Guid organizationId)
    {
        return await _context.DecisionLogs
            .Where(d => d.OrganizationId == organizationId && !d.IsDeleted)
            .OrderByDescending(d => d.DecisionDate)
            .ToListAsync();
    }

    public async Task<DecisionLog?> GetByIdAsync(Guid id, Guid organizationId)
    {
        return await _context.DecisionLogs
            .FirstOrDefaultAsync(d => d.Id == id && d.OrganizationId == organizationId && !d.IsDeleted);
    }

    public async Task<DecisionLog> AddAsync(DecisionLog log)
    {
        _context.DecisionLogs.Add(log);
        await _context.SaveChangesAsync();
        return log;
    }

    public async Task UpdateAsync(DecisionLog log)
    {
        _context.DecisionLogs.Update(log);
        await _context.SaveChangesAsync();
    }
}
