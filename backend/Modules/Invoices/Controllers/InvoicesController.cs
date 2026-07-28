using backend.Modules.Finance.Controllers;
using backend.Modules.Invoices.DTOs;
using backend.Modules.Invoices.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.Invoices.Controllers;

[Route("api/invoices")]
public class InvoicesController : BaseFinanceController
{
    private readonly IInvoiceService _invoiceService;

    public InvoicesController(IInvoiceService invoiceService)
    {
        _invoiceService = invoiceService;
    }

    [HttpGet]
    public async Task<IActionResult> GetInvoices([FromQuery] string? status, [FromQuery] Guid? customerId)
    {
        var orgId = GetOrganizationId();
        var invoices = await _invoiceService.GetInvoicesAsync(orgId, status, customerId);
        return Ok(invoices);
    }

    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary()
    {
        var orgId = GetOrganizationId();
        var summary = await _invoiceService.GetInvoiceSummaryAsync(orgId);
        return Ok(summary);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetInvoiceById(Guid id)
    {
        var orgId = GetOrganizationId();
        var invoice = await _invoiceService.GetInvoiceByIdAsync(id, orgId);
        if (invoice == null) return NotFound(new { message = "Invoice not found" });
        return Ok(invoice);
    }

    [HttpPost]
    public async Task<IActionResult> CreateInvoice([FromBody] CreateFinanceInvoiceDto dto)
    {
        var orgId = GetOrganizationId();
        var created = await _invoiceService.CreateInvoiceAsync(orgId, dto);
        return CreatedAtAction(nameof(GetInvoiceById), new { id = created.Id }, created);
    }

    [HttpPost("{id:guid}/pay")]
    public async Task<IActionResult> RecordPayment(Guid id, [FromBody] RecordInvoicePaymentDto request)
    {
        var orgId = GetOrganizationId();
        var updated = await _invoiceService.RecordPaymentAsync(id, orgId, request.Amount);
        if (updated == null) return NotFound(new { message = "Invoice not found" });
        return Ok(updated);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteInvoice(Guid id)
    {
        var orgId = GetOrganizationId();
        var success = await _invoiceService.DeleteInvoiceAsync(id, orgId);
        if (!success) return NotFound(new { message = "Invoice not found" });
        return NoContent();
    }
}

public record RecordInvoicePaymentDto(decimal Amount);
