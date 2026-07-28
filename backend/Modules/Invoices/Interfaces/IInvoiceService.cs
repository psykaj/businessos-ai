using backend.Modules.Invoices.DTOs;

namespace backend.Modules.Invoices.Interfaces;

public interface IInvoiceService
{
    Task<FinanceInvoiceDto> CreateInvoiceAsync(Guid organizationId, CreateFinanceInvoiceDto dto);
    Task<FinanceInvoiceDto?> GetInvoiceByIdAsync(Guid id, Guid organizationId);
    Task<IEnumerable<FinanceInvoiceDto>> GetInvoicesAsync(Guid organizationId, string? status = null, Guid? customerId = null);
    Task<FinanceInvoiceDto?> RecordPaymentAsync(Guid invoiceId, Guid organizationId, decimal paymentAmount);
    Task<bool> DeleteInvoiceAsync(Guid id, Guid organizationId);
    Task<InvoiceSummaryDto> GetInvoiceSummaryAsync(Guid organizationId);
}
