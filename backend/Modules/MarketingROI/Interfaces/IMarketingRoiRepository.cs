using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Common;
using backend.Modules.MarketingROI.Entities;

namespace backend.Modules.MarketingROI.Interfaces;

public interface IMarketingRoiRepository
{
    Task<MarketingPerformance?> GetByIdAsync(Guid id, Guid organizationId);
    Task<PagedResult<MarketingPerformance>> GetPerformancesAsync(Guid organizationId, string? channel, string? period, int pageNumber, int pageSize);
    Task<List<MarketingPerformance>> GetAllActiveAsync(Guid organizationId);
    Task<MarketingPerformance> CreateAsync(MarketingPerformance performance);
    Task<bool> DeleteAsync(Guid id, Guid organizationId);
}
