using backend.Entities;
using backend.Modules.Billing.DTOs;
using backend.Modules.Billing.Interfaces;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.Billing.Services;

public class BillingService : IBillingService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IRazorpayService _razorpayService;

    // Hardcoded plans for MVP, usually stored in DB or Stripe/Razorpay Products
    private static readonly List<PlanDto> Plans = new()
    {
        new PlanDto { Id = "plan_free", Name = "Free", MonthlyPrice = 0, YearlyPrice = 0, Features = new List<string> { "1 Dynamic QR Code" }, Limits = new PlanLimitsDto { Qrs = 1, Scans = 100 } },
        new PlanDto { Id = "plan_starter", Name = "Starter", MonthlyPrice = 15, YearlyPrice = 150, Features = new List<string> { "50 Dynamic QR Codes" }, Limits = new PlanLimitsDto { Qrs = 50, Scans = 10000 }, IsPopular = true },
        new PlanDto { Id = "plan_pro", Name = "Professional", MonthlyPrice = 49, YearlyPrice = 490, Features = new List<string> { "Unlimited QR Codes" }, Limits = new PlanLimitsDto { Qrs = 1000, Scans = 100000 } }
    };

    public BillingService(ApplicationDbContext dbContext, IRazorpayService razorpayService)
    {
        _dbContext = dbContext;
        _razorpayService = razorpayService;
    }

    public Task<List<PlanDto>> GetPlansAsync()
    {
        return Task.FromResult(Plans);
    }

    public async Task<SubscriptionDto> GetSubscriptionAsync(Guid organizationId)
    {
        var subscription = await _dbContext.Subscriptions
            .FirstOrDefaultAsync(s => s.OrganizationId == organizationId);

        if (subscription == null)
        {
            return new SubscriptionDto
            {
                Id = "sub_free",
                PlanId = "plan_free",
                Status = "active",
                Plan = Plans.First(p => p.Id == "plan_free")
            };
        }

        return new SubscriptionDto
        {
            Id = subscription.Id.ToString(),
            PlanId = subscription.PlanId,
            Status = subscription.Status,
            CurrentPeriodEnd = subscription.CurrentPeriodEnd,
            CancelAtPeriodEnd = subscription.CancelAtPeriodEnd,
            Plan = Plans.FirstOrDefault(p => p.Id == subscription.PlanId) ?? Plans.First()
        };
    }

    public async Task<UsageMetricsDto> GetUsageMetricsAsync(Guid organizationId)
    {
        var qrsCount = await _dbContext.QRCodes.CountAsync(q => q.OrganizationId == organizationId);
        
        return new UsageMetricsDto
        {
            Qrs = new UsageMetricDto { Current = qrsCount, Limit = 50 },
            Scans = new UsageMetricDto { Current = 1500, Limit = 10000 },
            StorageMB = new UsageMetricDto { Current = 15, Limit = 100 },
            AiCredits = new UsageMetricDto { Current = 10, Limit = 50 },
            ApiRequests = new UsageMetricDto { Current = 250, Limit = 1000 },
            TeamMembers = new UsageMetricDto { Current = 1, Limit = 1 }
        };
    }

    public async Task<List<InvoiceDto>> GetInvoicesAsync(Guid organizationId)
    {
        var invoices = await _dbContext.Invoices
            .Where(i => i.OrganizationId == organizationId)
            .OrderByDescending(i => i.CreatedAt)
            .Select(i => new InvoiceDto
            {
                Id = i.Id.ToString(),
                Number = i.Number,
                Amount = i.Amount,
                Status = i.Status,
                CreatedAt = i.CreatedAt,
                PdfUrl = i.PdfUrl
            })
            .ToListAsync();
            
        return invoices;
    }

    public async Task<List<PaymentHistoryDto>> GetPaymentHistoryAsync(Guid organizationId)
    {
        var payments = await _dbContext.Payments
            .Where(p => p.OrganizationId == organizationId)
            .OrderByDescending(p => p.CreatedAt)
            .Select(p => new PaymentHistoryDto
            {
                Id = p.Id.ToString(),
                Date = p.CreatedAt,
                PlanName = p.PlanName,
                Amount = p.Amount,
                Status = p.Status,
                TransactionId = p.TransactionId,
                InvoiceId = p.InvoiceId.ToString()
            })
            .ToListAsync();
            
        return payments;
    }

    public async Task<bool> CancelSubscriptionAsync(Guid organizationId)
    {
        var subscription = await _dbContext.Subscriptions
            .FirstOrDefaultAsync(s => s.OrganizationId == organizationId);
            
        if (subscription == null) return false;
        
        subscription.CancelAtPeriodEnd = true;
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ResumeSubscriptionAsync(Guid organizationId)
    {
        var subscription = await _dbContext.Subscriptions
            .FirstOrDefaultAsync(s => s.OrganizationId == organizationId);
            
        if (subscription == null) return false;
        
        subscription.CancelAtPeriodEnd = false;
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<RazorpayOrderResponseDto> CreateRazorpayOrderAsync(Guid organizationId, string planId, string billingCycle)
    {
        var plan = Plans.FirstOrDefault(p => p.Id == planId);
        if (plan == null) throw new ArgumentException("Invalid plan");

        decimal amount = billingCycle == "yearly" ? plan.YearlyPrice : plan.MonthlyPrice;
        
        // Ensure amount is at least 1 for testing, or rely on Razorpay limits
        if (amount <= 0) throw new ArgumentException("Amount must be greater than 0");

        var result = await _razorpayService.CreateOrderAsync(amount, "USD", $"receipt_{organizationId}_{DateTime.UtcNow.Ticks}");
        
        return new RazorpayOrderResponseDto
        {
            OrderId = result.OrderId,
            Amount = result.Amount,
            Key = result.Key
        };
    }

    public async Task<bool> VerifyRazorpayPaymentAsync(Guid organizationId, string paymentId, string orderId, string signature)
    {
        var isValid = _razorpayService.VerifySignature(paymentId, orderId, signature);
        
        if (isValid)
        {
            // Payment success, create or update subscription
            var subscription = await _dbContext.Subscriptions
                .FirstOrDefaultAsync(s => s.OrganizationId == organizationId);

            if (subscription == null)
            {
                subscription = new Subscription
                {
                    OrganizationId = organizationId,
                    PlanId = "plan_starter", // Hardcoded for simplicity, usually resolved from OrderId mappings
                    Status = "active",
                    CurrentPeriodEnd = DateTime.UtcNow.AddMonths(1),
                    RazorpaySubscriptionId = "sub_" + orderId
                };
                _dbContext.Subscriptions.Add(subscription);
            }
            else
            {
                subscription.PlanId = "plan_starter";
                subscription.Status = "active";
                subscription.CurrentPeriodEnd = DateTime.UtcNow.AddMonths(1);
            }

            // Create Invoice
            var invoice = new Invoice
            {
                OrganizationId = organizationId,
                Number = "INV-" + DateTime.UtcNow.ToString("yyyyMMdd-HHmmss"),
                Amount = 15,
                Status = "paid"
            };
            _dbContext.Invoices.Add(invoice);
            
            // Create Payment Log
            var payment = new Payment
            {
                OrganizationId = organizationId,
                PlanName = "Starter",
                Amount = 15,
                Status = "succeeded",
                TransactionId = paymentId,
                Invoice = invoice
            };
            _dbContext.Payments.Add(payment);
            
            await _dbContext.SaveChangesAsync();
            return true;
        }
        
        return false;
    }
}
