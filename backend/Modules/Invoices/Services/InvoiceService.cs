using backend.Modules.AccountsReceivable.Entities;
using backend.Modules.CashFlow.Entities;
using backend.Modules.Invoices.DTOs;
using backend.Modules.Invoices.Entities;
using backend.Modules.Invoices.Interfaces;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.Invoices.Services;

public class InvoiceService : IInvoiceService
{
    private readonly IInvoiceRepository _repository;
    private readonly ApplicationDbContext _context;

    public InvoiceService(IInvoiceRepository repository, ApplicationDbContext context)
    {
        _repository = repository;
        _context = context;
    }

    public async Task<FinanceInvoiceDto> CreateInvoiceAsync(Guid organizationId, CreateFinanceInvoiceDto dto)
    {
        var invoice = new FinanceInvoice
        {
            OrganizationId = organizationId,
            InvoiceNumber = string.IsNullOrWhiteSpace(dto.InvoiceNumber) 
                ? $"INV-{DateTime.UtcNow:yyyyMMdd}-{Random.Shared.Next(1000, 9999)}" 
                : dto.InvoiceNumber,
            CustomerId = dto.CustomerId,
            CustomerName = dto.CustomerName,
            CustomerEmail = dto.CustomerEmail,
            IssueDate = dto.IssueDate,
            DueDate = dto.DueDate,
            Currency = dto.Currency,
            Notes = dto.Notes,
            Terms = dto.Terms,
            Status = "Sent"
        };

        if (dto.Items != null && dto.Items.Any())
        {
            foreach (var itemDto in dto.Items)
            {
                var lineSubtotal = itemDto.Quantity * itemDto.UnitPrice - itemDto.DiscountAmount;
                var tax = lineSubtotal * (itemDto.TaxRate / 100m);
                var total = lineSubtotal + tax;

                invoice.Items.Add(new FinanceInvoiceItem
                {
                    ProductId = itemDto.ProductId,
                    Description = itemDto.Description,
                    Quantity = itemDto.Quantity,
                    UnitPrice = itemDto.UnitPrice,
                    DiscountAmount = itemDto.DiscountAmount,
                    TaxRate = itemDto.TaxRate,
                    TaxAmount = tax,
                    TotalAmount = total
                });
            }
        }

        invoice.SubTotal = invoice.Items.Sum(i => i.Quantity * i.UnitPrice);
        invoice.DiscountTotal = invoice.Items.Sum(i => i.DiscountAmount);
        invoice.TaxTotal = invoice.Items.Sum(i => i.TaxAmount);
        invoice.TotalAmount = invoice.Items.Sum(i => i.TotalAmount);
        invoice.AmountPaid = 0;
        invoice.BalanceDue = invoice.TotalAmount;

        var created = await _repository.AddAsync(invoice);

        // Auto-create Accounts Receivable record for tracking
        var arEntry = new AccountsReceivableRecord
        {
            OrganizationId = organizationId,
            InvoiceId = created.Id,
            CustomerId = created.CustomerId,
            CustomerName = created.CustomerName,
            TotalAmount = created.TotalAmount,
            AmountPaid = 0,
            BalanceDue = created.TotalAmount,
            DueDate = created.DueDate,
            Status = created.DueDate < DateTime.UtcNow ? "Overdue" : "Current",
            DaysOverdue = created.DueDate < DateTime.UtcNow ? (DateTime.UtcNow - created.DueDate).Days : 0
        };

        await _context.AccountsReceivable.AddAsync(arEntry);
        await _context.SaveChangesAsync();

        return MapToDto(created);
    }

    public async Task<FinanceInvoiceDto?> GetInvoiceByIdAsync(Guid id, Guid organizationId)
    {
        var invoice = await _repository.GetByIdAsync(id, organizationId);
        return invoice != null ? MapToDto(invoice) : null;
    }

    public async Task<IEnumerable<FinanceInvoiceDto>> GetInvoicesAsync(Guid organizationId, string? status = null, Guid? customerId = null)
    {
        var invoices = await _repository.GetAllAsync(organizationId, status, customerId);
        return invoices.Select(MapToDto);
    }

