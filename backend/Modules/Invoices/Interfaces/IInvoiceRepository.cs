using backend.Modules.Invoices.Entities;

namespace backend.Modules.Invoices.Interfaces;

public interface IInvoiceRepository
{
    Task<FinanceInvoice?> GetByIdAsync(Guid id, Guid organizationId);
    Task<FinanceInvoice?> GetByNumberAsync(string invoiceNumber, Guid organizationId);
    Task<IEnumerable<FinanceInvoice>> GetAllAsync(Guid organizationId, string? status = null, Guid? customerId = null);
    Task<FinanceInvoice> AddAsync(FinanceInvoice invoice);
    Task UpdateAsync(FinanceInvoice invoice);
    Task DeleteAsync(FinanceInvoice invoice);
}
