using backend.Modules.CashFlow.Entities;
using backend.Modules.Payments.DTOs;
using backend.Modules.Payments.Entities;
using backend.Modules.Payments.Interfaces;
using backend.Persistence;

namespace backend.Modules.Payments.Services;

public class FinancePaymentService : IFinancePaymentService
{
    private readonly IFinancePaymentRepository _repository;
    private readonly ApplicationDbContext _context;

    public FinancePaymentService(IFinancePaymentRepository repository, ApplicationDbContext context)
    {
        _repository = repository;
        _context = context;
    }

    public async Task<FinancePaymentDto> RecordPaymentAsync(Guid organizationId, CreateFinancePaymentDto dto)
    {
        var payment = new FinancePayment
        {
            OrganizationId = organizationId,
            PaymentNumber = string.IsNullOrWhiteSpace(dto.PaymentNumber)
                ? $"PAY-{DateTime.UtcNow:yyyyMMdd}-{Random.Shared.Next(1000, 9999)}"
                : dto.PaymentNumber,
            InvoiceId = dto.InvoiceId,
            BillId = dto.BillId,
            AccountId = dto.AccountId,
            Amount = dto.Amount,
            PaymentDate = dto.PaymentDate,
            PaymentMethod = dto.PaymentMethod,
            PaymentType = dto.PaymentType,
            ReferenceNumber = dto.ReferenceNumber,
            Notes = dto.Notes,
            Status = "Completed"
        };

        var created = await _repository.AddAsync(payment);

        // Auto-create CashFlowEntry
        var cashFlowType = dto.PaymentType == "Incoming" ? "CashIn" : "CashOut";
        var category = dto.PaymentType == "Incoming" ? "ARCollection" : "APPayment";

        var cashFlow = new CashFlowEntry
        {
            OrganizationId = organizationId,
            EntryDate = dto.PaymentDate,
            Type = cashFlowType,
            Category = category,
            Amount = dto.Amount,
            AccountId = dto.AccountId,
            ReferenceType = "Payment",
            ReferenceId = created.Id,
            Description = $"Payment #{created.PaymentNumber} ({dto.PaymentMethod})"
        };

        await _context.CashFlowEntries.AddAsync(cashFlow);
        await _context.SaveChangesAsync();

        return MapToDto(created);
    }

    public async Task<IEnumerable<FinancePaymentDto>> GetPaymentsAsync(Guid organizationId, string? paymentType = null)
    {
        var payments = await _repository.GetAllAsync(organizationId, paymentType);
        return payments.Select(MapToDto);
    }

    public async Task<FinancePaymentDto?> GetPaymentByIdAsync(Guid id, Guid organizationId)
    {
        var payment = await _repository.GetByIdAsync(id, organizationId);
        return payment != null ? MapToDto(payment) : null;
    }

    private static FinancePaymentDto MapToDto(FinancePayment p) => new(
        p.Id,
        p.OrganizationId,
        p.PaymentNumber,
        p.InvoiceId,
        p.BillId,
        p.AccountId,
        p.Amount,
        p.PaymentDate,
        p.PaymentMethod,
        p.PaymentType,
        p.ReferenceNumber,
        p.Notes,
        p.Status,
        p.CreatedAt
    );
}
