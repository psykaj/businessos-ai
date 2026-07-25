using backend.Modules.CustomerSuccess.Referrals.DTOs;

namespace backend.Modules.CustomerSuccess.Referrals.Interfaces;

public interface IReferralService
{
    Task<ReferralDto> CreateReferralAsync(Guid orgId, CreateReferralDto dto);
    Task<ReferralDto> GetByCodeAsync(Guid orgId, string code);
    Task<ReferralDto> ConvertReferralAsync(Guid orgId, ConvertReferralDto dto);
    Task<ReferralDto> IssueRewardAsync(Guid orgId, Guid referralId);
    Task<(IEnumerable<ReferralDto> Items, int TotalCount)> GetPagedAsync(
        Guid orgId, string? status, string? search, int page, int pageSize);
    Task<ReferralAnalyticsDto> GetAnalyticsAsync(Guid orgId);
    Task<IEnumerable<ReferralDto>> GetByReferrerAsync(Guid orgId, Guid referrerId);
}
