using AutoMapper;
using backend.Exceptions;
using backend.Modules.Branches.Interfaces;
using backend.Modules.Warehouses.DTOs;
using backend.Modules.Warehouses.Entities;
using backend.Modules.Warehouses.Interfaces;

namespace backend.Modules.Warehouses.Services;

public class WarehouseService : IWarehouseService
{
    private readonly IWarehouseRepository _repository;
    private readonly IBranchRepository _branchRepository;
    private readonly IMapper _mapper;

    public WarehouseService(IWarehouseRepository repository, IBranchRepository branchRepository, IMapper mapper)
    {
        _repository = repository;
        _branchRepository = branchRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<WarehouseResponseDto>> GetWarehousesAsync(Guid organizationId, Guid? branchId = null, CancellationToken cancellationToken = default)
    {
        var warehouses = await _repository.GetAllByOrgAsync(organizationId, branchId, cancellationToken);
        return _mapper.Map<IEnumerable<WarehouseResponseDto>>(warehouses);
    }

    public async Task<WarehouseResponseDto?> GetWarehouseByIdAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default)
    {
        var warehouse = await _repository.GetByIdAsync(id, organizationId, cancellationToken);
        if (warehouse == null)
        {
            throw new NotFoundException("Warehouse", id);
        }
        return _mapper.Map<WarehouseResponseDto>(warehouse);
    }

    public async Task<WarehouseResponseDto> CreateWarehouseAsync(Guid organizationId, CreateWarehouseDto dto, CancellationToken cancellationToken = default)
    {
        var branch = await _branchRepository.GetByIdAsync(dto.BranchId, organizationId, cancellationToken);
        if (branch == null)
        {
            throw new NotFoundException("Branch", dto.BranchId);
        }

        var entity = _mapper.Map<Warehouse>(dto);
        entity.OrganizationId = organizationId;

        await _repository.AddAsync(entity, cancellationToken);
        return _mapper.Map<WarehouseResponseDto>(entity);
    }

    public async Task<WarehouseResponseDto> UpdateWarehouseAsync(Guid id, Guid organizationId, UpdateWarehouseDto dto, CancellationToken cancellationToken = default)
    {
        var warehouse = await _repository.GetByIdAsync(id, organizationId, cancellationToken);
        if (warehouse == null)
        {
            throw new NotFoundException("Warehouse", id);
        }

        if (dto.BranchId != warehouse.BranchId)
        {
            var branch = await _branchRepository.GetByIdAsync(dto.BranchId, organizationId, cancellationToken);
            if (branch == null)
            {
                throw new NotFoundException("Branch", dto.BranchId);
            }
        }

        _mapper.Map(dto, warehouse);
        await _repository.UpdateAsync(warehouse, cancellationToken);
        return _mapper.Map<WarehouseResponseDto>(warehouse);
    }

    public async Task DeleteWarehouseAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default)
    {
        var warehouse = await _repository.GetByIdAsync(id, organizationId, cancellationToken);
        if (warehouse == null)
        {
            throw new NotFoundException("Warehouse", id);
        }
        await _repository.DeleteAsync(warehouse, cancellationToken);
    }
}
