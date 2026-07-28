using backend.Modules.CashFlow.DTOs;
using backend.Modules.CashFlow.Entities;
using backend.Modules.CashFlow.Interfaces;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.CashFlow.Services;

public class CashFlowEngine : ICashFlowEngine
{
    private readonly ApplicationDbContext _context;

    public CashFlowEngine(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CashFlowSummaryDto> GetSummaryAsync(Guid organizationId, DateTime? startDate = null, DateTime? endDate = null)
    {
        var start = startDate ?? DateTime.UtcNow.AddYears(-1);
        var end = endDate ?? DateTime.UtcNow;

        var entries = await _context.CashFlowEntries
            .AsNoTracking()
            .Where(c => c.OrganizationId == organizationId && c.EntryDate >= start && c.EntryDate <= end && !c.IsDeleted)
            .ToListAsync();

        var totalCashIn = entries.Where(e => e.Type == "CashIn").Sum(e => e.Amount);
        var totalCashOut = entries.Where(e => e.Type == "CashOut").Sum(e => e.Amount);

        // Include Expenses if not explicitly in entries
        var expenses = await _context.Expenses
            .AsNoTracking()
            .Where(e => e.OrganizationId == organizationId && e.ExpenseDate >= start && e.ExpenseDate <= end && !e.IsDeleted)
            .SumAsync(e => (decimal?)e.Amount) ?? 0m;

        // Take max of tracked cash out vs recorded expenses to ensure full coverage
        if (totalCashOut < expenses)
        {
            totalCashOut = expenses;
        }

        var netCashFlow = totalCashIn - totalCashOut;

        // Current Bank / Asset Account Balance
        var bankBalance = await _context.Accounts
            .AsNoTracking()
            .Where(a => a.OrganizationId == organizationId && a.AccountType == "Asset" && !a.IsDeleted)
            .SumAsync(a => (decimal?)a.CurrentBalance) ?? 0m;

        var currentCashPosition = bankBalance + netCashFlow;

        // AR & AP Outstanding
        var arOutstanding = await _context.AccountsReceivable
            .AsNoTracking()
            .Where(a => a.OrganizationId == organizationId && a.BalanceDue > 0 && !a.IsDeleted)
            .SumAsync(a => (decimal?)a.BalanceDue) ?? 0m;

        var apOutstanding = await _context.AccountsPayable
            .AsNoTracking()
            .Where(a => a.OrganizationId == organizationId && a.BalanceDue > 0 && !a.IsDeleted)
            .SumAsync(a => (decimal?)a.BalanceDue) ?? 0m;

        var estimatedNetProfit = totalCashIn - totalCashOut;

        return new CashFlowSummaryDto(
            totalCashIn,
            totalCashOut,
            currentCashPosition,
            netCashFlow,
            arOutstanding,
            apOutstanding,
            estimatedNetProfit
        );
    }

    public async Task<IEnumerable<MonthlyCashFlowPointDto>> GetMonthlyCashFlowAsync(Guid organizationId, int months = 6)
    {
        var result = new List<MonthlyCashFlowPointDto>();
        var now = DateTime.UtcNow;

        for (int i = months - 1; i >= 0; i--)
        {
            var monthStart = new DateTime(now.Year, now.Month, 1).AddMonths(-i);
            var monthEnd = monthStart.AddMonths(1).AddTicks(-1);

            var monthEntries = await _context.CashFlowEntries
                .AsNoTracking()
                .Where(c => c.OrganizationId == organizationId && c.EntryDate >= monthStart && c.EntryDate <= monthEnd && !c.IsDeleted)
                .ToListAsync();

            var cashIn = monthEntries.Where(e => e.Type == "CashIn").Sum(e => e.Amount);
            var cashOut = monthEntries.Where(e => e.Type == "CashOut").Sum(e => e.Amount);

            var monthExpenses = await _context.Expenses
                .AsNoTracking()
                .Where(e => e.OrganizationId == organizationId && e.ExpenseDate >= monthStart && e.ExpenseDate <= monthEnd && !e.IsDeleted)
                .SumAsync(e => (decimal?)e.Amount) ?? 0m;

            if (cashOut < monthExpenses) cashOut = monthExpenses;

            result.Add(new MonthlyCashFlowPointDto(
                monthStart.ToString("MMM yyyy"),
                cashIn,
                cashOut,
                cashIn - cashOut
            ));
        }

        return result;
    }

    public async Task<CashFlowForecastDto> GetForecastAsync(Guid organizationId)
    {
        var summary = await GetSummaryAsync(organizationId);

        // Projected receivables expected in next 30 days
        var now = DateTime.UtcNow;
        var next30 = now.AddDays(30);

        var projectedIn = await _context.AccountsReceivable
            .AsNoTracking()
            .Where(a => a.OrganizationId == organizationId && a.DueDate <= next30 && a.BalanceDue > 0 && !a.IsDeleted)
            .SumAsync(a => (decimal?)a.BalanceDue) ?? 0m;

        var projectedOut = await _context.AccountsPayable
            .AsNoTracking()
            .Where(a => a.OrganizationId == organizationId && a.DueDate <= next30 && a.BalanceDue > 0 && !a.IsDeleted)
            .SumAsync(a => (decimal?)a.BalanceDue) ?? 0m;

        var recurringExpenses = await _context.Expenses
            .AsNoTracking()
            .Where(e => e.OrganizationId == organizationId && e.IsRecurring && !e.IsDeleted)
            .SumAsync(e => (decimal?)e.Amount) ?? 0m;

        projectedOut += recurringExpenses;

        var projectedPosition = summary.CurrentCashPosition + projectedIn - projectedOut;

        var monthlyDetails = (await GetMonthlyCashFlowAsync(organizationId, 6)).ToList();

        return new CashFlowForecastDto(
            projectedIn,
            projectedOut,
            projectedPosition,
            monthlyDetails
        );
    }

    public async Task<CashFlowEntryDto> AddEntryAsync(Guid organizationId, CreateCashFlowEntryDto dto)
    {
        var entry = new CashFlowEntry
        {
            OrganizationId = organizationId,
            EntryDate = dto.EntryDate,
            Type = dto.Type,
            Category = dto.Category,
            Amount = dto.Amount,
            AccountId = dto.AccountId,
            ReferenceType = dto.ReferenceType,
            ReferenceId = dto.ReferenceId,
            Description = dto.Description
        };

        await _context.CashFlowEntries.AddAsync(entry);
        await _context.SaveChangesAsync();

        return new CashFlowEntryDto(
            entry.Id,
            entry.OrganizationId,
            entry.EntryDate,
            entry.Type,
            entry.Category,
            entry.Amount,
            entry.AccountId,
            entry.ReferenceType,
            entry.ReferenceId,
            entry.Description,
            entry.CreatedAt
        );
    }
}
