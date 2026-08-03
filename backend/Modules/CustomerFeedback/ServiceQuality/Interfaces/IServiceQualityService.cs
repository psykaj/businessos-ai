using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Modules.CustomerFeedback.Entities;
using backend.Modules.CustomerFeedback.ServiceQuality.DTOs;

namespace backend.Modules.CustomerFeedback.ServiceQuality.Interfaces;

public interface IServiceQualityRepository
{
    Task<ServiceMetric?> GetByDateAsync(Guid organizationId, DateTime date);
    Task<IEnumerable<ServiceMetric>> GetHistoryAsync(Guid organizationId, int days);
    Task<ServiceMetric> AddOrUpdateAsync(ServiceMetric metric);
}

public interface IServiceQualityService
{
    Task<ServiceMetricDto> GetLatestKpisAsync(Guid organizationId);
    Task<IEnumerable<ServiceMetricDto>> GetKpiHistoryAsync(Guid organizationId, int days = 30);
    Task<ServiceMetricDto> RecordKpiAsync(RecordServiceMetricRequestDto request);
}
