using backend.Modules.CustomerSuccess.CustomerHealth.Interfaces;
using backend.Modules.CustomerSuccess.Retention.DTOs;
using backend.Modules.CustomerSuccess.Retention.Interfaces;

namespace backend.Modules.CustomerSuccess.Retention.Services;

public class RetentionService : IRetentionService
{
    private readonly ICustomerHealthRepository _healthRepo;

    public RetentionService(ICustomerHealthRepository healthRepo)
    {
        _healthRepo = healthRepo;
    }

    public async Task<RetentionOverviewDto> GetRetentionOverviewAsync(Guid orgId)
    {
        var (records, totalCount) = await _healthRepo.GetPagedAsync(orgId, null, null, 1, 10000, null, false);
        var recordList = records.ToList();

        if (!recordList.Any())
        {
            return new RetentionOverviewDto(100.0, 0.0, 0, 0, 0, Array.Empty<RetentionActionItemDto>());
        }

        int atRiskCount = recordList.Count(r => r.RiskLevel == "High Risk" || r.RiskLevel == "Needs Attention");
        int activeCount = recordList.Count(r => r.RiskLevel == "Healthy" || r.RiskLevel == "Stable");
        decimal revenueAtRisk = recordList.Where(r => r.RiskLevel == "High Risk" || r.RiskLevel == "Needs Attention").Sum(r => r.LifetimeValue);

        double retentionRate = totalCount > 0 ? (activeCount / (double)totalCount) * 100.0 : 100.0;
        double churnRate = 100.0 - retentionRate;

        var actions = new List<RetentionActionItemDto>();
        if (atRiskCount > 0)
        {
            actions.Add(new RetentionActionItemDto(
                "HighRiskOutreach",
                $"Reach out to {atRiskCount} accounts at risk of churning to prevent ${revenueAtRisk:N2} revenue loss.",
                "High",
                atRiskCount
            ));
        }

        int inactiveCount = recordList.Count(r => r.LastInteractionDate.HasValue && (DateTime.UtcNow - r.LastInteractionDate.Value).TotalDays > 30);
        if (inactiveCount > 0)
        {
            actions.Add(new RetentionActionItemDto(
                "ReEngagementCampaign",
                $"Trigger email/WhatsApp re-engagement workflow for {inactiveCount} inactive customers.",
                "Medium",
                inactiveCount
            ));
        }

        return new RetentionOverviewDto(
            Math.Round(retentionRate, 1),
            Math.Round(churnRate, 1),
            atRiskCount,
            activeCount,
            revenueAtRisk,
            actions
        );
    }
}
