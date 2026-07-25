using backend.Modules.CustomerSuccess.CustomerSegments.Entities;

namespace backend.Modules.CustomerSuccess.CustomerSegments.Interfaces;

public interface ICustomerSegmentRepository
{
    Task<CustomerSegment?> GetByIdAsync(Guid orgId, Guid id);
    Task<CustomerSegment?> GetByNameAsync(Guid orgId, string name);
    Task<IEnumerable<CustomerSegment>> GetAllAsync(Guid orgId);
    Task AddAsync(CustomerSegment segment);
    Task UpdateAsync(CustomerSegment segment);
    Task SaveChangesAsync();
}
