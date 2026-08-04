using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Common;
using backend.Modules.Profitability.Entities;

namespace backend.Modules.Profitability.Interfaces;

public interface IProfitabilityRepository
{
    Task<ProfitSnapshot?> GetByIdAsync(Guid id, Guid organizationId);
    Task<PagedResult<ProfitSnapshot>> GetSnapshotsAsync(Guid organizationId, string? period, int pageNumber, int pageSize);
    Task<List<ProfitSnapshot>> GetRecentSnapshotsAsync(Guid organizationId, int limit = 12);
    Task<ProfitSnapshot> CreateAsync(ProfitSnapshot snapshot);
    Task<bool> DeleteAsync(Guid id, Guid organizationId);
}
