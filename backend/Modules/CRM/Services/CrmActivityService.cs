using AutoMapper;
using backend.Exceptions;
using backend.Modules.CRM.DTOs;
using backend.Modules.CRM.Entities;
using backend.Modules.CRM.Enums;
using backend.Modules.CRM.Interfaces;
using backend.Persistence;

namespace backend.Modules.CRM.Services;

public class CrmActivityService : ICrmActivityService
{
    private readonly ICrmActivityRepository _repository;
    private readonly IMapper _mapper;
    private readonly ApplicationDbContext _context;

    public CrmActivityService(ICrmActivityRepository repository, IMapper mapper, ApplicationDbContext context)
    {
        _repository = repository;
        _mapper = mapper;
        _context = context;
    }

    public async Task<ActivityResponseDto> CreateActivityAsync(CreateActivityDto dto, Guid organizationId)
    {
        var entity = _mapper.Map<CrmActivity>(dto);
        entity.OrganizationId = organizationId;
        
        await _repository.AddAsync(entity);
        await _context.SaveChangesAsync();
        
        return _mapper.Map<ActivityResponseDto>(entity);
    }

    public async Task<ActivityResponseDto?> GetActivityAsync(Guid id, Guid organizationId)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null || entity.OrganizationId != organizationId) return null;
        
        return _mapper.Map<ActivityResponseDto>(entity);
    }

    public async Task<IReadOnlyList<ActivityResponseDto>> GetAllActivitiesAsync(Guid organizationId)
    {
        var entities = await _repository.FindAsync(e => e.OrganizationId == organizationId);
        return _mapper.Map<IReadOnlyList<ActivityResponseDto>>(entities);
    }

    public async Task<ActivityResponseDto> UpdateActivityAsync(Guid id, UpdateActivityDto dto, Guid organizationId)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null || entity.OrganizationId != organizationId)
            throw new NotFoundException($"Activity {id} not found");

        _mapper.Map(dto, entity);
        _repository.Update(entity);
        await _context.SaveChangesAsync();
        
        return _mapper.Map<ActivityResponseDto>(entity);
    }

    public async Task DeleteActivityAsync(Guid id, Guid organizationId)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null || entity.OrganizationId != organizationId)
            throw new NotFoundException($"Activity {id} not found");

        _repository.Delete(entity);
        await _context.SaveChangesAsync();
    }

    public async Task LogActivityAsync(Guid organizationId, string relatedEntity, Guid relatedEntityId, ActivityType type, string description)
    {
        var activity = new CrmActivity
        {
            OrganizationId = organizationId,
            RelatedEntity = relatedEntity,
            RelatedEntityId = relatedEntityId,
            Type = type,
            Description = description,
            Status = "Completed",
            DueDate = DateTime.UtcNow
        };

        await _repository.AddAsync(activity);
        await _context.SaveChangesAsync();
    }
}
