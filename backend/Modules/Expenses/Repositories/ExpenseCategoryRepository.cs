using backend.Modules.Expenses.Entities;
using backend.Modules.Expenses.Interfaces;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.Expenses.Repositories;

public class ExpenseCategoryRepository : IExpenseCategoryRepository
{
    private readonly ApplicationDbContext _context;

    public ExpenseCategoryRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ExpenseCategory?> GetByIdAsync(Guid id, Guid organizationId)
    {
        return await _context.ExpenseCategories
            .FirstOrDefaultAsync(c => c.Id == id && c.OrganizationId == organizationId && !c.IsDeleted);
    }

    public async Task<IEnumerable<ExpenseCategory>> GetAllAsync(Guid organizationId)
    {
        return await _context.ExpenseCategories
            .AsNoTracking()
            .Where(c => c.OrganizationId == organizationId && !c.IsDeleted)
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    public async Task<ExpenseCategory> AddAsync(ExpenseCategory category)
    {
        await _context.ExpenseCategories.AddAsync(category);
        await _context.SaveChangesAsync();
        return category;
    }

    public async Task UpdateAsync(ExpenseCategory category)
    {
        _context.ExpenseCategories.Update(category);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(ExpenseCategory category)
    {
        category.IsDeleted = true;
        category.DeletedAt = DateTime.UtcNow;
        _context.ExpenseCategories.Update(category);
        await _context.SaveChangesAsync();
    }
}
