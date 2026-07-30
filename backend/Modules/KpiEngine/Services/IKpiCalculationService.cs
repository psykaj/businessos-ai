using backend.Modules.KpiEngine.Entities;

namespace backend.Modules.KpiEngine.Services;

public interface IKpiCalculationService
{
    Task<KPI> CalculateKpiAsync(Guid organizationId, Guid kpiId);
    Task CalculateAllKpisAsync(Guid organizationId);
}
