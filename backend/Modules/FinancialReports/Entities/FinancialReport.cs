using backend.Common;

namespace backend.Modules.FinancialReports.Entities;

public class FinancialReport : BaseEntity
{
    public Guid OrganizationId { get; set; }
    
    // ProfitAndLoss, CashFlow, ExpenseReport, RevenueReport, OutstandingPayments, TaxSummary
    public string ReportType { get; set; } = "ProfitAndLoss";
    
    public DateTime PeriodStart { get; set; }
    public DateTime PeriodEnd { get; set; }
    
    public string ReportDataJson { get; set; } = "{}";
    
    public string? GeneratedBy { get; set; }
    public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
}
