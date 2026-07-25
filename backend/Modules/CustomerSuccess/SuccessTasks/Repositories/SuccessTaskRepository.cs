using backend.Modules.CustomerSuccess.SuccessTasks.Entities;
using backend.Modules.CustomerSuccess.SuccessTasks.Interfaces;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.CustomerSuccess.SuccessTasks.Repositories;

public class SuccessTaskRepository : ISuccessTaskRepository
{
    private readonly ApplicationDbContext _context;

    public SuccessTaskRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SuccessTask?> GetByIdAsync(Guid orgId, Guid id)
    {
        return await _context.SuccessTasks
            .Include(t => t.Customer)
            .Include(t => t.AssignedUser)
            .FirstOrDefaultAsync(t => t.OrganizationId == orgId && t.Id == id);
    }

    public async Task<(IEnumerable<SuccessTask> Items, int TotalCount)> GetPagedAsync(
        Guid orgId, Guid? customerId, Guid? assignedUserId, string? status, string? priority, string? taskType, int page, int pageSize)
    {
        var query = _context.SuccessTasks
            .Include(t => t.Customer)
            .Include(t => t.AssignedUser)
            .Where(t => t.OrganizationId == orgId)
            .AsNoTracking();

        if (customerId.HasValue)
        {
            query = query.Where(t => t.CustomerId == customerId.Value);
        }

        if (assignedUserId.HasValue)
        {
            query = query.Where(t => t.AssignedUserId == assignedUserId.Value);
        }

        if (!string.IsNullOrWhiteSpace(status))
        {
            query = query.Where(t => t.Status.ToLower() == status.ToLower());
        }

        if (!string.IsNullOrWhiteSpace(priority))
        {
            query = query.Where(t => t.Priority.ToLower() == priority.ToLower());
        }

        if (!string.IsNullOrWhiteSpace(taskType))
        {
            query = query.Where(t => t.TaskType.ToLower() == taskType.ToLower());
        }

        var totalCount = await query.CountAsync();
        var items = await query
            .OrderByDescending(t => t.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task AddAsync(SuccessTask task)
    {
        await _context.SuccessTasks.AddAsync(task);
    }

    public async Task UpdateAsync(SuccessTask task)
    {
        _context.SuccessTasks.Update(task);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(SuccessTask task)
    {
        _context.SuccessTasks.Remove(task);
        await Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
