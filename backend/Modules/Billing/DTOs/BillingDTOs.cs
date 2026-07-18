using System.Text.Json.Serialization;

namespace backend.Modules.Billing.DTOs;

public class PlanDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal MonthlyPrice { get; set; }
    public decimal YearlyPrice { get; set; }
    public List<string> Features { get; set; } = new();
    public PlanLimitsDto Limits { get; set; } = new();
    public bool? IsPopular { get; set; }
}

public class PlanLimitsDto
{
    public int Qrs { get; set; }
    public int Scans { get; set; }
    public int StorageMB { get; set; }
    public int AiCredits { get; set; }
    public int ApiRequests { get; set; }
    public int TeamMembers { get; set; }
}

public class SubscriptionDto
{
    public string Id { get; set; } = string.Empty;
    public string PlanId { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime CurrentPeriodEnd { get; set; }
    public bool CancelAtPeriodEnd { get; set; }
    public PlanDto Plan { get; set; } = new();
}

public class UsageMetricDto
{
    public int Current { get; set; }
    public int Limit { get; set; }
}

public class UsageMetricsDto
{
    public UsageMetricDto Qrs { get; set; } = new();
    public UsageMetricDto Scans { get; set; } = new();
    public UsageMetricDto StorageMB { get; set; } = new();
    public UsageMetricDto AiCredits { get; set; } = new();
    public UsageMetricDto ApiRequests { get; set; } = new();
    public UsageMetricDto TeamMembers { get; set; } = new();
}

public class InvoiceDto
{
    public string Id { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string? PdfUrl { get; set; }
}

public class PaymentHistoryDto
{
    public string Id { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string PlanName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Status { get; set; } = string.Empty;
    public string TransactionId { get; set; } = string.Empty;
    public string? InvoiceId { get; set; }
}

public class RazorpayOrderRequestDto
{
    public string PlanId { get; set; } = string.Empty;
    public string BillingCycle { get; set; } = string.Empty;
}

public class RazorpayOrderResponseDto
{
    public string OrderId { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Key { get; set; } = string.Empty;
}

public class RazorpayVerifyRequestDto
{
    public string PaymentId { get; set; } = string.Empty;
    public string OrderId { get; set; } = string.Empty;
    public string Signature { get; set; } = string.Empty;
}
