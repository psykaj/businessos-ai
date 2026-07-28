using backend.Modules.Invoices.Entities;
using backend.Modules.Invoices.Interfaces;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.Invoices.Repositories;

public class InvoiceRepository : IInvoiceRepository
{
    private readonly ApplicationDbContext _context;

    public InvoiceRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<FinanceInvoice?> GetByIdAsync(Guid id, Guid organizationId)
    {
        return await _context.FinanceInvoices
            .Include(i => i.Items)
            .FirstOrDefaultAsync(i => i.Id == id && i.OrganizationId == organizationId && !i.IsDeleted);
    }

    public async Task<FinanceInvoice?> GetByNumberAsync(string invoiceNumber, Guid organizationId)
    {
        return await _context.FinanceInvoices
            .Include(i => i.Items)
            .FirstOrDefaultAsync(i => i.InvoiceNumber == invoiceNumber && i.OrganizationId == organizationId && !i.IsDeleted);
    }

    public async Task<IEnumerable<FinanceInvoice>> GetAllAsync(Guid organizationId, string? status = null, Guid? customerId = null)
    {
        var query = _context.FinanceInvoices
            .Include(i => i.Items)
            .AsNoTracking()
            .Where(i => i.OrganizationId == organizationId && !i.IsDeleted);

        if (!string.IsNullOrWhiteSpace(status))
        {
            query = query.Where(i => i.Status == status);
        }

        if (customerId.HasValue)
        {
            query = query.Where(i => i.CustomerId == customerId.Value);
        }

        return await query.OrderByDescending(i => i.IssueDate).ToListAsync();
    }

    public async Task<FinanceInvoice> AddAsync(FinanceInvoice invoice)
    {
        await _context.FinanceInvoices.AddAsync(invoice);
        await _context.SaveChangesAsync();
        return invoice;
    }

    public async Task UpdateAsync(FinanceInvoice invoice)
    {
        _context.FinanceInvoices.Update(invoice);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(FinanceInvoice invoice)
    {
        invoice.IsDeleted = true;
        invoice.DeletedAt = DateTime.UtcNow;
        _context.FinanceInvoices.Update(invoice);
        await _context.SaveChangesAsync();
    }
}
