using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Common;
using backend.Modules.RevenueAnalytics.Entities;

namespace backend.Modules.RevenueAnalytics.Interfaces;

public interface IRevenueAnalyticsRepository
{
    Task<RevenueSnapshot?> GetByIdAsync(Guid id, Guid organizationId);
    Task<PagedResult<RevenueSnapshot>> GetSnapshotsAsync(Guid organizationId, string? periodType, int pageNumber, int pageSize);
    Task<List<RevenueSnapshot>> GetRecentSnapshotsAsync(Guid organizationId, int limit = 12);
    Task<RevenueSnapshot> CreateAsync(RevenueSnapshot snapshot);
    Task<bool> DeleteAsync(Guid id, Guid organizationId);
}
