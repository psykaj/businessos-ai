using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Modules.Scorecards.Entities;

namespace backend.Modules.Scorecards.Repositories;

public interface IScorecardRepository
{
    Task<IEnumerable<Scorecard>> GetAllAsync(Guid organizationId);
    Task<Scorecard?> GetByIdAsync(Guid id, Guid organizationId);
    Task<Scorecard> AddAsync(Scorecard scorecard);
    Task UpdateAsync(Scorecard scorecard);
    Task DeleteAsync(Scorecard scorecard);
}
