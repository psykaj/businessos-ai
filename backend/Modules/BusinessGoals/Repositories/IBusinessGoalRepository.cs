using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Modules.BusinessGoals.Entities;

namespace backend.Modules.BusinessGoals.Repositories;

public interface IBusinessGoalRepository
{
    Task<IEnumerable<BusinessGoal>> GetAllAsync(Guid organizationId);
    Task<BusinessGoal?> GetByIdAsync(Guid id, Guid organizationId);
    Task<BusinessGoal> AddAsync(BusinessGoal goal);
    Task UpdateAsync(BusinessGoal goal);
    Task DeleteAsync(BusinessGoal goal);
}
