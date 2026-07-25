using backend.Modules.CustomerSuccess.CustomerHealth.Entities;

namespace backend.Modules.CustomerSuccess.CustomerHealth.Interfaces;

public interface ICustomerHealthRepository
{
    Task<Entities.CustomerHealth?> GetByCustomerIdAsync(Guid orgId, Guid customerId);
    Task<Entities.CustomerHealth?> GetByIdAsync(Guid orgId, Guid id);
    Task<(IEnumerable<Entities.CustomerHealth> Items, int TotalCount)> GetPagedAsync(
        Guid orgId, string? riskLevel, string? search, int page, int pageSize, string? sortBy, bool descending);
    Task<IEnumerable<Entities.CustomerHealth>> GetHighRiskCustomersAsync(Guid orgId);
    Task AddAsync(Entities.CustomerHealth health);
    Task UpdateAsync(Entities.CustomerHealth health);
    Task SaveChangesAsync();
}
