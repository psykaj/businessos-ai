namespace backend.Modules.Finance.DTOs;

public record FinanceOverviewDto(
    decimal TotalRevenue,
    decimal TotalExpenses,
    decimal NetIncome,
    decimal CashPosition,
    decimal TotalReceivables,
    decimal TotalPayables,
    int PendingInvoicesCount,
    int PendingBillsCount,
    decimal ProfitMarginPercentage
);
