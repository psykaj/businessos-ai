using backend.Common;
using backend.Modules.Analytics.DTOs;

namespace backend.Modules.Analytics.Repositories;

public interface IAnalyticsRepository
{
    Task<AnalyticsOverviewDto> GetOverviewAsync(Guid organizationId, DateTime? startDate, DateTime? endDate);
    Task<QRPerformanceDto> GetQRPerformanceAsync(Guid organizationId, Guid qrCodeId, DateTime? startDate, DateTime? endDate);
    Task<IEnumerable<ScanTimelineDto>> GetTimelineAsync(Guid organizationId, DateTime? startDate, DateTime? endDate);
    Task<IEnumerable<DeviceAnalyticsDto>> GetDeviceAnalyticsAsync(Guid organizationId, DateTime? startDate, DateTime? endDate);
    Task<IEnumerable<BrowserAnalyticsDto>> GetBrowserAnalyticsAsync(Guid organizationId, DateTime? startDate, DateTime? endDate);
    Task<IEnumerable<CountryAnalyticsDto>> GetCountryAnalyticsAsync(Guid organizationId, DateTime? startDate, DateTime? endDate);
    Task<IEnumerable<ReferrerAnalyticsDto>> GetReferrerAnalyticsAsync(Guid organizationId, DateTime? startDate, DateTime? endDate);
    Task<backend.Common.PagedResult<ScanHistoryDto>> GetHistoryAsync(Guid organizationId, Guid? qrCodeId, string? search, int page, int pageSize, DateTime? startDate, DateTime? endDate);
}
