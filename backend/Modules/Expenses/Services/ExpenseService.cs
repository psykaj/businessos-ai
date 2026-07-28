using backend.Modules.Expenses.DTOs;
using backend.Modules.Expenses.Entities;
using backend.Modules.Expenses.Interfaces;
using Microsoft.AspNetCore.Http;

namespace backend.Modules.Expenses.Services;

public class ExpenseService : IExpenseService
{
    private readonly IExpenseRepository _expenseRepository;
    private readonly IExpenseCategoryRepository _categoryRepository;
    private readonly IReceiptStorageService _receiptStorageService;

    public ExpenseService(
        IExpenseRepository expenseRepository,
        IExpenseCategoryRepository categoryRepository,
        IReceiptStorageService receiptStorageService)
    {
        _expenseRepository = expenseRepository;
        _categoryRepository = categoryRepository;
        _receiptStorageService = receiptStorageService;
    }

    public async Task<ExpenseDto> CreateExpenseAsync(Guid organizationId, CreateExpenseDto dto)
    {
        var category = await _categoryRepository.GetByIdAsync(dto.ExpenseCategoryId, organizationId);
        if (category == null)
        {
            throw new ArgumentException("Expense category not found.");
        }

        var expense = new Expense
        {
            OrganizationId = organizationId,
            Title = dto.Title,
            Description = dto.Description,
            Amount = dto.Amount,
            TaxAmount = dto.TaxAmount,
            TaxRate = dto.TaxRate,
            Currency = dto.Currency,
            ExpenseCategoryId = dto.ExpenseCategoryId,
            ExpenseDate = dto.ExpenseDate,
            VendorName = dto.VendorName,
            PaymentMethod = dto.PaymentMethod,
            Status = "Paid",
            ReceiptUrl = dto.ReceiptUrl,
            IsRecurring = dto.IsRecurring,
            RecurringInterval = dto.RecurringInterval,
            NextRecurringDate = dto.IsRecurring ? CalculateNextRecurringDate(dto.ExpenseDate, dto.RecurringInterval) : null
        };

        var created = await _expenseRepository.AddAsync(expense);
        created.ExpenseCategory = category;
        return MapToDto(created);
    }

    public async Task<ExpenseDto?> UpdateExpenseAsync(Guid id, Guid organizationId, UpdateExpenseDto dto)
    {
        var expense = await _expenseRepository.GetByIdAsync(id, organizationId);
        if (expense == null) return null;

        var category = await _categoryRepository.GetByIdAsync(dto.ExpenseCategoryId, organizationId);
        if (category == null) throw new ArgumentException("Expense category not found.");

        expense.Title = dto.Title;
        expense.Description = dto.Description;
        expense.Amount = dto.Amount;
        expense.TaxAmount = dto.TaxAmount;
        expense.TaxRate = dto.TaxRate;
        expense.Currency = dto.Currency;
        expense.ExpenseCategoryId = dto.ExpenseCategoryId;
        expense.ExpenseDate = dto.ExpenseDate;
        expense.VendorName = dto.VendorName;
        expense.PaymentMethod = dto.PaymentMethod;
        expense.Status = dto.Status;
        expense.ReceiptUrl = dto.ReceiptUrl;
        expense.IsRecurring = dto.IsRecurring;
        expense.RecurringInterval = dto.RecurringInterval;
        expense.NextRecurringDate = dto.IsRecurring ? CalculateNextRecurringDate(dto.ExpenseDate, dto.RecurringInterval) : null;

        await _expenseRepository.UpdateAsync(expense);
        expense.ExpenseCategory = category;
        return MapToDto(expense);
    }

    public async Task<bool> DeleteExpenseAsync(Guid id, Guid organizationId)
    {
        var expense = await _expenseRepository.GetByIdAsync(id, organizationId);
        if (expense == null) return false;

        await _expenseRepository.DeleteAsync(expense);
        return true;
    }

    public async Task<ExpenseDto?> GetExpenseByIdAsync(Guid id, Guid organizationId)
    {
        var expense = await _expenseRepository.GetByIdAsync(id, organizationId);
        return expense != null ? MapToDto(expense) : null;
    }

    public async Task<IEnumerable<ExpenseDto>> GetExpensesAsync(Guid organizationId, Guid? categoryId = null, string? status = null, DateTime? startDate = null, DateTime? endDate = null)
    {
        var expenses = await _expenseRepository.GetAllAsync(organizationId, categoryId, status, startDate, endDate);
        return expenses.Select(MapToDto);
    }

