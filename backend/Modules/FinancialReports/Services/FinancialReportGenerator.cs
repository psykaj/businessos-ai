using backend.Modules.FinancialReports.DTOs;
using backend.Modules.FinancialReports.Interfaces;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.FinancialReports.Services;

public class FinancialReportGenerator : IFinancialReportGenerator
{
    private readonly ApplicationDbContext _context;

    public FinancialReportGenerator(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ProfitAndLossReportDto> GenerateProfitAndLossAsync(Guid organizationId, DateTime start, DateTime end)
    {
        // 1. Calculate Operating Revenue (Invoices)
        var invoices = await _context.FinanceInvoices
            .AsNoTracking()
            .Where(i => i.OrganizationId == organizationId && i.IssueDate >= start && i.IssueDate <= end && !i.IsDeleted)
            .ToListAsync();

        var operatingRevenue = invoices.Sum(i => i.SubTotal);

        // 2. Expenses (COGS vs Operating Expenses)
        var expenses = await _context.Expenses
            .Include(e => e.ExpenseCategory)
            .AsNoTracking()
            .Where(e => e.OrganizationId == organizationId && e.ExpenseDate >= start && e.ExpenseDate <= end && !e.IsDeleted)
            .ToListAsync();

        var totalExpenses = expenses.Sum(e => e.Amount);

        var cogs = expenses
            .Where(e => e.ExpenseCategory?.Name.Contains("Cost of Goods", StringComparison.OrdinalIgnoreCase) == true)
            .Sum(e => e.Amount);

        var operatingExpenses = totalExpenses - cogs;
        var grossProfit = operatingRevenue - cogs;
        var netIncome = grossProfit - operatingExpenses;

        // Group expense breakdown
        var expenseCategoryGroups = expenses
            .GroupBy(e => e.ExpenseCategory?.Name ?? "Uncategorized")
            .Select(g => new CategoryBreakdownDto(
                g.Key,
                g.Sum(x => x.Amount),
                totalExpenses > 0 ? (double)Math.Round(g.Sum(x => x.Amount) / totalExpenses * 100, 2) : 0
            ))
            .OrderByDescending(x => x.Amount)
            .ToList();

        var revenueBreakdown = new List<CategoryBreakdownDto>
        {
            new CategoryBreakdownDto("Direct Invoiced Revenue", operatingRevenue, 100.0)
        };

        return new ProfitAndLossReportDto(
            start,
            end,
            operatingRevenue,
            cogs,
            grossProfit,
            operatingExpenses,
            netIncome,
            revenueBreakdown,
            expenseCategoryGroups
        );
    }

    public async Task<CashFlowStatementReportDto> GenerateCashFlowStatementAsync(Guid organizationId, DateTime start, DateTime end)
    {
        var cashEntries = await _context.CashFlowEntries
            .AsNoTracking()
            .Where(c => c.OrganizationId == organizationId && c.EntryDate >= start && c.EntryDate <= end && !c.IsDeleted)
            .ToListAsync();

        var operatingIn = cashEntries.Where(e => e.Type == "CashIn" && e.Category != "Investment" && e.Category != "Loan").Sum(e => e.Amount);
        var operatingOut = cashEntries.Where(e => e.Type == "CashOut" && e.Category != "Investment" && e.Category != "Loan").Sum(e => e.Amount);

        var cashFromOperating = operatingIn - operatingOut;
        var cashFromInvesting = cashEntries.Where(e => e.Category == "Investment").Sum(e => e.Type == "CashIn" ? e.Amount : -e.Amount);
        var cashFromFinancing = cashEntries.Where(e => e.Category == "Loan").Sum(e => e.Type == "CashIn" ? e.Amount : -e.Amount);

        var netIncrease = cashFromOperating + cashFromInvesting + cashFromFinancing;

        var beginningCash = await _context.Accounts
            .AsNoTracking()
            .Where(a => a.OrganizationId == organizationId && a.AccountType == "Asset" && !a.IsDeleted)
            .SumAsync(a => (decimal?)a.CurrentBalance) ?? 0m;

        var endingCash = beginningCash + netIncrease;

        return new CashFlowStatementReportDto(
            start,
            end,
            cashFromOperating,
            cashFromInvesting,
            cashFromFinancing,
            netIncrease,
            beginningCash,
            endingCash
        );
    }

    public async Task<ExpenseReportDto> GenerateExpenseReportAsync(Guid organizationId, DateTime start, DateTime end)
    {
        var expenses = await _context.Expenses
            .Include(e => e.ExpenseCategory)
            .AsNoTracking()
            .Where(e => e.OrganizationId == organizationId && e.ExpenseDate >= start && e.ExpenseDate <= end && !e.IsDeleted)
            .ToListAsync();

        var totalAmount = expenses.Sum(e => e.Amount);
        var totalCount = expenses.Count;

        var byCategory = expenses
            .GroupBy(e => e.ExpenseCategory?.Name ?? "Uncategorized")
            .Select(g => new CategoryBreakdownDto(
                g.Key,
                g.Sum(x => x.Amount),
                totalAmount > 0 ? (double)Math.Round(g.Sum(x => x.Amount) / totalAmount * 100, 2) : 0
            ))
            .OrderByDescending(x => x.Amount)
            .ToList();

        return new ExpenseReportDto(
            start,
            end,
            totalAmount,
            totalCount,
            byCategory
        );
    }

    public async Task<RevenueReportDto> GenerateRevenueReportAsync(Guid organizationId, DateTime start, DateTime end)
    {
        var invoices = await _context.FinanceInvoices
            .AsNoTracking()
            .Where(i => i.OrganizationId == organizationId && i.IssueDate >= start && i.IssueDate <= end && !i.IsDeleted)
            .ToListAsync();

        var totalRevenue = invoices.Sum(i => i.TotalAmount);
        var invoiceCount = invoices.Count;

        var topCustomers = invoices
            .GroupBy(i => i.CustomerName)
            .Select(g => new CustomerRevenueDto(
                g.Key,
                g.Sum(x => x.TotalAmount),
                g.Sum(x => x.AmountPaid)
            ))
            .OrderByDescending(x => x.TotalBilled)
            .Take(10)
            .ToList();

        return new RevenueReportDto(
            start,
            end,
            totalRevenue,
            invoiceCount,
            topCustomers
        );
    }
}
