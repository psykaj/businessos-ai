using backend.Modules.Billing.DTOs;

namespace backend.Modules.Billing.Interfaces;

public interface IBillingService
{
    Task<List<PlanDto>> GetPlansAsync();
    Task<SubscriptionDto> GetSubscriptionAsync(Guid organizationId);
    Task<UsageMetricsDto> GetUsageMetricsAsync(Guid organizationId);
    Task<List<InvoiceDto>> GetInvoicesAsync(Guid organizationId);
    Task<List<PaymentHistoryDto>> GetPaymentHistoryAsync(Guid organizationId);
    Task<bool> CancelSubscriptionAsync(Guid organizationId);
    Task<bool> ResumeSubscriptionAsync(Guid organizationId);
    
    // Razorpay specific flow
    Task<RazorpayOrderResponseDto> CreateRazorpayOrderAsync(Guid organizationId, string planId, string billingCycle);
    Task<bool> VerifyRazorpayPaymentAsync(Guid organizationId, string paymentId, string orderId, string signature);
}
