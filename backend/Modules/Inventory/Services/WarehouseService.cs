using AutoMapper;
using backend.Modules.Inventory.DTOs;
using backend.Modules.Inventory.Entities;
using backend.Modules.Inventory.Interfaces;

namespace backend.Modules.Inventory.Services;

public class WarehouseService
{
    private readonly IWarehouseRepository _repository;
    private readonly IMapper _mapper;

    public WarehouseService(IWarehouseRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<WarehouseResponseDto> CreateWarehouseAsync(Guid organizationId, CreateWarehouseDto dto, CancellationToken cancellationToken = default)
    {
        if (dto.IsPrimary)
        {
            var existingPrimary = await _repository.GetPrimaryAsync(organizationId, cancellationToken);
            if (existingPrimary != null)
            {
                existingPrimary.IsPrimary = false;
                _repository.Update(existingPrimary);
            }
        }

        var warehouse = new Warehouse
        {
            OrganizationId = organizationId,
            Name = dto.Name,
            Code = dto.Code,
            Address = dto.Address,
            City = dto.City,
            State = dto.State,
            Country = dto.Country,
            PostalCode = dto.PostalCode,
            ManagerName = dto.ManagerName,
            ManagerPhone = dto.ManagerPhone,
            IsPrimary = dto.IsPrimary,
            IsActive = true
        };

        await _repository.AddAsync(warehouse, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return _mapper.Map<WarehouseResponseDto>(warehouse);
    }

    public async Task<WarehouseResponseDto> UpdateWarehouseAsync(Guid id, Guid organizationId, UpdateWarehouseDto dto, CancellationToken cancellationToken = default)
    {
        var warehouse = await _repository.GetByIdAsync(id, organizationId, cancellationToken);
        if (warehouse == null) throw new KeyNotFoundException($"Warehouse {id} not found.");

        if (dto.IsPrimary && !warehouse.IsPrimary)
        {
            var existingPrimary = await _repository.GetPrimaryAsync(organizationId, cancellationToken);
            if (existingPrimary != null)
            {
                existingPrimary.IsPrimary = false;
                _repository.Update(existingPrimary);
            }
        }

        warehouse.Name = dto.Name;
        warehouse.Code = dto.Code;
        warehouse.Address = dto.Address;
        warehouse.City = dto.City;
        warehouse.State = dto.State;
        warehouse.Country = dto.Country;
        warehouse.PostalCode = dto.PostalCode;
        warehouse.ManagerName = dto.ManagerName;
        warehouse.ManagerPhone = dto.ManagerPhone;
        warehouse.IsActive = dto.IsActive;
        warehouse.IsPrimary = dto.IsPrimary;

        _repository.Update(warehouse);
        await _repository.SaveChangesAsync(cancellationToken);

        return _mapper.Map<WarehouseResponseDto>(warehouse);
    }

    public async Task<IEnumerable<WarehouseResponseDto>> GetAllWarehousesAsync(Guid organizationId, CancellationToken cancellationToken = default)
    {
        var warehouses = await _repository.GetAllAsync(organizationId, cancellationToken);
        return _mapper.Map<IEnumerable<WarehouseResponseDto>>(warehouses);
    }

    public async Task<WarehouseResponseDto?> GetWarehouseByIdAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default)
    {
        var warehouse = await _repository.GetByIdAsync(id, organizationId, cancellationToken);
        return warehouse != null ? _mapper.Map<WarehouseResponseDto>(warehouse) : null;
    }
}
