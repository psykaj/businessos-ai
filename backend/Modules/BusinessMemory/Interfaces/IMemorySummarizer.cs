using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace backend.Modules.BusinessMemory.Interfaces;

public interface IMemorySummarizer
{
    Task<string> SummarizeContextAsync(
        Guid organizationId, 
        string rawData, 
        string contextType, 
        CancellationToken cancellationToken = default);
        
    Task<string?> ExtractStructuredDataAsync(
        Guid organizationId, 
        string rawData, 
        CancellationToken cancellationToken = default);
}
