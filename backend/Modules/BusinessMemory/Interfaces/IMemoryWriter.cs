using System;
using System.Threading;
using System.Threading.Tasks;
using backend.Modules.BusinessMemory.DTOs;
using backend.Modules.BusinessMemory.Entities;

namespace backend.Modules.BusinessMemory.Interfaces;

public interface IMemoryWriter
{
    Task<Entities.BusinessMemory> WriteMemoryAsync(
        Guid organizationId, 
        CreateMemoryDto request, 
        string userId, 
        CancellationToken cancellationToken = default);
        
    Task DeactivateContradictoryMemoriesAsync(
        Guid organizationId, 
        string sourceModule, 
        string sourceEntityId, 
        string newContext,
        string userId,
        CancellationToken cancellationToken = default);
}
