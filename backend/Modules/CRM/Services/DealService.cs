using AutoMapper;
using backend.Exceptions;
using backend.Modules.CRM.DTOs;
using backend.Modules.CRM.Entities;
using backend.Modules.CRM.Enums;
using backend.Modules.CRM.Interfaces;
using backend.Persistence;

namespace backend.Modules.CRM.Services;

public class DealService : IDealService
{
    private readonly IDealRepository _repository;
    private readonly IDealStageHistoryRepository _historyRepository;
    private readonly ICrmActivityService _activityService;
    private readonly IMapper _mapper;
    private readonly ApplicationDbContext _context;

    public DealService(IDealRepository repository, IDealStageHistoryRepository historyRepository, ICrmActivityService activityService, IMapper mapper, ApplicationDbContext context)
    {
        _repository = repository;
        _historyRepository = historyRepository;
        _activityService = activityService;
        _mapper = mapper;
        _context = context;
    }

    public async Task<DealResponseDto> CreateDealAsync(CreateDealDto dto, Guid organizationId, Guid currentUserId)
    {
        var entity = _mapper.Map<Deal>(dto);
        entity.OrganizationId = organizationId;
        
        await _repository.AddAsync(entity);
        await _context.SaveChangesAsync();
        
        await _activityService.LogActivityAsync(organizationId, "Deal", entity.Id, ActivityType.SystemEvent, "Deal was created");
        
        return _mapper.Map<DealResponseDto>(entity);
    }

    public async Task<DealResponseDto?> GetDealAsync(Guid id, Guid organizationId)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null || entity.OrganizationId != organizationId) return null;
        
        return _mapper.Map<DealResponseDto>(entity);
    }

    public async Task<IReadOnlyList<DealResponseDto>> GetAllDealsAsync(Guid organizationId)
    {
        var entities = await _repository.FindAsync(e => e.OrganizationId == organizationId);
        return _mapper.Map<IReadOnlyList<DealResponseDto>>(entities);
    }

    public async Task<DealResponseDto> UpdateDealAsync(Guid id, UpdateDealDto dto, Guid organizationId)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null || entity.OrganizationId != organizationId)
            throw new NotFoundException($"Deal {id} not found");

        var oldStage = entity.PipelineStage;
        
        _mapper.Map(dto, entity);
        _repository.Update(entity);
        await _context.SaveChangesAsync();
        
        if (oldStage != entity.PipelineStage)
        {
            await _activityService.LogActivityAsync(organizationId, "Deal", entity.Id, ActivityType.SystemEvent, $"Stage changed from {oldStage} to {entity.PipelineStage}");
        }
        else
        {
            await _activityService.LogActivityAsync(organizationId, "Deal", entity.Id, ActivityType.SystemEvent, "Deal was updated");
        }
        
        return _mapper.Map<DealResponseDto>(entity);
    }

    public async Task DeleteDealAsync(Guid id, Guid organizationId)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null || entity.OrganizationId != organizationId)
            throw new NotFoundException($"Deal {id} not found");

        _repository.Delete(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<DealResponseDto> UpdateDealStageAsync(Guid id, PipelineStage stage, Guid organizationId, Guid currentUserId)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null || entity.OrganizationId != organizationId)
            throw new NotFoundException($"Deal {id} not found");

        var oldStage = entity.PipelineStage;
        if (oldStage == stage) return _mapper.Map<DealResponseDto>(entity); // No change

        entity.PipelineStage = stage;
        _repository.Update(entity);
        
        // Log history
        var history = new DealStageHistory
        {
            DealId = entity.Id,
            OldStage = oldStage,
            NewStage = stage,
            ChangedById = currentUserId
        };
        await _historyRepository.AddAsync(history);
        
        await _context.SaveChangesAsync();
        
        await _activityService.LogActivityAsync(organizationId, "Deal", entity.Id, ActivityType.SystemEvent, $"Stage changed from {oldStage} to {stage}");
        
        return _mapper.Map<DealResponseDto>(entity);
    }
}
