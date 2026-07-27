using AutoMapper;
using backend.Common;
using backend.Modules.Inventory.DTOs;
using backend.Modules.Inventory.Entities;
using backend.Modules.Inventory.Interfaces;

namespace backend.Modules.Inventory.Services;

public class SupplierService : ISupplierService
{
    private readonly ISupplierRepository _repository;
    private readonly IMapper _mapper;

    public SupplierService(ISupplierRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<SupplierResponseDto> CreateSupplierAsync(Guid organizationId, CreateSupplierDto dto, CancellationToken cancellationToken = default)
    {
        var existingCode = await _repository.GetByCodeAsync(dto.Code, organizationId, cancellationToken);
        if (existingCode != null) throw new InvalidOperationException($"Supplier code '{dto.Code}' already exists.");

        var supplier = new Supplier
        {
            OrganizationId = organizationId,
            Name = dto.Name.Trim(),
            Code = dto.Code.Trim().ToUpper(),
            ContactPerson = dto.ContactPerson.Trim(),
            Email = dto.Email.Trim(),
            Phone = dto.Phone.Trim(),
            Address = dto.Address,
            City = dto.City,
            Country = dto.Country,
            TaxId = dto.TaxId,
            PaymentTerms = dto.PaymentTerms,
            PerformanceScore = 100.0,
            TotalOrdersCount = 0,
            OnTimeDeliveriesCount = 0,
            IsActive = true
        };

        await _repository.AddAsync(supplier, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return _mapper.Map<SupplierResponseDto>(supplier);
    }

    public async Task<SupplierResponseDto> UpdateSupplierAsync(Guid id, Guid organizationId, UpdateSupplierDto dto, CancellationToken cancellationToken = default)
    {
        var supplier = await _repository.GetByIdAsync(id, organizationId, cancellationToken);
        if (supplier == null) throw new KeyNotFoundException($"Supplier {id} not found.");

        supplier.Name = dto.Name.Trim();
        supplier.Code = dto.Code.Trim().ToUpper();
        supplier.ContactPerson = dto.ContactPerson.Trim();
        supplier.Email = dto.Email.Trim();
        supplier.Phone = dto.Phone.Trim();
        supplier.Address = dto.Address;
        supplier.City = dto.City;
        supplier.Country = dto.Country;
        supplier.TaxId = dto.TaxId;
        supplier.PaymentTerms = dto.PaymentTerms;
        supplier.IsActive = dto.IsActive;

        _repository.Update(supplier);
        await _repository.SaveChangesAsync(cancellationToken);

        return _mapper.Map<SupplierResponseDto>(supplier);
    }

    public async Task<SupplierResponseDto?> GetSupplierByIdAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default)
    {
        var supplier = await _repository.GetByIdAsync(id, organizationId, cancellationToken);
        return supplier != null ? _mapper.Map<SupplierResponseDto>(supplier) : null;
    }

    public async Task<PagedResult<SupplierResponseDto>> GetSuppliersAsync(Guid organizationId, string? query, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        var pagedSuppliers = await _repository.GetPagedAsync(organizationId, query, pageNumber, pageSize, cancellationToken);
        return new PagedResult<SupplierResponseDto>(
            _mapper.Map<List<SupplierResponseDto>>(pagedSuppliers.Items),
            pagedSuppliers.TotalCount,
            pagedSuppliers.PageNumber,
            pagedSuppliers.PageSize
        );
    }

    public async Task<IEnumerable<SupplierResponseDto>> GetAllSuppliersAsync(Guid organizationId, CancellationToken cancellationToken = default)
    {
        var suppliers = await _repository.GetAllAsync(organizationId, cancellationToken);
        return _mapper.Map<IEnumerable<SupplierResponseDto>>(suppliers);
    }

    public async Task<SupplierPerformanceDto> GetSupplierPerformanceAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default)
    {
        var supplier = await _repository.GetByIdAsync(id, organizationId, cancellationToken);
        if (supplier == null) throw new KeyNotFoundException($"Supplier {id} not found.");

        double otdRate = supplier.TotalOrdersCount > 0 
            ? (double)supplier.OnTimeDeliveriesCount / supplier.TotalOrdersCount * 100.0 
            : 100.0;

        return new SupplierPerformanceDto(
            supplier.Id,
            supplier.Name,
            supplier.PerformanceScore,
            supplier.TotalOrdersCount,
            supplier.OnTimeDeliveriesCount,
            otdRate,
            100.0
        );
    }
}