    public async Task<FinanceInvoiceDto?> RecordPaymentAsync(Guid invoiceId, Guid organizationId, decimal paymentAmount)
    {
        var invoice = await _repository.GetByIdAsync(invoiceId, organizationId);
        if (invoice == null) return null;

        invoice.AmountPaid += paymentAmount;
        invoice.BalanceDue = Math.Max(0, invoice.TotalAmount - invoice.AmountPaid);

        if (invoice.BalanceDue <= 0)
        {
            invoice.Status = "Paid";
        }
        else
        {
            invoice.Status = "Partial";
        }

        await _repository.UpdateAsync(invoice);

        // Sync Accounts Receivable
        var arEntry = await _context.AccountsReceivable
            .FirstOrDefaultAsync(a => a.InvoiceId == invoiceId && a.OrganizationId == organizationId);

        if (arEntry != null)
        {
            arEntry.AmountPaid = invoice.AmountPaid;
            arEntry.BalanceDue = invoice.BalanceDue;
            arEntry.Status = invoice.BalanceDue <= 0 ? "Paid" : (invoice.DueDate < DateTime.UtcNow ? "Overdue" : "Current");
            _context.AccountsReceivable.Update(arEntry);
        }

        // Record Cash Flow Entry (CashIn)
        var cashFlow = new CashFlowEntry
        {
            OrganizationId = organizationId,
            EntryDate = DateTime.UtcNow,
            Type = "CashIn",
            Category = "ARCollection",
            Amount = paymentAmount,
            ReferenceType = "Invoice",
            ReferenceId = invoice.Id,
            Description = $"Payment received for Invoice #{invoice.InvoiceNumber} ({invoice.CustomerName})"
        };

        await _context.CashFlowEntries.AddAsync(cashFlow);
        await _context.SaveChangesAsync();

        return MapToDto(invoice);
    }

    public async Task<bool> DeleteInvoiceAsync(Guid id, Guid organizationId)
    {
        var invoice = await _repository.GetByIdAsync(id, organizationId);
        if (invoice == null) return false;

        await _repository.DeleteAsync(invoice);
        return true;
    }

    public async Task<InvoiceSummaryDto> GetInvoiceSummaryAsync(Guid organizationId)
    {
        var invoices = await _repository.GetAllAsync(organizationId);
        var totalInvoiced = invoices.Sum(i => i.TotalAmount);
        var totalPaid = invoices.Sum(i => i.AmountPaid);
        var totalOverdue = invoices.Where(i => i.DueDate < DateTime.UtcNow && i.BalanceDue > 0).Sum(i => i.BalanceDue);
        var totalCount = invoices.Count();
        var pendingCount = invoices.Count(i => i.Status == "Sent" || i.Status == "Partial");
        var overdueCount = invoices.Count(i => i.DueDate < DateTime.UtcNow && i.BalanceDue > 0);

        return new InvoiceSummaryDto(
            totalInvoiced,
            totalPaid,
            totalOverdue,
            totalCount,
            pendingCount,
            overdueCount
        );
    }

    private static FinanceInvoiceDto MapToDto(FinanceInvoice i) => new(
        i.Id,
        i.OrganizationId,
        i.InvoiceNumber,
        i.CustomerId,
        i.CustomerName,
        i.CustomerEmail,
        i.IssueDate,
        i.DueDate,
        i.Status,
        i.SubTotal,
        i.TaxTotal,
        i.DiscountTotal,
        i.TotalAmount,
        i.AmountPaid,
        i.BalanceDue,
        i.Currency,
        i.Notes,
        i.Terms,
        i.PdfUrl,
        i.Items.Select(item => new FinanceInvoiceItemDto(
            item.Id,
            item.ProductId,
            item.Description,
            item.Quantity,
            item.UnitPrice,
            item.DiscountAmount,
            item.TaxRate,
            item.TaxAmount,
            item.TotalAmount
        )).ToList(),
        i.CreatedAt
    );
}
