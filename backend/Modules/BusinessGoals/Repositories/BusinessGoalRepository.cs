using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Modules.BusinessGoals.Entities;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.BusinessGoals.Repositories;

public class BusinessGoalRepository : IBusinessGoalRepository
{
    private readonly ApplicationDbContext _context;

    public BusinessGoalRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<BusinessGoal>> GetAllAsync(Guid organizationId)
    {
        return await _context.BusinessGoals
            .Include(b => b.KPIs)
            .Where(b => b.OrganizationId == organizationId && !b.IsDeleted)
            .OrderBy(b => b.TargetDate)
            .ToListAsync();
    }

    public async Task<BusinessGoal?> GetByIdAsync(Guid id, Guid organizationId)
    {
        return await _context.BusinessGoals
            .Include(b => b.KPIs)
            .FirstOrDefaultAsync(b => b.Id == id && b.OrganizationId == organizationId && !b.IsDeleted);
    }

    public async Task<BusinessGoal> AddAsync(BusinessGoal goal)
    {
        _context.BusinessGoals.Add(goal);
        await _context.SaveChangesAsync();
        return goal;
    }

    public async Task UpdateAsync(BusinessGoal goal)
    {
        _context.BusinessGoals.Update(goal);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(BusinessGoal goal)
    {
        goal.IsDeleted = true;
        _context.BusinessGoals.Update(goal);
        await _context.SaveChangesAsync();
    }
}
