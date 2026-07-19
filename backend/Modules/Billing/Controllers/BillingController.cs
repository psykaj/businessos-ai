using backend.Modules.Billing.DTOs;
using backend.Modules.Billing.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.Billing.Controllers;

[ApiController]
[Route("api/billing")]
[Authorize]
public class BillingController : ControllerBase
{
    private readonly IBillingService _billingService;
    private readonly backend.Persistence.ApplicationDbContext _context;

    public BillingController(IBillingService billingService, backend.Persistence.ApplicationDbContext context)
    {
        _billingService = billingService;
        _context = context;
    }

    private async Task<Guid> GetOrganizationIdAsync(CancellationToken cancellationToken = default)
    {
        var orgIdClaim = User.FindFirst("organizationId")?.Value;
        if (Guid.TryParse(orgIdClaim, out var orgId))
        {
            return orgId;
        }

        var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
            throw new UnauthorizedAccessException("User not authenticated");

        var user = await _context.Users.FindAsync(new object[] { userId }, cancellationToken);
        if (user != null && user.OrganizationId.HasValue)
        {
            return user.OrganizationId.Value;
        }

        throw new UnauthorizedAccessException("Organization ID not found.");
    }

    [HttpGet("plans")]
    [AllowAnonymous] // Usually plans are public, but kept under auth based on preference
    public async Task<ActionResult<IEnumerable<PlanDto>>> GetPlans()
    {
        var plans = await _billingService.GetPlansAsync();
        return Ok(plans);
    }

    [HttpGet("subscription")]
    public async Task<ActionResult<SubscriptionDto>> GetSubscription(CancellationToken cancellationToken)
    {
        var orgId = await GetOrganizationIdAsync(cancellationToken);
        var subscription = await _billingService.GetSubscriptionAsync(orgId);
        return Ok(subscription);
    }

    [HttpGet("usage")]
    public async Task<ActionResult<UsageMetricsDto>> GetUsage(CancellationToken cancellationToken)
    {
        var orgId = await GetOrganizationIdAsync(cancellationToken);
        var usage = await _billingService.GetUsageMetricsAsync(orgId);
        return Ok(usage);
    }

    [HttpGet("invoices")]
    public async Task<ActionResult<IEnumerable<InvoiceDto>>> GetInvoices(CancellationToken cancellationToken)
    {
        var orgId = await GetOrganizationIdAsync(cancellationToken);
        var invoices = await _billingService.GetInvoicesAsync(orgId);
        return Ok(invoices);
    }

    [HttpGet("payments")]
    public async Task<ActionResult<IEnumerable<PaymentHistoryDto>>> GetPayments(CancellationToken cancellationToken)
    {
        var orgId = await GetOrganizationIdAsync(cancellationToken);
        var payments = await _billingService.GetPaymentHistoryAsync(orgId);
        return Ok(payments);
    }

    [HttpPost("subscription/cancel")]
    public async Task<ActionResult> CancelSubscription(CancellationToken cancellationToken)
    {
        var orgId = await GetOrganizationIdAsync(cancellationToken);
        var result = await _billingService.CancelSubscriptionAsync(orgId);
        if (result) return Ok();
        return BadRequest("Could not cancel subscription.");
    }

    [HttpPost("subscription/resume")]
    public async Task<ActionResult> ResumeSubscription(CancellationToken cancellationToken)
    {
        var orgId = await GetOrganizationIdAsync(cancellationToken);
        var result = await _billingService.ResumeSubscriptionAsync(orgId);
        if (result) return Ok();
        return BadRequest("Could not resume subscription.");
    }

    [HttpPost("checkout/razorpay")]
    public async Task<ActionResult<RazorpayOrderResponseDto>> CreateRazorpayOrder([FromBody] RazorpayOrderRequestDto request, CancellationToken cancellationToken)
    {
        var orgId = await GetOrganizationIdAsync(cancellationToken);
        var order = await _billingService.CreateRazorpayOrderAsync(orgId, request.PlanId, request.BillingCycle);
        return Ok(order);
    }

    [HttpPost("verify")]
    public async Task<ActionResult> VerifyPayment([FromBody] RazorpayVerifyRequestDto request, CancellationToken cancellationToken)
    {
        var orgId = await GetOrganizationIdAsync(cancellationToken);
        var isSuccess = await _billingService.VerifyRazorpayPaymentAsync(orgId, request.PaymentId, request.OrderId, request.Signature);
        if (isSuccess)
        {
            return Ok(new { success = true });
        }
        return BadRequest(new { success = false, message = "Payment verification failed" });
    }
}
