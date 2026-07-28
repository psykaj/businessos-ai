using backend.Modules.Taxes.Entities;

namespace backend.Modules.Taxes.Interfaces;

public interface ITaxRecordRepository
{
    Task<TaxRecord?> GetByIdAsync(Guid id, Guid organizationId);
    Task<TaxRecord?> GetByCodeAsync(string taxCode, Guid organizationId);
    Task<IEnumerable<TaxRecord>> GetAllAsync(Guid organizationId);
    Task<TaxRecord> AddAsync(TaxRecord tax);
    Task UpdateAsync(TaxRecord tax);
    Task DeleteAsync(TaxRecord tax);
}
