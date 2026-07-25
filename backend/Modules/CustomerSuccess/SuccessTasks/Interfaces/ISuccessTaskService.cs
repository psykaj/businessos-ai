using backend.Modules.CustomerSuccess.SuccessTasks.DTOs;

namespace backend.Modules.CustomerSuccess.SuccessTasks.Interfaces;

public interface ISuccessTaskService
{
    Task<SuccessTaskDto> GetByIdAsync(Guid orgId, Guid id);
    Task<SuccessTaskDto> CreateTaskAsync(Guid orgId, CreateSuccessTaskDto dto);
    Task<SuccessTaskDto> UpdateTaskAsync(Guid orgId, Guid id, UpdateSuccessTaskDto dto);
    Task<bool> DeleteTaskAsync(Guid orgId, Guid id);
    Task<(IEnumerable<SuccessTaskDto> Items, int TotalCount)> GetPagedAsync(
        Guid orgId, Guid? customerId, Guid? assignedUserId, string? status, string? priority, string? taskType, int page, int pageSize);
    Task AutoGenerateRetentionTasksAsync(Guid orgId);
}
