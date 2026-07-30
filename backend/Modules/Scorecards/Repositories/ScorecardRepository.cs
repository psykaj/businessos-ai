using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Modules.Scorecards.Entities;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.Scorecards.Repositories;

public class ScorecardRepository : IScorecardRepository
{
    private readonly ApplicationDbContext _context;

    public ScorecardRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Scorecard>> GetAllAsync(Guid organizationId)
    {
        return await _context.Scorecards
            .Where(s => s.OrganizationId == organizationId && !s.IsDeleted)
            .OrderByDescending(s => s.PeriodEnd)
            .ToListAsync();
    }

    public async Task<Scorecard?> GetByIdAsync(Guid id, Guid organizationId)
    {
        return await _context.Scorecards
            .FirstOrDefaultAsync(s => s.Id == id && s.OrganizationId == organizationId && !s.IsDeleted);
    }

    public async Task<Scorecard> AddAsync(Scorecard scorecard)
    {
        _context.Scorecards.Add(scorecard);
        await _context.SaveChangesAsync();
        return scorecard;
    }

    public async Task UpdateAsync(Scorecard scorecard)
    {
        _context.Scorecards.Update(scorecard);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Scorecard scorecard)
    {
        scorecard.IsDeleted = true;
        _context.Scorecards.Update(scorecard);
        await _context.SaveChangesAsync();
    }
}
