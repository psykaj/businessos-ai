using backend.Modules.AiAgent.Entities;
using backend.Modules.AiAgent.Recommendations.Interfaces;
using backend.Modules.AiAgent.Repositories;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.AiAgent.Recommendations.Services;

public class AiRecommendationEngine : IAiRecommendationEngine
{
    private readonly ApplicationDbContext _context;
    private readonly IRecommendationRepository _recommendationRepo;

    public AiRecommendationEngine(
        ApplicationDbContext context,
        IRecommendationRepository recommendationRepo)
    {
        _context = context;
        _recommendationRepo = recommendationRepo;
    }

    public async Task<List<Recommendation>> GenerateRecommendationsAsync(Guid organizationId, CancellationToken cancellationToken = default)
    {
        var recommendations = new List<Recommendation>();

        // 1. Check Inactive Leads (No activity in 14+ days)
        var fourteenDaysAgo = DateTime.UtcNow.AddDays(-14);
        var inactiveLeadsCount = await _context.Leads
            .AsNoTracking()
            .CountAsync(l => l.OrganizationId == organizationId && !l.IsDeleted && l.UpdatedAt < fourteenDaysAgo, cancellationToken);

        if (inactiveLeadsCount > 0)
        {
            recommendations.Add(new Recommendation
            {
                Id = Guid.NewGuid(),
                OrganizationId = organizationId,
                Category = "CRM",
                Title = "Follow up with inactive leads",
                Description = $"{inactiveLeadsCount} leads have had no interactions in over 14 days.",
                Priority = "High",
                Reason = "Leads left uncontacted for more than 2 weeks drop conversion likelihood by 65%.",
                SuggestedAction = "Assign inactive leads to sales representatives or trigger automated re-engagement email flow."
            });
        }

        // 2. Outstanding / Overdue Invoices
        var unpaidInvoices = await _context.Invoices
            .AsNoTracking()
            .Where(i => i.OrganizationId == organizationId && i.Status != "Paid" && !i.IsDeleted)
            .ToListAsync(cancellationToken);

        if (unpaidInvoices.Any())
        {
            var totalOutstanding = unpaidInvoices.Sum(i => i.Amount);
            recommendations.Add(new Recommendation
            {
                Id = Guid.NewGuid(),
                OrganizationId = organizationId,
                Category = "Billing",
                Title = "Outstanding invoices require attention",
                Description = $"{unpaidInvoices.Count} pending invoice(s) totaling ${totalOutstanding:F2} await collection.",
                Priority = totalOutstanding > 1000 ? "High" : "Medium",
                Reason = "Delayed receivables constrain company cash flow.",
                SuggestedAction = "Send automated WhatsApp/Email payment reminders to accounts with pending balances."
            });
        }

        // 3. Campaign Performance Check
        var activeCampaignsCount = await _context.Campaigns
            .AsNoTracking()
            .CountAsync(c => c.OrganizationId == organizationId && !c.IsDeleted, cancellationToken);

        if (activeCampaignsCount == 0)
        {
            recommendations.Add(new Recommendation
            {
                Id = Guid.NewGuid(),
                OrganizationId = organizationId,
                Category = "Marketing",
                Title = "No active marketing campaigns running",
                Description = "Your organization currently has zero active marketing campaigns.",
                Priority = "Medium",
                Reason = "Continuous inbound lead generation requires active outreach channels.",
                SuggestedAction = "Launch a QR-code or email promotion campaign to drive new customer acquisition."
            });
        }

        // Save generated recommendations to repository
        if (recommendations.Any())
        {
            await _recommendationRepo.AddRangeAsync(recommendations, cancellationToken);
            await _recommendationRepo.SaveChangesAsync(cancellationToken);
        }

        return await _recommendationRepo.GetAllByOrganizationIdAsync(organizationId, includeApplied: false, cancellationToken);
    }

    public async Task<Recommendation?> ApplyRecommendationAsync(Guid recommendationId, Guid organizationId, CancellationToken cancellationToken = default)
    {
        var rec = await _recommendationRepo.GetByIdAsync(recommendationId, organizationId, cancellationToken);
        if (rec == null) return null;

        rec.IsApplied = true;
        rec.AppliedAt = DateTime.UtcNow;
        _recommendationRepo.Update(rec);
        await _recommendationRepo.SaveChangesAsync(cancellationToken);

        return rec;
    }
}
