using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Modules.KpiEngine.Entities;

namespace backend.Modules.KpiEngine.Repositories;

public interface IKpiRepository
{
    Task<IEnumerable<KPI>> GetAllAsync(Guid organizationId);
    Task<KPI?> GetByIdAsync(Guid id, Guid organizationId);
    Task<IEnumerable<KPI>> GetByCategoryAsync(string category, Guid organizationId);
    Task<KPI> AddAsync(KPI kpi);
    Task UpdateAsync(KPI kpi);
    Task DeleteAsync(KPI kpi);
    Task<IEnumerable<KPI>> GetAllActiveAsync();
}
