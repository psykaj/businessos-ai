using backend.Modules.CustomerSuccess.CustomerHealth.Interfaces;
using backend.Modules.CustomerSuccess.SuccessTasks.DTOs;
using backend.Modules.CustomerSuccess.SuccessTasks.Entities;
using backend.Modules.CustomerSuccess.SuccessTasks.Interfaces;

namespace backend.Modules.CustomerSuccess.SuccessTasks.Services;

public class SuccessTaskService : ISuccessTaskService
{
    private readonly ISuccessTaskRepository _taskRepo;
    private readonly ICustomerHealthRepository _healthRepo;

    public SuccessTaskService(
        ISuccessTaskRepository taskRepo,
        ICustomerHealthRepository healthRepo)
    {
        _taskRepo = taskRepo;
        _healthRepo = healthRepo;
    }

    public async Task<SuccessTaskDto> GetByIdAsync(Guid orgId, Guid id)
    {
        var task = await _taskRepo.GetByIdAsync(orgId, id)
            ?? throw new KeyNotFoundException("Success task not found.");

        return MapToDto(task);
    }

    public async Task<SuccessTaskDto> CreateTaskAsync(Guid orgId, CreateSuccessTaskDto dto)
    {
        var task = new SuccessTask
        {
            OrganizationId = orgId,
            CustomerId = dto.CustomerId,
            AssignedUserId = dto.AssignedUserId,
            TaskType = dto.TaskType,
            Title = dto.Title,
            Description = dto.Description,
            DueDate = dto.DueDate ?? DateTime.UtcNow.AddDays(3),
            Priority = dto.Priority,
            Status = "Pending"
        };

        await _taskRepo.AddAsync(task);
        await _taskRepo.SaveChangesAsync();

        return MapToDto(task);
    }

    public async Task<SuccessTaskDto> UpdateTaskAsync(Guid orgId, Guid id, UpdateSuccessTaskDto dto)
    {
        var task = await _taskRepo.GetByIdAsync(orgId, id)
            ?? throw new KeyNotFoundException("Success task not found.");

        task.AssignedUserId = dto.AssignedUserId;
        task.Title = dto.Title;
        task.Description = dto.Description;
        task.DueDate = dto.DueDate;
        task.Priority = dto.Priority;
        task.Status = dto.Status;

        await _taskRepo.UpdateAsync(task);
        await _taskRepo.SaveChangesAsync();

        return MapToDto(task);
    }

    public async Task<bool> DeleteTaskAsync(Guid orgId, Guid id)
    {
        var task = await _taskRepo.GetByIdAsync(orgId, id);
        if (task == null) return false;

        await _taskRepo.DeleteAsync(task);
        await _taskRepo.SaveChangesAsync();
        return true;
    }

    public async Task<(IEnumerable<SuccessTaskDto> Items, int TotalCount)> GetPagedAsync(
        Guid orgId, Guid? customerId, Guid? assignedUserId, string? status, string? priority, string? taskType, int page, int pageSize)
    {
        var (items, totalCount) = await _taskRepo.GetPagedAsync(orgId, customerId, assignedUserId, status, priority, taskType, page, pageSize);
        return (items.Select(MapToDto), totalCount);
    }

    public async Task AutoGenerateRetentionTasksAsync(Guid orgId)
    {
        var (healthRecords, _) = await _healthRepo.GetPagedAsync(orgId, null, null, 1, 1000, null, false);
        
        foreach (var health in healthRecords)
        {
            // 1. Inactive Customer Task (> 30 days without interaction)
            if (health.LastInteractionDate.HasValue && (DateTime.UtcNow - health.LastInteractionDate.Value).TotalDays > 30)
            {
                var existing = await _taskRepo.GetPagedAsync(orgId, health.CustomerId, null, "Pending", null, "FollowUpInactive", 1, 1);
                if (!existing.Items.Any())
                {
                    await _taskRepo.AddAsync(new SuccessTask
                    {
                        OrganizationId = orgId,
                        CustomerId = health.CustomerId,
                        TaskType = "FollowUpInactive",
                        Title = $"Follow up with inactive customer ({health.Customer?.Name})",
                        Description = $"Customer has not interacted in over {Math.Round((DateTime.UtcNow - health.LastInteractionDate.Value).TotalDays)} days.",
                        Priority = "Medium",
                        Status = "Pending",
                        DueDate = DateTime.UtcNow.AddDays(2)
                    });
                }
            }

            // 2. High Value Upsell (> $1000 LTV)
            if (health.LifetimeValue >= 1000)
            {
                var existing = await _taskRepo.GetPagedAsync(orgId, health.CustomerId, null, "Pending", null, "UpsellHighValue", 1, 1);
                if (!existing.Items.Any())
                {
                    await _taskRepo.AddAsync(new SuccessTask
                    {
                        OrganizationId = orgId,
                        CustomerId = health.CustomerId,
                        TaskType = "UpsellHighValue",
                        Title = $"VIP Upsell Opportunity ({health.Customer?.Name})",
                        Description = $"High LTV Customer (${health.LifetimeValue:N2}). Offer premium upgrade or exclusive enterprise deal.",
                        Priority = "High",
                        Status = "Pending",
                        DueDate = DateTime.UtcNow.AddDays(5)
                    });
                }
            }
        }

        await _taskRepo.SaveChangesAsync();
    }

    private static SuccessTaskDto MapToDto(SuccessTask t)
    {
        return new SuccessTaskDto(
            t.Id,
            t.CustomerId,
            t.Customer?.Name ?? "Customer",
            t.AssignedUserId,
            t.AssignedUser?.FullName,
            t.TaskType,
            t.Title,
            t.Description,
            t.DueDate,
            t.Priority,
            t.Status,
            t.CreatedAt
        );
    }
}
