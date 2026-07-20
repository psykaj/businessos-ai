using AutoMapper;
using backend.Exceptions;
using backend.Modules.CRM.DTOs;
using backend.Modules.CRM.Entities;
using backend.Modules.CRM.Enums;
using backend.Modules.CRM.Interfaces;
using backend.Persistence;

namespace backend.Modules.CRM.Services;

public class LeadService : ILeadService
{
    private readonly ILeadRepository _repository;
    private readonly ICrmActivityService _activityService;
    private readonly IMapper _mapper;
    private readonly ApplicationDbContext _context;

    public LeadService(ILeadRepository repository, ICrmActivityService activityService, IMapper mapper, ApplicationDbContext context)
    {
        _repository = repository;
        _activityService = activityService;
        _mapper = mapper;
        _context = context;
    }

    public async Task<LeadResponseDto> CreateLeadAsync(CreateLeadDto dto, Guid organizationId)
    {
        var entity = _mapper.Map<Lead>(dto);
        entity.OrganizationId = organizationId;
        
        await _repository.AddAsync(entity);
        await _context.SaveChangesAsync();
        
        await _activityService.LogActivityAsync(organizationId, "Lead", entity.Id, ActivityType.SystemEvent, "Lead was created");
        
        return _mapper.Map<LeadResponseDto>(entity);
    }

    public async Task<LeadResponseDto?> GetLeadAsync(Guid id, Guid organizationId)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null || entity.OrganizationId != organizationId) return null;
        
        return _mapper.Map<LeadResponseDto>(entity);
    }

    public async Task<IReadOnlyList<LeadResponseDto>> GetAllLeadsAsync(Guid organizationId)
    {
        var entities = await _repository.FindAsync(e => e.OrganizationId == organizationId);
        return _mapper.Map<IReadOnlyList<LeadResponseDto>>(entities);
    }

    public async Task<LeadResponseDto> UpdateLeadAsync(Guid id, UpdateLeadDto dto, Guid organizationId)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null || entity.OrganizationId != organizationId)
            throw new NotFoundException($"Lead {id} not found");

        _mapper.Map(dto, entity);
        _repository.Update(entity);
        await _context.SaveChangesAsync();
        
        await _activityService.LogActivityAsync(organizationId, "Lead", entity.Id, ActivityType.SystemEvent, "Lead was updated");
        
        return _mapper.Map<LeadResponseDto>(entity);
    }

    public async Task DeleteLeadAsync(Guid id, Guid organizationId)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null || entity.OrganizationId != organizationId)
            throw new NotFoundException($"Lead {id} not found");

        _repository.Delete(entity);
        await _context.SaveChangesAsync();
    }
}
