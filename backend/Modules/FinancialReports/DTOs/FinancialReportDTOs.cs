namespace backend.Modules.FinancialReports.DTOs;

public record ProfitAndLossReportDto(
    DateTime PeriodStart,
    DateTime PeriodEnd,
    decimal OperatingRevenue,
    decimal CostOfGoodsSold,
    decimal GrossProfit,
    decimal OperatingExpenses,
    decimal NetIncome,
    List<CategoryBreakdownDto> RevenueBreakdown,
    List<CategoryBreakdownDto> ExpenseBreakdown
);

public record CategoryBreakdownDto(
    string CategoryName,
    decimal Amount,
    double Percentage
);

public record CashFlowStatementReportDto(
    DateTime PeriodStart,
    DateTime PeriodEnd,
    decimal CashFromOperatingActivities,
    decimal CashFromInvestingActivities,
    decimal CashFromFinancingActivities,
    decimal NetIncreaseInCash,
    decimal BeginningCashBalance,
    decimal EndingCashBalance
);

public record ExpenseReportDto(
    DateTime PeriodStart,
    DateTime PeriodEnd,
    decimal TotalExpenseAmount,
    int TotalExpenseCount,
    List<CategoryBreakdownDto> ExpensesByCategory
);

public record RevenueReportDto(
    DateTime PeriodStart,
    DateTime PeriodEnd,
    decimal TotalRevenue,
    int InvoiceCount,
    List<CustomerRevenueDto> TopCustomers
);

public record CustomerRevenueDto(
    string CustomerName,
    decimal TotalBilled,
    decimal TotalPaid
);
