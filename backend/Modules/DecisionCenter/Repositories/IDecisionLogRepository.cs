using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Modules.DecisionCenter.Entities;

namespace backend.Modules.DecisionCenter.Repositories;

public interface IDecisionLogRepository
{
    Task<IEnumerable<DecisionLog>> GetAllAsync(Guid organizationId);
    Task<DecisionLog?> GetByIdAsync(Guid id, Guid organizationId);
    Task<DecisionLog> AddAsync(DecisionLog log);
    Task UpdateAsync(DecisionLog log);
}
