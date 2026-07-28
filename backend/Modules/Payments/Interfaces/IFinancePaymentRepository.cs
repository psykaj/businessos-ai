using backend.Modules.Payments.Entities;

namespace backend.Modules.Payments.Interfaces;

public interface IFinancePaymentRepository
{
    Task<FinancePayment?> GetByIdAsync(Guid id, Guid organizationId);
    Task<IEnumerable<FinancePayment>> GetAllAsync(Guid organizationId, string? paymentType = null);
    Task<FinancePayment> AddAsync(FinancePayment payment);
}
