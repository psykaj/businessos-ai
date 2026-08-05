using AutoMapper;
using backend.Exceptions;
using backend.Modules.Locations.DTOs;
using backend.Modules.Locations.Entities;
using backend.Modules.Locations.Interfaces;

namespace backend.Modules.Locations.Services;

public class LocationService : ILocationService
{
    private readonly ILocationRepository _repository;
    private readonly IMapper _mapper;

    public LocationService(ILocationRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<LocationResponseDto>> GetLocationsAsync(Guid organizationId, string? region = null, CancellationToken cancellationToken = default)
    {
        var locations = await _repository.GetAllByOrgAsync(organizationId, region, cancellationToken);
        return _mapper.Map<IEnumerable<LocationResponseDto>>(locations);
    }

    public async Task<LocationResponseDto?> GetLocationByIdAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default)
    {
        var location = await _repository.GetByIdAsync(id, organizationId, cancellationToken);
        if (location == null)
        {
            throw new NotFoundException("Location", id);
        }
        return _mapper.Map<LocationResponseDto>(location);
    }

    public async Task<LocationResponseDto> CreateLocationAsync(Guid organizationId, CreateLocationDto dto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<Location>(dto);
        entity.OrganizationId = organizationId;

        await _repository.AddAsync(entity, cancellationToken);
        return _mapper.Map<LocationResponseDto>(entity);
    }

    public async Task<LocationResponseDto> UpdateLocationAsync(Guid id, Guid organizationId, UpdateLocationDto dto, CancellationToken cancellationToken = default)
    {
        var location = await _repository.GetByIdAsync(id, organizationId, cancellationToken);
        if (location == null)
        {
            throw new NotFoundException("Location", id);
        }

        _mapper.Map(dto, location);
        await _repository.UpdateAsync(location, cancellationToken);
        return _mapper.Map<LocationResponseDto>(location);
    }

    public async Task DeleteLocationAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default)
    {
        var location = await _repository.GetByIdAsync(id, organizationId, cancellationToken);
        if (location == null)
        {
            throw new NotFoundException("Location", id);
        }

        await _repository.DeleteAsync(location, cancellationToken);
    }
}
