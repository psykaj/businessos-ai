using AutoMapper;
using backend.Exceptions;
using backend.Modules.CRM.DTOs;
using backend.Modules.CRM.Entities;
using backend.Modules.CRM.Enums;
using backend.Modules.CRM.Interfaces;
using backend.Persistence;

namespace backend.Modules.CRM.Services;

public class CompanyService : ICompanyService
{
    private readonly ICompanyRepository _repository;
    private readonly ICrmActivityService _activityService;
    private readonly IMapper _mapper;
    private readonly ApplicationDbContext _context;

    public CompanyService(ICompanyRepository repository, ICrmActivityService activityService, IMapper mapper, ApplicationDbContext context)
    {
        _repository = repository;
        _activityService = activityService;
        _mapper = mapper;
        _context = context;
    }

    public async Task<CompanyResponseDto> CreateCompanyAsync(CreateCompanyDto dto, Guid organizationId)
    {
        var entity = _mapper.Map<Company>(dto);
        entity.OrganizationId = organizationId;
        
        await _repository.AddAsync(entity);
        await _context.SaveChangesAsync();
        
        await _activityService.LogActivityAsync(organizationId, "Company", entity.Id, ActivityType.SystemEvent, "Company was created");
        
        return _mapper.Map<CompanyResponseDto>(entity);
    }

    public async Task<CompanyResponseDto?> GetCompanyAsync(Guid id, Guid organizationId)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null || entity.OrganizationId != organizationId) return null;
        
        return _mapper.Map<CompanyResponseDto>(entity);
    }

    public async Task<IReadOnlyList<CompanyResponseDto>> GetAllCompaniesAsync(Guid organizationId)
    {
        var entities = await _repository.FindAsync(e => e.OrganizationId == organizationId);
        return _mapper.Map<IReadOnlyList<CompanyResponseDto>>(entities);
    }

    public async Task<CompanyResponseDto> UpdateCompanyAsync(Guid id, UpdateCompanyDto dto, Guid organizationId)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null || entity.OrganizationId != organizationId)
            throw new NotFoundException($"Company {id} not found");

        _mapper.Map(dto, entity);
        _repository.Update(entity);
        await _context.SaveChangesAsync();
        
        await _activityService.LogActivityAsync(organizationId, "Company", entity.Id, ActivityType.SystemEvent, "Company was updated");
        
        return _mapper.Map<CompanyResponseDto>(entity);
    }

    public async Task DeleteCompanyAsync(Guid id, Guid organizationId)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null || entity.OrganizationId != organizationId)
            throw new NotFoundException($"Company {id} not found");

        _repository.Delete(entity);
        await _context.SaveChangesAsync();
    }
}
