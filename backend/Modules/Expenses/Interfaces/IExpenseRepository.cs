using backend.Modules.Expenses.Entities;

namespace backend.Modules.Expenses.Interfaces;

public interface IExpenseRepository
{
    Task<Expense?> GetByIdAsync(Guid id, Guid organizationId);
    Task<IEnumerable<Expense>> GetAllAsync(Guid organizationId, Guid? categoryId = null, string? status = null, DateTime? startDate = null, DateTime? endDate = null);
    Task<Expense> AddAsync(Expense expense);
    Task UpdateAsync(Expense expense);
    Task DeleteAsync(Expense expense);
    Task<IEnumerable<Expense>> GetRecurringExpensesDueAsync(DateTime maxDate);
}
