using AutoMapper;
using backend.Modules.Inventory.DTOs;
using backend.Modules.Inventory.Entities;
using backend.Modules.Inventory.Interfaces;

namespace backend.Modules.Inventory.Services;

public class CategoryService
{
    private readonly ICategoryRepository _repository;
    private readonly IMapper _mapper;

    public CategoryService(ICategoryRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<CategoryResponseDto> CreateCategoryAsync(Guid organizationId, CreateCategoryDto dto, CancellationToken cancellationToken = default)
    {
        var category = new ProductCategory
        {
            OrganizationId = organizationId,
            Name = dto.Name,
            Code = dto.Code,
            Description = dto.Description,
            ParentCategoryId = dto.ParentCategoryId
        };

        await _repository.AddAsync(category, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        var created = await _repository.GetByIdAsync(category.Id, organizationId, cancellationToken);
        return _mapper.Map<CategoryResponseDto>(created!);
    }

    public async Task<CategoryResponseDto> UpdateCategoryAsync(Guid id, Guid organizationId, UpdateCategoryDto dto, CancellationToken cancellationToken = default)
    {
        var category = await _repository.GetByIdAsync(id, organizationId, cancellationToken);
        if (category == null) throw new KeyNotFoundException($"Category {id} not found.");

        category.Name = dto.Name;
        category.Code = dto.Code;
        category.Description = dto.Description;
        category.ParentCategoryId = dto.ParentCategoryId;

        _repository.Update(category);
        await _repository.SaveChangesAsync(cancellationToken);

        return _mapper.Map<CategoryResponseDto>(category);
    }

    public async Task<IEnumerable<CategoryResponseDto>> GetAllCategoriesAsync(Guid organizationId, CancellationToken cancellationToken = default)
    {
        var categories = await _repository.GetAllAsync(organizationId, cancellationToken);
        return _mapper.Map<IEnumerable<CategoryResponseDto>>(categories);
    }

    public async Task<CategoryResponseDto?> GetCategoryByIdAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default)
    {
        var category = await _repository.GetByIdAsync(id, organizationId, cancellationToken);
        return category != null ? _mapper.Map<CategoryResponseDto>(category) : null;
    }
}
