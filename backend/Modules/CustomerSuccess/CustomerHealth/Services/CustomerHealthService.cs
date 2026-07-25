using backend.Modules.CustomerSuccess.CustomerHealth.DTOs;
using backend.Modules.CustomerSuccess.CustomerHealth.Entities;
using backend.Modules.CustomerSuccess.CustomerHealth.Interfaces;
using backend.Modules.CustomerSuccess.SuccessTasks.Entities;
using backend.Modules.CustomerSuccess.SuccessTasks.Interfaces;

namespace backend.Modules.CustomerSuccess.CustomerHealth.Services;

public class CustomerHealthService : ICustomerHealthService
{
    private readonly ICustomerHealthRepository _healthRepo;
    private readonly ISuccessTaskRepository _taskRepo;

    public CustomerHealthService(
        ICustomerHealthRepository healthRepo,
        ISuccessTaskRepository taskRepo)
    {
        _healthRepo = healthRepo;
        _taskRepo = taskRepo;
    }

    public async Task<CustomerHealthDto> GetByCustomerIdAsync(Guid orgId, Guid customerId)
    {
        var health = await _healthRepo.GetByCustomerIdAsync(orgId, customerId);
        if (health == null)
        {
            return await CalculateHealthAsync(orgId, customerId);
        }

        return MapToDto(health);
    }

    public async Task<CustomerHealthDto> CalculateHealthAsync(Guid orgId, Guid customerId)
    {
        var health = await _healthRepo.GetByCustomerIdAsync(orgId, customerId);
        if (health == null)
        {
            health = new Entities.CustomerHealth
            {
                OrganizationId = orgId,
                CustomerId = customerId,
                LastInteractionDate = DateTime.UtcNow,
                CalculatedAt = DateTime.UtcNow
            };
            await _healthRepo.AddAsync(health);
        }

        RecomputeHealth(health);
        health.CalculatedAt = DateTime.UtcNow;

        await _healthRepo.UpdateAsync(health);
        await _healthRepo.SaveChangesAsync();

        // Auto-create Success Task if customer is At Risk or High Risk
        if (health.RiskLevel == "High Risk")
        {
            var existingTasks = await _taskRepo.GetPagedAsync(orgId, customerId, null, "Pending", null, "ContactDissatisfied", 1, 5);
            if (!existingTasks.Items.Any())
            {
                await _taskRepo.AddAsync(new SuccessTask
                {
                    OrganizationId = orgId,
                    CustomerId = customerId,
                    TaskType = "ContactDissatisfied",
                    Title = $"High Risk Customer Alert - Immediate Follow Up",
                    Description = $"Customer Health Score dropped to {health.HealthScore} ({health.RiskLevel}). Immediate outreach recommended.",
                    Priority = "Critical",
                    Status = "Pending",
                    DueDate = DateTime.UtcNow.AddDays(1)
                });
                await _taskRepo.SaveChangesAsync();
            }
        }

        return MapToDto(health);
    }

    public async Task<CustomerHealthDto> UpdateMetricsAsync(Guid orgId, UpdateCustomerHealthMetricsDto dto)
    {
        var health = await _healthRepo.GetByCustomerIdAsync(orgId, dto.CustomerId);
        if (health == null)
        {
            health = new Entities.CustomerHealth
            {
                OrganizationId = orgId,
                CustomerId = dto.CustomerId
            };
            await _healthRepo.AddAsync(health);
        }

        if (dto.LastPurchaseDate.HasValue) health.LastPurchaseDate = dto.LastPurchaseDate;
        if (dto.LastInteractionDate.HasValue) health.LastInteractionDate = dto.LastInteractionDate;
        if (dto.AdditionalPurchaseValue.HasValue)
        {
            health.LifetimeValue += dto.AdditionalPurchaseValue.Value;
            health.PurchaseFrequency++;
        }
        if (dto.OutstandingPayments.HasValue) health.OutstandingPayments = dto.OutstandingPayments.Value;
        if (dto.SupportTicketDelta.HasValue) health.SupportTicketCount = Math.Max(0, health.SupportTicketCount + dto.SupportTicketDelta.Value);
        if (dto.LatestRating.HasValue) health.SatisfactionRating = dto.LatestRating;
        if (dto.ReferralDelta.HasValue) health.ReferralCount = Math.Max(0, health.ReferralCount + dto.ReferralDelta.Value);

        RecomputeHealth(health);
        health.CalculatedAt = DateTime.UtcNow;

        await _healthRepo.UpdateAsync(health);
        await _healthRepo.SaveChangesAsync();

        return MapToDto(health);
    }

