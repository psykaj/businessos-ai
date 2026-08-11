using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using backend.Modules.BusinessMemory.Entities;
using backend.Modules.BusinessMemory.Enums;

namespace backend.Modules.BusinessMemory.Interfaces;

public interface IMemoryRetrievalService
{
    Task<IEnumerable<Entities.BusinessMemory>> RetrieveRelevantMemoriesAsync(
        Guid organizationId, 
        string question, 
        string? entityType = null, 
        string? entityId = null, 
        int limit = 10, 
        CancellationToken cancellationToken = default);
        
    Task<IEnumerable<Entities.BusinessMemory>> GetByEntityAsync(
        Guid organizationId, 
        string sourceModule, 
        string sourceEntityId, 
        CancellationToken cancellationToken = default);
}
