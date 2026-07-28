using backend.Modules.Expenses.Entities;
using backend.Modules.Expenses.Interfaces;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.Expenses.Repositories;

public class ExpenseRepository : IExpenseRepository
{
    private readonly ApplicationDbContext _context;

    public ExpenseRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Expense?> GetByIdAsync(Guid id, Guid organizationId)
    {
        return await _context.Expenses
            .Include(e => e.ExpenseCategory)
            .FirstOrDefaultAsync(e => e.Id == id && e.OrganizationId == organizationId && !e.IsDeleted);
    }

    public async Task<IEnumerable<Expense>> GetAllAsync(Guid organizationId, Guid? categoryId = null, string? status = null, DateTime? startDate = null, DateTime? endDate = null)
    {
        var query = _context.Expenses
            .Include(e => e.ExpenseCategory)
            .AsNoTracking()
            .Where(e => e.OrganizationId == organizationId && !e.IsDeleted);

        if (categoryId.HasValue)
        {
            query = query.Where(e => e.ExpenseCategoryId == categoryId.Value);
        }

        if (!string.IsNullOrWhiteSpace(status))
        {
            query = query.Where(e => e.Status == status);
        }

        if (startDate.HasValue)
        {
            query = query.Where(e => e.ExpenseDate >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            query = query.Where(e => e.ExpenseDate <= endDate.Value);
        }

        return await query.OrderByDescending(e => e.ExpenseDate).ToListAsync();
    }

    public async Task<Expense> AddAsync(Expense expense)
    {
        await _context.Expenses.AddAsync(expense);
        await _context.SaveChangesAsync();
        return expense;
    }

    public async Task UpdateAsync(Expense expense)
    {
        _context.Expenses.Update(expense);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Expense expense)
    {
        expense.IsDeleted = true;
        expense.DeletedAt = DateTime.UtcNow;
        _context.Expenses.Update(expense);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Expense>> GetRecurringExpensesDueAsync(DateTime maxDate)
    {
        return await _context.Expenses
            .Where(e => e.IsRecurring && e.NextRecurringDate <= maxDate && !e.IsDeleted)
            .ToListAsync();
    }
}
