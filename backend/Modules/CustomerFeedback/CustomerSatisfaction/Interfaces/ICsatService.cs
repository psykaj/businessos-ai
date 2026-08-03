using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Modules.CustomerFeedback.CustomerSatisfaction.DTOs;
using backend.Modules.CustomerFeedback.Entities;

namespace backend.Modules.CustomerFeedback.CustomerSatisfaction.Interfaces;

public interface ICsatRepository
{
    Task<CustomerSatisfactionScore?> GetByCustomerIdAsync(Guid organizationId, Guid customerId);
    Task<IEnumerable<CustomerSatisfactionScore>> GetAllAsync(Guid organizationId);
    Task<CustomerSatisfactionScore> AddOrUpdateAsync(CustomerSatisfactionScore entity);
}

public interface ICsatService
{
    Task<CsatDashboardDto> GetDashboardMetricsAsync(Guid organizationId);
    Task<CustomerCsatDto> GetCustomerScoreAsync(Guid organizationId, Guid customerId);
    Task RecalculateCustomerMetricsAsync(Guid organizationId, Guid customerId);
}
