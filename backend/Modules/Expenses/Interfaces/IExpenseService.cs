using backend.Modules.Expenses.DTOs;
using Microsoft.AspNetCore.Http;

namespace backend.Modules.Expenses.Interfaces;

public interface IExpenseService
{
    Task<ExpenseDto> CreateExpenseAsync(Guid organizationId, CreateExpenseDto dto);
    Task<ExpenseDto?> UpdateExpenseAsync(Guid id, Guid organizationId, UpdateExpenseDto dto);
    Task<bool> DeleteExpenseAsync(Guid id, Guid organizationId);
    Task<ExpenseDto?> GetExpenseByIdAsync(Guid id, Guid organizationId);
    Task<IEnumerable<ExpenseDto>> GetExpensesAsync(Guid organizationId, Guid? categoryId = null, string? status = null, DateTime? startDate = null, DateTime? endDate = null);
    
    Task<ExpenseDto?> ApproveExpenseAsync(Guid id, Guid organizationId, string approvedBy);
    Task<ExpenseDto?> RejectExpenseAsync(Guid id, Guid organizationId);
    
    Task<string> UploadReceiptAsync(Guid organizationId, IFormFile file);
    
    Task<IEnumerable<ExpenseCategoryDto>> GetCategoriesAsync(Guid organizationId);
    Task<ExpenseCategoryDto> CreateCategoryAsync(Guid organizationId, CreateExpenseCategoryDto dto);
}
