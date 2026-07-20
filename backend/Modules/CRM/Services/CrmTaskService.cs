using AutoMapper;
using backend.Exceptions;
using backend.Modules.CRM.DTOs;
using backend.Modules.CRM.Entities;
using backend.Modules.CRM.Enums;
using backend.Modules.CRM.Interfaces;
using backend.Persistence;

namespace backend.Modules.CRM.Services;

public class CrmTaskService : ICrmTaskService
{
    private readonly ICrmTaskRepository _repository;
    private readonly ICrmActivityService _activityService;
    private readonly IMapper _mapper;
    private readonly ApplicationDbContext _context;

    public CrmTaskService(ICrmTaskRepository repository, ICrmActivityService activityService, IMapper mapper, ApplicationDbContext context)
    {
        _repository = repository;
        _activityService = activityService;
        _mapper = mapper;
        _context = context;
    }

    public async Task<TaskResponseDto> CreateTaskAsync(CreateTaskDto dto, Guid organizationId)
    {
        var entity = _mapper.Map<CrmTask>(dto);
        entity.OrganizationId = organizationId;
        
        await _repository.AddAsync(entity);
        await _context.SaveChangesAsync();
        
        await _activityService.LogActivityAsync(organizationId, "Task", entity.Id, ActivityType.SystemEvent, "Task was created");
        
        return _mapper.Map<TaskResponseDto>(entity);
    }

    public async Task<TaskResponseDto?> GetTaskAsync(Guid id, Guid organizationId)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null || entity.OrganizationId != organizationId) return null;
        
        return _mapper.Map<TaskResponseDto>(entity);
    }

    public async Task<IReadOnlyList<TaskResponseDto>> GetAllTasksAsync(Guid organizationId)
    {
        var entities = await _repository.FindAsync(e => e.OrganizationId == organizationId);
        return _mapper.Map<IReadOnlyList<TaskResponseDto>>(entities);
    }

    public async Task<TaskResponseDto> UpdateTaskAsync(Guid id, UpdateTaskDto dto, Guid organizationId)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null || entity.OrganizationId != organizationId)
            throw new NotFoundException($"Task {id} not found");

        var oldStatus = entity.Status;

        _mapper.Map(dto, entity);
        _repository.Update(entity);
        await _context.SaveChangesAsync();
        
        if (oldStatus != entity.Status && entity.Status == CrmTaskStatus.Completed)
        {
            await _activityService.LogActivityAsync(organizationId, "Task", entity.Id, ActivityType.SystemEvent, "Task completed");
        }
        else
        {
            await _activityService.LogActivityAsync(organizationId, "Task", entity.Id, ActivityType.SystemEvent, "Task was updated");
        }
        
        return _mapper.Map<TaskResponseDto>(entity);
    }

    public async Task DeleteTaskAsync(Guid id, Guid organizationId)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null || entity.OrganizationId != organizationId)
            throw new NotFoundException($"Task {id} not found");

        _repository.Delete(entity);
        await _context.SaveChangesAsync();
    }
}
