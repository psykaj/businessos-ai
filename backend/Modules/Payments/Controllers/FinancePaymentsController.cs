using backend.Modules.Finance.Controllers;
using backend.Modules.Payments.DTOs;
using backend.Modules.Payments.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.Payments.Controllers;

[Route("api/finance/payments")]
public class FinancePaymentsController : BaseFinanceController
{
    private readonly IFinancePaymentService _paymentService;

    public FinancePaymentsController(IFinancePaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpGet]
    public async Task<IActionResult> GetPayments([FromQuery] string? paymentType)
    {
        var orgId = GetOrganizationId();
        var payments = await _paymentService.GetPaymentsAsync(orgId, paymentType);
        return Ok(payments);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetPaymentById(Guid id)
    {
        var orgId = GetOrganizationId();
        var payment = await _paymentService.GetPaymentByIdAsync(id, orgId);
        if (payment == null) return NotFound(new { message = "Payment record not found" });
        return Ok(payment);
    }

    [HttpPost]
    public async Task<IActionResult> RecordPayment([FromBody] CreateFinancePaymentDto dto)
    {
        var orgId = GetOrganizationId();
        var created = await _paymentService.RecordPaymentAsync(orgId, dto);
        return CreatedAtAction(nameof(GetPaymentById), new { id = created.Id }, created);
    }
}
