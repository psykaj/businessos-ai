using System;
using System.Threading.Tasks;
using backend.Modules.GrowthCenter.DTOs;

namespace backend.Modules.GrowthCenter.Interfaces;

public interface IGrowthCenterService
{
    Task<GrowthCenterOverviewDto> GetOverviewAsync(Guid organizationId, bool forceRefresh = false);
    Task<ActionPlanDto> GetActionPlanAsync(Guid organizationId);
    Task<GrowthCenterOverviewDto> RefreshAllEnginesAsync(Guid organizationId);
    Task<string> ExportMasterGrowthReportAsync(Guid organizationId);
}
