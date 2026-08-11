using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using backend.Modules.BusinessMemory.DTOs;
using backend.Modules.BusinessMemory.Enums;
using backend.Modules.BusinessMemory.Entities;

namespace backend.Modules.BusinessMemory.Interfaces;

public interface IMemoryService
{
    Task<MemoryDto> GetMemoryAsync(Guid organizationId, Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<MemoryDto>> GetCustomerMemoriesAsync(Guid organizationId, string customerId, CancellationToken cancellationToken = default);
    Task<IEnumerable<MemoryDto>> GetBusinessMemoriesAsync(Guid organizationId, CancellationToken cancellationToken = default);
    Task<MemoryDto> CreateMemoryAsync(Guid organizationId, CreateMemoryDto request, string userId, CancellationToken cancellationToken = default);
    Task<MemoryDto> UpdateMemoryAsync(Guid organizationId, Guid id, UpdateMemoryDto request, string userId, CancellationToken cancellationToken = default);
    Task DeleteMemoryAsync(Guid organizationId, Guid id, string userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<MemoryDto>> GetRelevantMemoriesAsync(Guid organizationId, string query, CancellationToken cancellationToken = default);
    Task RebuildContextAsync(Guid organizationId, string entityType, string entityId, CancellationToken cancellationToken = default);
}
