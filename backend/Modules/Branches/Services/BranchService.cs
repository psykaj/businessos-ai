using AutoMapper;
using backend.Exceptions;
using backend.Modules.Branches.DTOs;
using backend.Modules.Branches.Entities;
using backend.Modules.Branches.Interfaces;
using backend.Modules.Locations.Interfaces;

namespace backend.Modules.Branches.Services;

public class BranchService : IBranchService
{
    private readonly IBranchRepository _repository;
    private readonly ILocationRepository _locationRepository;
    private readonly IMapper _mapper;

    public BranchService(IBranchRepository repository, ILocationRepository locationRepository, IMapper mapper)
    {
        _repository = repository;
        _locationRepository = locationRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<BranchResponseDto>> GetBranchesAsync(Guid organizationId, Guid? locationId = null, BranchStatus? status = null, CancellationToken cancellationToken = default)
    {
        var branches = await _repository.GetAllByOrgAsync(organizationId, locationId, status, cancellationToken);
        return _mapper.Map<IEnumerable<BranchResponseDto>>(branches);
    }

    public async Task<BranchResponseDto?> GetBranchByIdAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default)
    {
        var branch = await _repository.GetByIdAsync(id, organizationId, cancellationToken);
        if (branch == null)
        {
            throw new NotFoundException("Branch", id);
        }
        return _mapper.Map<BranchResponseDto>(branch);
    }

    public async Task<BranchResponseDto> CreateBranchAsync(Guid organizationId, CreateBranchDto dto, CancellationToken cancellationToken = default)
    {
        if (await _repository.CodeExistsAsync(dto.Code, organizationId, null, cancellationToken))
        {
            throw new InvalidOperationException($"A branch with code '{dto.Code}' already exists in this organization.");
        }

        if (dto.LocationId.HasValue)
        {
            if (!await _locationRepository.ExistsAsync(dto.LocationId.Value, organizationId, cancellationToken))
            {
                throw new NotFoundException("Location", dto.LocationId.Value);
            }
        }

        var branch = _mapper.Map<Branch>(dto);
        branch.OrganizationId = organizationId;

        await _repository.AddAsync(branch, cancellationToken);
        return _mapper.Map<BranchResponseDto>(branch);
    }

    public async Task<BranchResponseDto> UpdateBranchAsync(Guid id, Guid organizationId, UpdateBranchDto dto, CancellationToken cancellationToken = default)
    {
        var branch = await _repository.GetByIdAsync(id, organizationId, cancellationToken);
        if (branch == null)
        {
            throw new NotFoundException("Branch", id);
        }

        if (await _repository.CodeExistsAsync(dto.Code, organizationId, id, cancellationToken))
        {
            throw new InvalidOperationException($"Another branch with code '{dto.Code}' already exists.");
        }

        if (dto.LocationId.HasValue && dto.LocationId != branch.LocationId)
        {
            if (!await _locationRepository.ExistsAsync(dto.LocationId.Value, organizationId, cancellationToken))
            {
                throw new NotFoundException("Location", dto.LocationId.Value);
            }
        }

        _mapper.Map(dto, branch);
        await _repository.UpdateAsync(branch, cancellationToken);
        return _mapper.Map<BranchResponseDto>(branch);
    }

    public async Task DeleteBranchAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default)
    {
        var branch = await _repository.GetByIdAsync(id, organizationId, cancellationToken);
        if (branch == null)
        {
            throw new NotFoundException("Branch", id);
        }

        await _repository.DeleteAsync(branch, cancellationToken);
    }

    public async Task<BranchResponseDto> UpdateBranchStatusAsync(Guid id, Guid organizationId, UpdateBranchStatusDto dto, CancellationToken cancellationToken = default)
    {
        var branch = await _repository.GetByIdAsync(id, organizationId, cancellationToken);
        if (branch == null)
        {
            throw new NotFoundException("Branch", id);
        }

        branch.Status = dto.Status;
        await _repository.UpdateAsync(branch, cancellationToken);
        return _mapper.Map<BranchResponseDto>(branch);
    }

    public async Task<BranchManagerResponseDto> AssignManagerAsync(Guid branchId, Guid organizationId, AssignManagerDto dto, CancellationToken cancellationToken = default)
    {
        var branch = await _repository.GetByIdAsync(branchId, organizationId, cancellationToken);
        if (branch == null)
        {
            throw new NotFoundException("Branch", branchId);
        }

        var existing = await _repository.GetManagerAsync(branchId, dto.UserId, organizationId, cancellationToken);
        if (existing != null)
        {
            existing.ManagerName = dto.ManagerName;
            existing.ManagerEmail = dto.ManagerEmail;
            existing.ManagerPhone = dto.ManagerPhone;
            existing.CanApproveTransfers = dto.CanApproveTransfers;
            existing.MaxTransferApprovalLimit = dto.MaxTransferApprovalLimit;
            existing.IsActive = true;
            await _repository.AddManagerAsync(existing, cancellationToken); // EF tracker will handle update if attached or we can save changes
            return _mapper.Map<BranchManagerResponseDto>(existing);
        }

        var manager = new BranchManager
        {
            OrganizationId = organizationId,
            BranchId = branchId,
            UserId = dto.UserId,
            ManagerName = dto.ManagerName,
            ManagerEmail = dto.ManagerEmail,
            ManagerPhone = dto.ManagerPhone,
            AssignedDate = DateTime.UtcNow,
            CanApproveTransfers = dto.CanApproveTransfers,
            MaxTransferApprovalLimit = dto.MaxTransferApprovalLimit,
            IsActive = true
        };

        await _repository.AddManagerAsync(manager, cancellationToken);
        return _mapper.Map<BranchManagerResponseDto>(manager);
    }

    public async Task<IEnumerable<BranchManagerResponseDto>> GetBranchManagersAsync(Guid branchId, Guid organizationId, CancellationToken cancellationToken = default)
    {
        var managers = await _repository.GetManagersByBranchAsync(branchId, organizationId, cancellationToken);
        return _mapper.Map<IEnumerable<BranchManagerResponseDto>>(managers);
    }

    public async Task RemoveManagerAsync(Guid branchId, Guid userId, Guid organizationId, CancellationToken cancellationToken = default)
    {
        var manager = await _repository.GetManagerAsync(branchId, userId, organizationId, cancellationToken);
        if (manager == null)
        {
            throw new NotFoundException("BranchManager", userId);
        }
        await _repository.RemoveManagerAsync(manager, cancellationToken);
    }
}
