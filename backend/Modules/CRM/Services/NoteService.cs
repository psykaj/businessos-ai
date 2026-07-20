using AutoMapper;
using backend.Exceptions;
using backend.Modules.CRM.DTOs;
using backend.Modules.CRM.Entities;
using backend.Modules.CRM.Enums;
using backend.Modules.CRM.Interfaces;
using backend.Persistence;

namespace backend.Modules.CRM.Services;

public class NoteService : INoteService
{
    private readonly INoteRepository _repository;
    private readonly ICrmActivityService _activityService;
    private readonly IMapper _mapper;
    private readonly ApplicationDbContext _context;

    public NoteService(INoteRepository repository, ICrmActivityService activityService, IMapper mapper, ApplicationDbContext context)
    {
        _repository = repository;
        _activityService = activityService;
        _mapper = mapper;
        _context = context;
    }

    public async Task<NoteResponseDto> CreateNoteAsync(CreateNoteDto dto, Guid organizationId, Guid currentUserId)
    {
        var entity = _mapper.Map<Note>(dto);
        entity.OrganizationId = organizationId;
        entity.CreatedByUserId = currentUserId;
        
        await _repository.AddAsync(entity);
        await _context.SaveChangesAsync();
        
        await _activityService.LogActivityAsync(organizationId, entity.RelatedEntity, entity.RelatedEntityId, ActivityType.Note, "Note added");
        
        return _mapper.Map<NoteResponseDto>(entity);
    }

    public async Task<NoteResponseDto?> GetNoteAsync(Guid id, Guid organizationId)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null || entity.OrganizationId != organizationId) return null;
        
        return _mapper.Map<NoteResponseDto>(entity);
    }

    public async Task<IReadOnlyList<NoteResponseDto>> GetAllNotesAsync(Guid organizationId)
    {
        var entities = await _repository.FindAsync(e => e.OrganizationId == organizationId);
        return _mapper.Map<IReadOnlyList<NoteResponseDto>>(entities);
    }

    public async Task<NoteResponseDto> UpdateNoteAsync(Guid id, UpdateNoteDto dto, Guid organizationId)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null || entity.OrganizationId != organizationId)
            throw new NotFoundException($"Note {id} not found");

        _mapper.Map(dto, entity);
        _repository.Update(entity);
        await _context.SaveChangesAsync();
        
        return _mapper.Map<NoteResponseDto>(entity);
    }

    public async Task DeleteNoteAsync(Guid id, Guid organizationId)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null || entity.OrganizationId != organizationId)
            throw new NotFoundException($"Note {id} not found");

        _repository.Delete(entity);
        await _context.SaveChangesAsync();
    }
}
