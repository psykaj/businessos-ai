using backend.Modules.AccountsPayable.DTOs;
using backend.Modules.AccountsPayable.Entities;
using backend.Modules.AccountsPayable.Interfaces;
using backend.Modules.CashFlow.Entities;
using backend.Persistence;

namespace backend.Modules.AccountsPayable.Services;

public class AccountsPayableService : IAccountsPayableService
{
    private readonly IAccountsPayableRepository _repository;
    private readonly ApplicationDbContext _context;

    public AccountsPayableService(IAccountsPayableRepository repository, ApplicationDbContext context)
    {
        _repository = repository;
        _context = context;
    }

    public async Task<AccountsPayableDto> CreateBillAsync(Guid organizationId, CreateAccountsPayableDto dto)
    {
        var ap = new AccountsPayableRecord
        {
            OrganizationId = organizationId,
            BillNumber = dto.BillNumber,
            SupplierId = dto.SupplierId,
            SupplierName = dto.SupplierName,
            PurchaseOrderId = dto.PurchaseOrderId,
            TotalAmount = dto.TotalAmount,
            AmountPaid = 0,
            BalanceDue = dto.TotalAmount,
            DueDate = dto.DueDate,
            Status = dto.DueDate < DateTime.UtcNow ? "Overdue" : "Current",
            DaysOverdue = dto.DueDate < DateTime.UtcNow ? (DateTime.UtcNow - dto.DueDate).Days : 0
        };

        var created = await _repository.AddAsync(ap);
        return MapToDto(created);
    }

    public async Task<IEnumerable<AccountsPayableDto>> GetPayablesAsync(Guid organizationId, string? status = null)
    {
        var payables = await _repository.GetAllAsync(organizationId, status);
        return payables.Select(MapToDto);
    }

    public async Task<AccountsPayableDto?> RecordPaymentAsync(Guid id, Guid organizationId, decimal amountPaid)
    {
        var ap = await _repository.GetByIdAsync(id, organizationId);
        if (ap == null) return null;

        ap.AmountPaid += amountPaid;
        ap.BalanceDue = Math.Max(0, ap.TotalAmount - ap.AmountPaid);
        ap.Status = ap.BalanceDue <= 0 ? "Paid" : (ap.DueDate < DateTime.UtcNow ? "Overdue" : "Current");

        await _repository.UpdateAsync(ap);

        // Record CashOut Entry in CashFlow
        var cashFlow = new CashFlowEntry
        {
            OrganizationId = organizationId,
            EntryDate = DateTime.UtcNow,
            Type = "CashOut",
            Category = "APPayment",
            Amount = amountPaid,
            ReferenceType = "Bill",
            ReferenceId = ap.Id,
            Description = $"Supplier payment for Bill #{ap.BillNumber} ({ap.SupplierName})"
        };

        await _context.CashFlowEntries.AddAsync(cashFlow);
        await _context.SaveChangesAsync();

        return MapToDto(ap);
    }

    public async Task<AccountsPayableAgingDto> GetAgingReportAsync(Guid organizationId)
    {
        var payables = await _repository.GetAllAsync(organizationId);
        var now = DateTime.UtcNow;

        decimal totalOutstanding = 0;
        decimal current = 0;
        decimal days1To30 = 0;
        decimal days31To60 = 0;
        decimal days61To90 = 0;
        decimal days90Plus = 0;
        int totalOverdueBills = 0;

        foreach (var item in payables.Where(p => p.BalanceDue > 0))
        {
            totalOutstanding += item.BalanceDue;

            if (item.DueDate >= now)
            {
                current += item.BalanceDue;
            }
            else
            {
                totalOverdueBills++;
                var daysOverdue = (now - item.DueDate).Days;

                if (daysOverdue <= 30)
                {
                    days1To30 += item.BalanceDue;
                }
                else if (daysOverdue <= 60)
                {
                    days31To60 += item.BalanceDue;
                }
                else if (daysOverdue <= 90)
                {
                    days61To90 += item.BalanceDue;
                }
                else
                {
                    days90Plus += item.BalanceDue;
                }
            }
        }

        return new AccountsPayableAgingDto(
            totalOutstanding,
            current,
            days1To30,
            days31To60,
            days61To90,
            days90Plus,
            totalOverdueBills
        );
    }

    private static AccountsPayableDto MapToDto(AccountsPayableRecord p) => new(
        p.Id,
        p.OrganizationId,
        p.BillNumber,
        p.SupplierId,
        p.SupplierName,
        p.PurchaseOrderId,
        p.TotalAmount,
        p.AmountPaid,
        p.BalanceDue,
        p.DueDate,
        p.Status,
        p.DaysOverdue,
        p.CreatedAt
    );
}
