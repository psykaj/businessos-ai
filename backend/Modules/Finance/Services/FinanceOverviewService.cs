using backend.Modules.CashFlow.Interfaces;
using backend.Modules.Finance.DTOs;
using backend.Modules.Finance.Interfaces;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.Finance.Services;

public class FinanceOverviewService : IFinanceOverviewService
{
    private readonly ICashFlowEngine _cashFlowEngine;
    private readonly ApplicationDbContext _context;

    public FinanceOverviewService(ICashFlowEngine cashFlowEngine, ApplicationDbContext context)
    {
        _cashFlowEngine = cashFlowEngine;
        _context = context;
    }

    public async Task<FinanceOverviewDto> GetOverviewAsync(Guid organizationId)
    {
        var cashFlowSummary = await _cashFlowEngine.GetSummaryAsync(organizationId);

        var totalRevenue = await _context.FinanceInvoices
            .AsNoTracking()
            .Where(i => i.OrganizationId == organizationId && !i.IsDeleted)
            .SumAsync(i => (decimal?)i.TotalAmount) ?? 0m;

        var totalExpenses = await _context.Expenses
            .AsNoTracking()
            .Where(e => e.OrganizationId == organizationId && !e.IsDeleted)
            .SumAsync(e => (decimal?)e.Amount) ?? 0m;

        var netIncome = totalRevenue - totalExpenses;

        var pendingInvoicesCount = await _context.FinanceInvoices
            .AsNoTracking()
            .CountAsync(i => i.OrganizationId == organizationId && (i.Status == "Sent" || i.Status == "Partial") && !i.IsDeleted);

        var pendingBillsCount = await _context.AccountsPayable
            .AsNoTracking()
            .CountAsync(p => p.OrganizationId == organizationId && p.BalanceDue > 0 && !p.IsDeleted);

        var profitMargin = totalRevenue > 0 ? (decimal)Math.Round((netIncome / totalRevenue) * 100, 2) : 0m;

        return new FinanceOverviewDto(
            totalRevenue,
            totalExpenses,
            netIncome,
            cashFlowSummary.CurrentCashPosition,
            cashFlowSummary.OutstandingReceivables,
            cashFlowSummary.OutstandingPayables,
            pendingInvoicesCount,
            pendingBillsCount,
            profitMargin
        );
    }
}
