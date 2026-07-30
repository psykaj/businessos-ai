using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Modules.BusinessHealth.Entities;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.BusinessHealth.Repositories;

public class BusinessHealthRepository : IBusinessHealthRepository
{
    private readonly ApplicationDbContext _context;

    public BusinessHealthRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<BusinessHealthScore>> GetHistoryAsync(Guid organizationId, int limit)
    {
        return await _context.BusinessHealthScores
            .Where(b => b.OrganizationId == organizationId && !b.IsDeleted)
            .OrderByDescending(b => b.CalculatedAt)
            .Take(limit)
            .ToListAsync();
    }

    public async Task<BusinessHealthScore?> GetLatestAsync(Guid organizationId)
    {
        return await _context.BusinessHealthScores
            .Where(b => b.OrganizationId == organizationId && !b.IsDeleted)
            .OrderByDescending(b => b.CalculatedAt)
            .FirstOrDefaultAsync();
    }

    public async Task<BusinessHealthScore> AddAsync(BusinessHealthScore score)
    {
        _context.BusinessHealthScores.Add(score);
        await _context.SaveChangesAsync();
        return score;
    }
}