    public async Task<ExpenseDto?> ApproveExpenseAsync(Guid id, Guid organizationId, string approvedBy)
    {
        var expense = await _expenseRepository.GetByIdAsync(id, organizationId);
        if (expense == null) return null;

        expense.Status = "Approved";
        expense.ApprovedBy = approvedBy;
        expense.ApprovedAt = DateTime.UtcNow;

        await _expenseRepository.UpdateAsync(expense);
        return MapToDto(expense);
    }

    public async Task<ExpenseDto?> RejectExpenseAsync(Guid id, Guid organizationId)
    {
        var expense = await _expenseRepository.GetByIdAsync(id, organizationId);
        if (expense == null) return null;

        expense.Status = "Rejected";

        await _expenseRepository.UpdateAsync(expense);
        return MapToDto(expense);
    }

    public async Task<string> UploadReceiptAsync(Guid organizationId, IFormFile file)
    {
        return await _receiptStorageService.UploadReceiptAsync(file, organizationId);
    }

    public async Task<IEnumerable<ExpenseCategoryDto>> GetCategoriesAsync(Guid organizationId)
    {
        var categories = await _categoryRepository.GetAllAsync(organizationId);
        if (!categories.Any())
        {
            await SeedDefaultCategoriesAsync(organizationId);
            categories = await _categoryRepository.GetAllAsync(organizationId);
        }
        return categories.Select(c => new ExpenseCategoryDto(c.Id, c.OrganizationId, c.Name, c.Code, c.Description, c.Color, c.IsActive));
    }

    public async Task<ExpenseCategoryDto> CreateCategoryAsync(Guid organizationId, CreateExpenseCategoryDto dto)
    {
        var cat = new ExpenseCategory
        {
            OrganizationId = organizationId,
            Name = dto.Name,
            Code = dto.Code,
            Description = dto.Description,
            Color = dto.Color,
            IsActive = true
        };

        var created = await _categoryRepository.AddAsync(cat);
        return new ExpenseCategoryDto(created.Id, created.OrganizationId, created.Name, created.Code, created.Description, created.Color, created.IsActive);
    }

    private async Task SeedDefaultCategoriesAsync(Guid organizationId)
    {
        var defaultCats = new List<ExpenseCategory>
        {
            new ExpenseCategory { OrganizationId = organizationId, Name = "Office Supplies", Code = "OFFICE", Color = "#3B82F6" },
            new ExpenseCategory { OrganizationId = organizationId, Name = "Travel & Transportation", Code = "TRAVEL", Color = "#10B981" },
            new ExpenseCategory { OrganizationId = organizationId, Name = "Software & IT", Code = "SOFTWARE", Color = "#8B5CF6" },
            new ExpenseCategory { OrganizationId = organizationId, Name = "Rent & Facilities", Code = "RENT", Color = "#F59E0B" },
            new ExpenseCategory { OrganizationId = organizationId, Name = "Marketing & Ads", Code = "MARKETING", Color = "#EF4444" },
            new ExpenseCategory { OrganizationId = organizationId, Name = "Meals & Entertainment", Code = "MEALS", Color = "#EC4899" }
        };

        foreach (var c in defaultCats)
        {
            await _categoryRepository.AddAsync(c);
        }
    }

    private static DateTime? CalculateNextRecurringDate(DateTime baseDate, string? interval)
    {
        return interval switch
        {
            "Monthly" => baseDate.AddMonths(1),
            "Quarterly" => baseDate.AddMonths(3),
            "Yearly" => baseDate.AddYears(1),
            _ => null
        };
    }

    private static ExpenseDto MapToDto(Expense e) => new(
        e.Id,
        e.OrganizationId,
        e.Title,
        e.Description,
        e.Amount,
        e.TaxAmount,
        e.TaxRate,
        e.Currency,
        e.ExpenseCategoryId,
        e.ExpenseCategory?.Name ?? "Uncategorized",
        e.ExpenseDate,
        e.VendorName,
        e.PaymentMethod,
        e.Status,
        e.ReceiptUrl,
        e.IsRecurring,
        e.RecurringInterval,
        e.NextRecurringDate,
        e.ApprovedBy,
        e.ApprovedAt,
        e.CreatedAt
    );
}
