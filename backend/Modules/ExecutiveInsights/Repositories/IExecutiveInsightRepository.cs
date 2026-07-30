using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Modules.ExecutiveInsights.Entities;

namespace backend.Modules.ExecutiveInsights.Repositories;

public interface IExecutiveInsightRepository
{
    Task<IEnumerable<ExecutiveInsight>> GetAllAsync(Guid organizationId);
    Task<IEnumerable<ExecutiveInsight>> GetUnreadAsync(Guid organizationId);
    Task<ExecutiveInsight?> GetByIdAsync(Guid id, Guid organizationId);
    Task<ExecutiveInsight> AddAsync(ExecutiveInsight insight);
    Task UpdateAsync(ExecutiveInsight insight);
    Task DeleteAsync(ExecutiveInsight insight);
}
