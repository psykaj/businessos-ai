using backend.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.LeadCapture.Controllers;

[Authorize]
[ApiController]
[Route("api/marketing/analytics")]
public class MarketingAnalyticsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public MarketingAnalyticsController(ApplicationDbContext context)
    {
        _context = context;
    }

    private Guid OrganizationId => Guid.Parse(User.FindFirst("OrganizationId")?.Value ?? Guid.Empty.ToString());

    [HttpGet("dashboard")]
    public async Task<IActionResult> GetDashboardMetrics(CancellationToken cancellationToken)
    {
        var totalForms = await _context.Forms.CountAsync(f => f.OrganizationId == OrganizationId && !f.IsDeleted, cancellationToken);
        var totalSubmissions = await _context.FormSubmissions.CountAsync(s => s.OrganizationId == OrganizationId && !s.IsDeleted, cancellationToken);
        
        var conversionRate = totalForms > 0 && totalSubmissions > 0 
            ? Math.Round((double)totalSubmissions / (totalForms * 100), 2) // Simplified dummy logic
            : 0;

        var bestCampaign = await _context.Campaigns
            .Where(c => c.OrganizationId == OrganizationId && !c.IsDeleted)
            .OrderByDescending(c => c.Budget) // Just a placeholder for actual performance metric
            .Select(c => c.Name)
            .FirstOrDefaultAsync(cancellationToken);

        var topLeadSource = await _context.Leads
            .Where(l => l.OrganizationId == OrganizationId && !string.IsNullOrEmpty(l.Source))
            .GroupBy(l => l.Source)
            .Select(g => new { Source = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .Select(x => x.Source)
            .FirstOrDefaultAsync(cancellationToken) ?? "Unknown";

        var today = DateTime.UtcNow.Date;
        var dailyLeads = await _context.Leads
            .CountAsync(l => l.OrganizationId == OrganizationId && l.CreatedAt >= today, cancellationToken);

        var thisMonth = new DateTime(today.Year, today.Month, 1).ToUniversalTime();
        var monthlyLeads = await _context.Leads
            .CountAsync(l => l.OrganizationId == OrganizationId && l.CreatedAt >= thisMonth, cancellationToken);

        return Ok(new
        {
            TotalForms = totalForms,
            TotalSubmissions = totalSubmissions,
            ConversionRate = $"{conversionRate}%",
            BestCampaign = bestCampaign ?? "N/A",
            TopLeadSource = topLeadSource,
            DailyLeads = dailyLeads,
            MonthlyLeads = monthlyLeads
        });
    }
}
