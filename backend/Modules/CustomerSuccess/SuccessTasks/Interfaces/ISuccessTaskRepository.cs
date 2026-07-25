using backend.Modules.CustomerSuccess.SuccessTasks.Entities;

namespace backend.Modules.CustomerSuccess.SuccessTasks.Interfaces;

public interface ISuccessTaskRepository
{
    Task<SuccessTask?> GetByIdAsync(Guid orgId, Guid id);
    Task<(IEnumerable<SuccessTask> Items, int TotalCount)> GetPagedAsync(
        Guid orgId, Guid? customerId, Guid? assignedUserId, string? status, string? priority, string? taskType, int page, int pageSize);
    Task AddAsync(SuccessTask task);
    Task UpdateAsync(SuccessTask task);
    Task DeleteAsync(SuccessTask task);
    Task SaveChangesAsync();
}
