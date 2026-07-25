using backend.Modules.CustomerSuccess.Referrals.Entities;

namespace backend.Modules.CustomerSuccess.Referrals.Interfaces;

public interface IReferralRepository
{
    Task<Referral?> GetByIdAsync(Guid orgId, Guid id);
    Task<Referral?> GetByCodeAsync(Guid orgId, string code);
    Task<Referral?> GetByReferrerAndReferredAsync(Guid orgId, Guid referrerId, Guid referredId);
    Task<IEnumerable<Referral>> GetByReferrerIdAsync(Guid orgId, Guid referrerId);
    Task<(IEnumerable<Referral> Items, int TotalCount)> GetPagedAsync(
        Guid orgId, string? status, string? search, int page, int pageSize);
    Task AddAsync(Referral referral);
    Task UpdateAsync(Referral referral);
    Task<int> GetConversionCountAsync(Guid orgId, Guid referrerId);
    Task SaveChangesAsync();
}