    public async Task<(IEnumerable<CustomerHealthDto> Items, int TotalCount)> GetPagedAsync(
        Guid orgId, string? riskLevel, string? search, int page, int pageSize, string? sortBy, bool descending)
    {
        var (items, totalCount) = await _healthRepo.GetPagedAsync(orgId, riskLevel, search, page, pageSize, sortBy, descending);
        return (items.Select(MapToDto), totalCount);
    }

    public async Task<CustomerHealthSummaryDto> GetSummaryAsync(Guid orgId)
    {
        var (items, totalCount) = await _healthRepo.GetPagedAsync(orgId, null, null, 1, 10000, null, false);
        var itemList = items.ToList();

        int healthy = itemList.Count(i => i.RiskLevel == "Healthy");
        int stable = itemList.Count(i => i.RiskLevel == "Stable");
        int needsAttn = itemList.Count(i => i.RiskLevel == "Needs Attention");
        int highRisk = itemList.Count(i => i.RiskLevel == "High Risk");
        double avgScore = itemList.Any() ? itemList.Average(i => i.HealthScore) : 100.0;

        return new CustomerHealthSummaryDto(totalCount, healthy, stable, needsAttn, highRisk, Math.Round(avgScore, 1));
    }

    public async Task RecalculateAllAsync(Guid orgId)
    {
        var (items, _) = await _healthRepo.GetPagedAsync(orgId, null, null, 1, 10000, null, false);
        foreach (var item in items)
        {
            RecomputeHealth(item);
            item.CalculatedAt = DateTime.UtcNow;
            await _healthRepo.UpdateAsync(item);
        }
        await _healthRepo.SaveChangesAsync();
    }

    private static void RecomputeHealth(Entities.CustomerHealth health)
    {
        double score = 80.0; // Base baseline

        // 1. Recency of last interaction
        if (health.LastInteractionDate.HasValue)
        {
            var daysInactive = (DateTime.UtcNow - health.LastInteractionDate.Value).TotalDays;
            if (daysInactive > 60) score -= 30;
            else if (daysInactive > 30) score -= 15;
            else if (daysInactive <= 7) score += 10;
        }

        // 2. Purchase Frequency & Recency
        score += Math.Min(20, health.PurchaseFrequency * 3);
        if (health.LastPurchaseDate.HasValue)
        {
            var daysSincePurchase = (DateTime.UtcNow - health.LastPurchaseDate.Value).TotalDays;
            if (daysSincePurchase > 90) score -= 20;
            else if (daysSincePurchase <= 14) score += 10;
        }

        // 3. Lifetime Value bonus
        if (health.LifetimeValue > 1000) score += 15;
        else if (health.LifetimeValue > 500) score += 10;
        else if (health.LifetimeValue > 100) score += 5;

        // 4. Penalties for Outstanding Payments & Support Tickets
        if (health.OutstandingPayments > 500) score -= 25;
        else if (health.OutstandingPayments > 0) score -= 10;

        if (health.SupportTicketCount > 3) score -= 20;
        else if (health.SupportTicketCount > 0) score -= health.SupportTicketCount * 4;

        // 5. Satisfaction rating bonus/penalty
        if (health.SatisfactionRating.HasValue)
        {
            if (health.SatisfactionRating >= 4.5) score += 15;
            else if (health.SatisfactionRating >= 3.5) score += 5;
            else if (health.SatisfactionRating <= 2.0) score -= 25;
        }

        // 6. Referral Activity
        score += Math.Min(15, health.ReferralCount * 5);

        // Clamp 0 - 100
        int finalScore = Math.Clamp((int)Math.Round(score), 0, 100);
        health.HealthScore = finalScore;

        health.RiskLevel = finalScore switch
        {
            >= 80 => "Healthy",
            >= 60 => "Stable",
            >= 40 => "Needs Attention",
            _ => "High Risk"
        };
    }

    private static CustomerHealthDto MapToDto(Entities.CustomerHealth h)
    {
        return new CustomerHealthDto(
            h.Id,
            h.CustomerId,
            h.Customer?.Name ?? "Unknown Customer",
            h.HealthScore,
            h.RiskLevel,
            h.LastPurchaseDate,
            h.LastInteractionDate,
            h.LifetimeValue,
            h.OutstandingPayments,
            h.SupportTicketCount,
            h.SatisfactionRating,
            h.ReferralCount,
            h.PurchaseFrequency,
            h.CalculatedAt
        );
    }
}
