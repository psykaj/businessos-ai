using backend.Modules.Locations.Entities;
using backend.Common;

namespace backend.Modules.Locations.Interfaces;

public interface ILocationRepository
{
    Task<Location?> GetByIdAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Location>> GetAllByOrgAsync(Guid organizationId, string? region = null, CancellationToken cancellationToken = default);
    Task<Location> AddAsync(Location location, CancellationToken cancellationToken = default);
    Task UpdateAsync(Location location, CancellationToken cancellationToken = default);
    Task DeleteAsync(Location location, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default);
}

public interface ILocationService
{
    Task<IEnumerable<DTOs.LocationResponseDto>> GetLocationsAsync(Guid organizationId, string? region = null, CancellationToken cancellationToken = default);
    Task<DTOs.LocationResponseDto?> GetLocationByIdAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default);
    Task<DTOs.LocationResponseDto> CreateLocationAsync(Guid organizationId, DTOs.CreateLocationDto dto, CancellationToken cancellationToken = default);
    Task<DTOs.LocationResponseDto> UpdateLocationAsync(Guid id, Guid organizationId, DTOs.UpdateLocationDto dto, CancellationToken cancellationToken = default);
    Task DeleteLocationAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default);
}
