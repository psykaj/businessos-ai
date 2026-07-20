using AutoMapper;
using backend.Exceptions;
using backend.Modules.CRM.DTOs;
using backend.Modules.CRM.Entities;
using backend.Modules.CRM.Enums;
using backend.Modules.CRM.Interfaces;
using backend.Persistence;
using backend.Entities; // Needed for backend.Entities.Contact

namespace backend.Modules.CRM.Services;

public class ContactService : IContactService
{
    private readonly IContactRepository _repository;
    private readonly ICrmActivityService _activityService;
    private readonly IMapper _mapper;
    private readonly ApplicationDbContext _context;

    public ContactService(IContactRepository repository, ICrmActivityService activityService, IMapper mapper, ApplicationDbContext context)
    {
        _repository = repository;
        _activityService = activityService;
        _mapper = mapper;
        _context = context;
    }

    public async Task<ContactResponseDto> CreateContactAsync(CreateContactDto dto, Guid organizationId)
    {
        var entity = _mapper.Map<Contact>(dto);
        entity.OrganizationId = organizationId;
        
        await _repository.AddAsync(entity);
        await _context.SaveChangesAsync();
        
        await _activityService.LogActivityAsync(organizationId, "Contact", entity.Id, ActivityType.SystemEvent, "Contact was created");
        
        return _mapper.Map<ContactResponseDto>(entity);
    }

    public async Task<ContactResponseDto?> GetContactAsync(Guid id, Guid organizationId)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null || entity.OrganizationId != organizationId) return null;
        
        return _mapper.Map<ContactResponseDto>(entity);
    }

    public async Task<IReadOnlyList<ContactResponseDto>> GetAllContactsAsync(Guid organizationId)
    {
        var entities = await _repository.FindAsync(e => e.OrganizationId == organizationId);
        return _mapper.Map<IReadOnlyList<ContactResponseDto>>(entities);
    }

    public async Task<ContactResponseDto> UpdateContactAsync(Guid id, UpdateContactDto dto, Guid organizationId)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null || entity.OrganizationId != organizationId)
            throw new NotFoundException($"Contact {id} not found");

        _mapper.Map(dto, entity);
        _repository.Update(entity);
        await _context.SaveChangesAsync();
        
        await _activityService.LogActivityAsync(organizationId, "Contact", entity.Id, ActivityType.SystemEvent, "Contact was updated");
        
        return _mapper.Map<ContactResponseDto>(entity);
    }

    public async Task DeleteContactAsync(Guid id, Guid organizationId)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null || entity.OrganizationId != organizationId)
            throw new NotFoundException($"Contact {id} not found");

        _repository.Delete(entity);
        await _context.SaveChangesAsync();
    }
}
