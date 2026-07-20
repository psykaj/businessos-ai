using backend.Modules.CRM.DTOs;
using backend.Modules.CRM.Enums;
using backend.Modules.CRM.Interfaces;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.CRM.Services;

public class CrmReportingService : ICrmReportingService
{
    private readonly ApplicationDbContext _context;

    public CrmReportingService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CrmOverviewDto> GetOverviewAsync(Guid organizationId)
    {
        var totalLeads = await _context.Leads.CountAsync(l => l.OrganizationId == organizationId);
        var totalDeals = await _context.Deals.CountAsync(d => d.OrganizationId == organizationId);
        
        var openDeals = await _context.Deals
            .Where(d => d.OrganizationId == organizationId && d.PipelineStage != PipelineStage.Won && d.PipelineStage != PipelineStage.Lost)
            .ToListAsync();
            
        var pipelineValue = openDeals.Sum(d => d.Amount);
        var revenueForecast = openDeals.Sum(d => d.Amount * (d.Probability / 100m));
        
        var wonDeals = await _context.Deals.CountAsync(d => d.OrganizationId == organizationId && d.PipelineStage == PipelineStage.Won);
        var conversionRate = totalDeals > 0 ? (decimal)wonDeals / totalDeals * 100 : 0;

        return new CrmOverviewDto
        {
            TotalLeads = totalLeads,
            TotalDeals = totalDeals,
            PipelineValue = pipelineValue,
            RevenueForecast = revenueForecast,
            ConversionRate = conversionRate
        };
    }

    public async Task<IReadOnlyList<SalesPerformanceDto>> GetSalesPerformanceAsync(Guid organizationId, DateTime? startDate, DateTime? endDate)
    {
        var start = startDate ?? DateTime.UtcNow.AddMonths(-6);
        var end = endDate ?? DateTime.UtcNow;

        var wonDeals = await _context.Deals
            .Where(d => d.OrganizationId == organizationId && 
                        d.PipelineStage == PipelineStage.Won &&
                        d.CreatedAt >= start && d.CreatedAt <= end)
            .ToListAsync();

        var performance = wonDeals
            .GroupBy(d => new { d.CreatedAt.Year, d.CreatedAt.Month })
            .Select(g => new SalesPerformanceDto
            {
                Date = new DateTime(g.Key.Year, g.Key.Month, 1),
                Revenue = g.Sum(d => d.Amount),
                DealsWon = g.Count()
            })
            .OrderBy(p => p.Date)
            .ToList();

        return performance;
    }
}
