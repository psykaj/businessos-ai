using backend.Modules.CustomerSuccess.Retention.DTOs;

namespace backend.Modules.CustomerSuccess.Retention.Interfaces;

public interface IRetentionService
{
    Task<RetentionOverviewDto> GetRetentionOverviewAsync(Guid orgId);
}
