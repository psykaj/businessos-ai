using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Modules.BusinessHealth.Entities;

namespace backend.Modules.BusinessHealth.Repositories;

public interface IBusinessHealthRepository
{
    Task<IEnumerable<BusinessHealthScore>> GetHistoryAsync(Guid organizationId, int limit);
    Task<BusinessHealthScore?> GetLatestAsync(Guid organizationId);
    Task<BusinessHealthScore> AddAsync(BusinessHealthScore score);
}
