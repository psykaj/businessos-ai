using backend.Modules.CustomerSuccess.CustomerHealth.DTOs;

namespace backend.Modules.CustomerSuccess.CustomerHealth.Interfaces;

public interface ICustomerHealthService
{
    Task<CustomerHealthDto> GetByCustomerIdAsync(Guid orgId, Guid customerId);
    Task<CustomerHealthDto> CalculateHealthAsync(Guid orgId, Guid customerId);
    Task<CustomerHealthDto> UpdateMetricsAsync(Guid orgId, UpdateCustomerHealthMetricsDto dto);
    Task<(IEnumerable<CustomerHealthDto> Items, int TotalCount)> GetPagedAsync(
        Guid orgId, string? riskLevel, string? search, int page, int pageSize, string? sortBy, bool descending);
    Task<CustomerHealthSummaryDto> GetSummaryAsync(Guid orgId);
    Task RecalculateAllAsync(Guid orgId);
}
