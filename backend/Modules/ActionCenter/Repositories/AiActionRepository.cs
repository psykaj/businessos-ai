using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Modules.ActionCenter.Entities;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.ActionCenter.Repositories;

public class AiActionRepository : IAiActionRepository
{
    private readonly ApplicationDbContext _context;

    public AiActionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<AiAction?> GetByIdAsync(Guid id, Guid organizationId)
    {
        return await _context.AiActions
            .FirstOrDefaultAsync(a => a.Id == id && a.OrganizationId == organizationId);
    }

    public async Task<IEnumerable<AiAction>> GetAllAsync(Guid organizationId)
    {
        return await _context.AiActions
            .Where(a => a.OrganizationId == organizationId)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<AiAction>> GetPendingAsync(Guid organizationId)
    {
        return await _context.AiActions
            .Where(a => a.OrganizationId == organizationId && (a.Status == AiActionStatus.Pending || a.Status == AiActionStatus.Approved))
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<AiAction>> GetHistoryAsync(Guid organizationId)
    {
        return await _context.AiActions
            .Where(a => a.OrganizationId == organizationId && (a.Status == AiActionStatus.Executed || a.Status == AiActionStatus.Failed || a.Status == AiActionStatus.Rejected))
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();
    }

    public async Task<AiAction> AddAsync(AiAction action)
    {
        _context.AiActions.Add(action);
        await _context.SaveChangesAsync();
        return action;
    }

    public async Task UpdateAsync(AiAction action)
    {
        _context.AiActions.Update(action);
        await _context.SaveChangesAsync();
    }
}
