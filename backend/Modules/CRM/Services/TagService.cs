using AutoMapper;
using backend.Exceptions;
using backend.Modules.CRM.DTOs;
using backend.Modules.CRM.Entities;
using backend.Modules.CRM.Interfaces;
using backend.Persistence;

namespace backend.Modules.CRM.Services;

public class TagService : ITagService
{
    private readonly ITagRepository _repository;
    private readonly IMapper _mapper;
    private readonly ApplicationDbContext _context;

    public TagService(ITagRepository repository, IMapper mapper, ApplicationDbContext context)
    {
        _repository = repository;
        _mapper = mapper;
        _context = context;
    }

    public async Task<TagResponseDto> CreateTagAsync(CreateTagDto dto, Guid organizationId)
    {
        var entity = _mapper.Map<Tag>(dto);
        entity.OrganizationId = organizationId;
        
        await _repository.AddAsync(entity);
        await _context.SaveChangesAsync();
        
        return _mapper.Map<TagResponseDto>(entity);
    }

    public async Task<TagResponseDto?> GetTagAsync(Guid id, Guid organizationId)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null || entity.OrganizationId != organizationId) return null;
        
        return _mapper.Map<TagResponseDto>(entity);
    }

    public async Task<IReadOnlyList<TagResponseDto>> GetAllTagsAsync(Guid organizationId)
    {
        var entities = await _repository.FindAsync(e => e.OrganizationId == organizationId);
        return _mapper.Map<IReadOnlyList<TagResponseDto>>(entities);
    }

    public async Task<TagResponseDto> UpdateTagAsync(Guid id, UpdateTagDto dto, Guid organizationId)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null || entity.OrganizationId != organizationId)
            throw new NotFoundException($"Tag {id} not found");

        _mapper.Map(dto, entity);
        _repository.Update(entity);
        await _context.SaveChangesAsync();
        
        return _mapper.Map<TagResponseDto>(entity);
    }

    public async Task DeleteTagAsync(Guid id, Guid organizationId)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null || entity.OrganizationId != organizationId)
            throw new NotFoundException($"Tag {id} not found");

        _repository.Delete(entity);
        await _context.SaveChangesAsync();
    }
}
