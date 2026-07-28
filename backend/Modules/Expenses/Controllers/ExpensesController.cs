using backend.Modules.Expenses.DTOs;
using backend.Modules.Expenses.Interfaces;
using backend.Modules.Finance.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.Expenses.Controllers;

[Route("api/expenses")]
public class ExpensesController : BaseFinanceController
{
    private readonly IExpenseService _expenseService;

    public ExpensesController(IExpenseService expenseService)
    {
        _expenseService = expenseService;
    }

    [HttpGet]
    public async Task<IActionResult> GetExpenses(
        [FromQuery] Guid? categoryId,
        [FromQuery] string? status,
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate)
    {
        var orgId = GetOrganizationId();
        var expenses = await _expenseService.GetExpensesAsync(orgId, categoryId, status, startDate, endDate);
        return Ok(expenses);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetExpenseById(Guid id)
    {
        var orgId = GetOrganizationId();
        var expense = await _expenseService.GetExpenseByIdAsync(id, orgId);
        if (expense == null) return NotFound(new { message = "Expense not found" });
        return Ok(expense);
    }

    [HttpPost]
    public async Task<IActionResult> CreateExpense([FromBody] CreateExpenseDto dto)
    {
        var orgId = GetOrganizationId();
        try
        {
            var created = await _expenseService.CreateExpenseAsync(orgId, dto);
            return CreatedAtAction(nameof(GetExpenseById), new { id = created.Id }, created);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateExpense(Guid id, [FromBody] UpdateExpenseDto dto)
    {
        var orgId = GetOrganizationId();
        try
        {
            var updated = await _expenseService.UpdateExpenseAsync(id, orgId, dto);
            if (updated == null) return NotFound(new { message = "Expense not found" });
            return Ok(updated);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteExpense(Guid id)
    {
        var orgId = GetOrganizationId();
        var success = await _expenseService.DeleteExpenseAsync(id, orgId);
        if (!success) return NotFound(new { message = "Expense not found" });
        return NoContent();
    }

    [HttpPost("{id:guid}/approve")]
    public async Task<IActionResult> ApproveExpense(Guid id)
    {
        var orgId = GetOrganizationId();
        var userId = GetCurrentUserId();
        var approved = await _expenseService.ApproveExpenseAsync(id, orgId, userId);
        if (approved == null) return NotFound(new { message = "Expense not found" });
        return Ok(approved);
    }

    [HttpPost("{id:guid}/reject")]
    public async Task<IActionResult> RejectExpense(Guid id)
    {
        var orgId = GetOrganizationId();
        var rejected = await _expenseService.RejectExpenseAsync(id, orgId);
        if (rejected == null) return NotFound(new { message = "Expense not found" });
        return Ok(rejected);
    }

    [HttpPost("upload-receipt")]
    public async Task<IActionResult> UploadReceipt([FromForm] IFormFile file)
    {
        var orgId = GetOrganizationId();
        if (file == null || file.Length == 0) return BadRequest(new { message = "No file uploaded" });

        var url = await _expenseService.UploadReceiptAsync(orgId, file);
        return Ok(new { url });
    }

    [HttpGet("categories")]
    public async Task<IActionResult> GetCategories()
    {
        var orgId = GetOrganizationId();
        var categories = await _expenseService.GetCategoriesAsync(orgId);
        return Ok(categories);
    }

    [HttpPost("categories")]
    public async Task<IActionResult> CreateCategory([FromBody] CreateExpenseCategoryDto dto)
    {
        var orgId = GetOrganizationId();
        var created = await _expenseService.CreateCategoryAsync(orgId, dto);
        return Ok(created);
    }
}
