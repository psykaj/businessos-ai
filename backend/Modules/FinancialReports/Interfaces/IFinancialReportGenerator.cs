using backend.Modules.FinancialReports.DTOs;

namespace backend.Modules.FinancialReports.Interfaces;

public interface IFinancialReportGenerator
{
    Task<ProfitAndLossReportDto> GenerateProfitAndLossAsync(Guid organizationId, DateTime start, DateTime end);
    Task<CashFlowStatementReportDto> GenerateCashFlowStatementAsync(Guid organizationId, DateTime start, DateTime end);
    Task<ExpenseReportDto> GenerateExpenseReportAsync(Guid organizationId, DateTime start, DateTime end);
    Task<RevenueReportDto> GenerateRevenueReportAsync(Guid organizationId, DateTime start, DateTime end);
}
