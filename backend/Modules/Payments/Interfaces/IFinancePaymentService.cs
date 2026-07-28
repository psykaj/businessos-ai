using backend.Modules.Payments.DTOs;

namespace backend.Modules.Payments.Interfaces;

public interface IFinancePaymentService
{
    Task<FinancePaymentDto> RecordPaymentAsync(Guid organizationId, CreateFinancePaymentDto dto);
    Task<IEnumerable<FinancePaymentDto>> GetPaymentsAsync(Guid organizationId, string? paymentType = null);
    Task<FinancePaymentDto?> GetPaymentByIdAsync(Guid id, Guid organizationId);
}
