using backend.Modules.AccountsReceivable.DTOs;
using backend.Modules.AccountsReceivable.Entities;
using backend.Modules.AccountsReceivable.Interfaces;

namespace backend.Modules.AccountsReceivable.Services;

public class AccountsReceivableService : IAccountsReceivableService
{
    private readonly IAccountsReceivableRepository _repository;

    public AccountsReceivableService(IAccountsReceivableRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<AccountsReceivableDto>> GetReceivablesAsync(Guid organizationId, string? status = null)
    {
        var receivables = await _repository.GetAllAsync(organizationId, status);
        return receivables.Select(MapToDto);
    }

    public async Task<AccountsReceivableAgingDto> GetAgingReportAsync(Guid organizationId)
    {
        var receivables = await _repository.GetAllAsync(organizationId);
        var now = DateTime.UtcNow;

        decimal totalOutstanding = 0;
        decimal current = 0;
        decimal days1To30 = 0;
        decimal days31To60 = 0;
        decimal days61To90 = 0;
        decimal days90Plus = 0;
        int totalOverdueInvoices = 0;

        foreach (var item in receivables.Where(r => r.BalanceDue > 0))
        {
            totalOutstanding += item.BalanceDue;

            if (item.DueDate >= now)
            {
                current += item.BalanceDue;
            }
            else
            {
                totalOverdueInvoices++;
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

        return new AccountsReceivableAgingDto(
            totalOutstanding,
            current,
            days1To30,
            days31To60,
            days61To90,
            days90Plus,
            totalOverdueInvoices
        );
    }

    public async Task<bool> SendOverdueReminderAsync(Guid id, Guid organizationId, string? customNote)
    {
        var ar = await _repository.GetByIdAsync(id, organizationId);
        if (ar == null) return false;

        ar.LastReminderSentAt = DateTime.UtcNow;
        await _repository.UpdateAsync(ar);
        return true;
    }

    private static AccountsReceivableDto MapToDto(AccountsReceivableRecord r) => new(
        r.Id,
        r.OrganizationId,
        r.InvoiceId,
        r.CustomerId,
        r.CustomerName,
        r.TotalAmount,
        r.AmountPaid,
        r.BalanceDue,
        r.DueDate,
        r.Status,
        r.DaysOverdue,
        r.LastReminderSentAt,
        r.CreatedAt
    );
}
