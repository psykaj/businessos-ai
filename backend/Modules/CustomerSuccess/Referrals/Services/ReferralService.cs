using backend.Modules.CustomerSuccess.CustomerHealth.Interfaces;
using backend.Modules.CustomerSuccess.Loyalty.DTOs;
using backend.Modules.CustomerSuccess.Loyalty.Interfaces;
using backend.Modules.CustomerSuccess.Referrals.DTOs;
using backend.Modules.CustomerSuccess.Referrals.Entities;
using backend.Modules.CustomerSuccess.Referrals.Interfaces;

namespace backend.Modules.CustomerSuccess.Referrals.Services;

public class ReferralService : IReferralService
{
    private readonly IReferralRepository _referralRepo;
    private readonly ICustomerHealthRepository _healthRepo;
    private readonly ILoyaltyService _loyaltyService;

    public ReferralService(
        IReferralRepository referralRepo,
        ICustomerHealthRepository healthRepo,
        ILoyaltyService loyaltyService)
    {
        _referralRepo = referralRepo;
        _healthRepo = healthRepo;
        _loyaltyService = loyaltyService;
    }

    public async Task<ReferralDto> CreateReferralAsync(Guid orgId, CreateReferralDto dto)
    {
        string code = $"REF-{orgId.ToString()[..4].ToUpper()}-{Guid.NewGuid().ToString()[..6].ToUpper()}";

        var referral = new Referral
        {
            OrganizationId = orgId,
            ReferrerCustomerId = dto.ReferrerCustomerId,
            ReferralCode = code,
            Status = "Pending",
            RewardIssued = false,
            RewardAmount = dto.RewardAmount > 0 ? dto.RewardAmount : 50, // default reward 50 points or $50
            Notes = dto.Notes
        };

        await _referralRepo.AddAsync(referral);
        await _referralRepo.SaveChangesAsync();

        return MapToDto(referral);
    }

    public async Task<ReferralDto> GetByCodeAsync(Guid orgId, string code)
    {
        var referral = await _referralRepo.GetByCodeAsync(orgId, code)
            ?? throw new KeyNotFoundException($"Referral code '{code}' not found.");

        return MapToDto(referral);
    }

    public async Task<ReferralDto> ConvertReferralAsync(Guid orgId, ConvertReferralDto dto)
    {
        var referral = await _referralRepo.GetByCodeAsync(orgId, dto.ReferralCode)
            ?? throw new KeyNotFoundException($"Referral code '{dto.ReferralCode}' not found.");

        if (referral.Status == "Converted" || referral.Status == "Rewarded")
        {
            throw new InvalidOperationException("Referral code has already been converted.");
        }

        referral.ReferredCustomerId = dto.ReferredCustomerId;
        referral.Status = "Converted";

        await _referralRepo.UpdateAsync(referral);
        await _referralRepo.SaveChangesAsync();

        // Update Customer Health referral metric for referrer
        var health = await _healthRepo.GetByCustomerIdAsync(orgId, referral.ReferrerCustomerId);
        if (health != null)
        {
            health.ReferralCount++;
            await _healthRepo.UpdateAsync(health);
            await _healthRepo.SaveChangesAsync();
        }

        // Auto issue reward
        return await IssueRewardAsync(orgId, referral.Id);
    }

    public async Task<ReferralDto> IssueRewardAsync(Guid orgId, Guid referralId)
    {
        var referral = await _referralRepo.GetByIdAsync(orgId, referralId)
            ?? throw new KeyNotFoundException("Referral not found.");

        if (referral.RewardIssued)
        {
            return MapToDto(referral);
        }

        // Earn loyalty points for referrer
        int points = (int)referral.RewardAmount;
        if (points > 0)
        {
            await _loyaltyService.AdjustPointsAsync(orgId, new AdjustPointsDto(
                referral.ReferrerCustomerId,
                null,
                points,
                $"Referral reward for code {referral.ReferralCode}"
            ));
        }

        referral.RewardIssued = true;
        referral.Status = "Rewarded";

        await _referralRepo.UpdateAsync(referral);
        await _referralRepo.SaveChangesAsync();

        return MapToDto(referral);
    }

    public async Task<(IEnumerable<ReferralDto> Items, int TotalCount)> GetPagedAsync(
        Guid orgId, string? status, string? search, int page, int pageSize)
    {
        var (items, totalCount) = await _referralRepo.GetPagedAsync(orgId, status, search, page, pageSize);
        return (items.Select(MapToDto), totalCount);
    }

    public async Task<ReferralAnalyticsDto> GetAnalyticsAsync(Guid orgId)
    {
        var (items, totalCount) = await _referralRepo.GetPagedAsync(orgId, null, null, 1, 10000);
        var referralList = items.ToList();

        int pending = referralList.Count(r => r.Status == "Pending");
        int converted = referralList.Count(r => r.Status == "Converted");
        int rewarded = referralList.Count(r => r.Status == "Rewarded");
        decimal totalRewards = referralList.Where(r => r.RewardIssued).Sum(r => r.RewardAmount);
        double conversionRate = totalCount > 0 ? ((converted + rewarded) / (double)totalCount) * 100.0 : 0.0;

        return new ReferralAnalyticsDto(
            totalCount,
            pending,
            converted,
            rewarded,
            totalRewards,
            Math.Round(conversionRate, 1)
        );
    }

    public async Task<IEnumerable<ReferralDto>> GetByReferrerAsync(Guid orgId, Guid referrerId)
    {
        var referrals = await _referralRepo.GetByReferrerIdAsync(orgId, referrerId);
        return referrals.Select(MapToDto);
    }

    private static ReferralDto MapToDto(Referral r)
    {
        return new ReferralDto(
            r.Id,
            r.ReferrerCustomerId,
            r.ReferrerCustomer?.Name ?? "Referrer Customer",
            r.ReferredCustomerId,
            r.ReferredCustomer?.Name,
            r.ReferralCode,
            r.Status,
            r.RewardIssued,
            r.RewardAmount,
            r.Notes,
            r.CreatedAt
        );
    }
}
