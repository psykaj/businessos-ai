using backend.Modules.Expenses.Entities;

namespace backend.Modules.Expenses.Interfaces;

public interface IExpenseCategoryRepository
{
    Task<ExpenseCategory?> GetByIdAsync(Guid id, Guid organizationId);
    Task<IEnumerable<ExpenseCategory>> GetAllAsync(Guid organizationId);
    Task<ExpenseCategory> AddAsync(ExpenseCategory category);
    Task UpdateAsync(ExpenseCategory category);
    Task DeleteAsync(ExpenseCategory category);
}
