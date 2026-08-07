using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Modules.ActionCenter.Entities;

namespace backend.Modules.ActionCenter.Repositories;

public interface IAiActionRepository
{
    Task<AiAction?> GetByIdAsync(Guid id, Guid organizationId);
    Task<IEnumerable<AiAction>> GetAllAsync(Guid organizationId);
    Task<IEnumerable<AiAction>> GetPendingAsync(Guid organizationId);
    Task<IEnumerable<AiAction>> GetHistoryAsync(Guid organizationId);
    Task<AiAction> AddAsync(AiAction action);
    Task UpdateAsync(AiAction action);
}
