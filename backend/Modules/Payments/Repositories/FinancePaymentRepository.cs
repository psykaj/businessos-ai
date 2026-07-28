using backend.Modules.Payments.Entities;
using backend.Modules.Payments.Interfaces;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.Payments.Repositories;

public class FinancePaymentRepository : IFinancePaymentRepository
{
    private readonly ApplicationDbContext _context;

    public FinancePaymentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<FinancePayment?> GetByIdAsync(Guid id, Guid organizationId)
    {
        return await _context.FinancePayments
            .FirstOrDefaultAsync(p => p.Id == id && p.OrganizationId == organizationId && !p.IsDeleted);
    }

    public async Task<IEnumerable<FinancePayment>> GetAllAsync(Guid organizationId, string? paymentType = null)
    {
        var query = _context.FinancePayments
            .AsNoTracking()
            .Where(p => p.OrganizationId == organizationId && !p.IsDeleted);

        if (!string.IsNullOrWhiteSpace(paymentType))
        {
            query = query.Where(p => p.PaymentType == paymentType);
        }

        return await query.OrderByDescending(p => p.PaymentDate).ToListAsync();
    }

    public async Task<FinancePayment> AddAsync(FinancePayment payment)
    {
        await _context.FinancePayments.AddAsync(payment);
        await _context.SaveChangesAsync();
        return payment;
    }
}
