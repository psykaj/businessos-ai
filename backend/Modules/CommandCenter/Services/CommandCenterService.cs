using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using backend.Persistence;
using backend.Modules.CommandCenter.DTOs;
using backend.Modules.CommandCenter.Interfaces;
using backend.Modules.BusinessIntelligence.Interfaces;
using backend.Modules.BusinessIntelligence.Services;
using backend.Modules.BusinessIntelligence.DTOs;
using backend.Modules.ActionCenter.Queries;
using backend.Modules.ActionCenter.DTOs;
using backend.Modules.AiAgent.Executions.Interfaces;
using backend.Modules.AiAgent.Executions.DTOs;
using MediatR;

namespace backend.Modules.CommandCenter.Services;

public class CommandCenterService : ICommandCenterService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IBusinessHealthCalculator _healthCalculator;
    private readonly IProactiveAlertService _alertService;
    private readonly IBusinessBriefingService _briefingService;
    private readonly IMediator _mediator;
    private readonly IAiExecutionEngine _aiExecutionEngine;
    private readonly ILogger<CommandCenterService> _logger;

    public CommandCenterService(
        ApplicationDbContext dbContext,
        IBusinessHealthCalculator healthCalculator,
        IProactiveAlertService alertService,
        IBusinessBriefingService briefingService,
        IMediator mediator,
        IAiExecutionEngine aiExecutionEngine,
        ILogger<CommandCenterService> logger)
    {
        _dbContext = dbContext;
        _healthCalculator = healthCalculator;
        _alertService = alertService;
        _briefingService = briefingService;
        _mediator = mediator;
        _aiExecutionEngine = aiExecutionEngine;
        _logger = logger;
    }

    public async Task<CommandCenterSummaryDto> GetCommandCenterSummaryAsync(Guid organizationId, string userId, CancellationToken cancellationToken = default)
    {
        var summary = new CommandCenterSummaryDto();

        // Fire all queries in parallel where safe and possible
        var healthTask = GetHealthSafeAsync(organizationId, cancellationToken);
        var briefingTask = GetBriefingSafeAsync(organizationId, cancellationToken);
        var metricsTask = GetMetricsSafeAsync(organizationId, cancellationToken);
        var alertsTask = GetAlertsSafeAsync(organizationId, cancellationToken);
        var opportunitiesTask = GetOpportunitiesSafeAsync(organizationId, cancellationToken);
        var actionsTask = GetActionsSafeAsync(organizationId, cancellationToken);
        var automationsTask = GetAutomationsSafeAsync(organizationId, cancellationToken);
        var activityTask = GetActivitySafeAsync(organizationId, cancellationToken);
        var trendsTask = GetTrendsSafeAsync(organizationId, cancellationToken);

        await Task.WhenAll(
            healthTask,
            briefingTask,
            metricsTask,
            alertsTask,
            opportunitiesTask,
            actionsTask,
            automationsTask,
            activityTask,
            trendsTask
        );

        summary.BusinessHealth = await healthTask;
        summary.ExecutiveSummary = await briefingTask;
        summary.KeyMetrics = await metricsTask ?? new List<CommandMetricDto>();
        summary.PriorityAlerts = await alertsTask ?? new List<ProactiveAlertDto>();
        summary.TopOpportunities = await opportunitiesTask ?? new List<CommandOpportunityDto>();
        summary.RecommendedActions = await actionsTask ?? new List<AiActionDto>();
        summary.AutomationSummary = await automationsTask;
        summary.RecentActivity = await activityTask ?? new List<CommandActivityDto>();
        summary.Trends = await trendsTask ?? new List<CommandTrendDto>();

        return summary;
    }

    public async Task<List<CommandMetricDto>> GetMetricsAsync(Guid organizationId, CancellationToken cancellationToken = default)
    {
        // Compute deterministic metrics directly from DbContext
        var metrics = new List<CommandMetricDto>();

        var thisMonthStart = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
        var lastMonthStart = thisMonthStart.AddMonths(-1);

        // Active Customers
        var activeCustomers = await _dbContext.Customers
            .Where(c => c.OrganizationId == organizationId && !c.IsDeleted)
            .CountAsync(cancellationToken);
            
        metrics.Add(new CommandMetricDto
        {
            Name = "Total Customers",
            Value = activeCustomers,
            Trend = "Neutral"
        });

        // Outstanding Invoices
        var outstandingInvoices = await _dbContext.FinanceInvoices
            .Where(i => i.OrganizationId == organizationId && i.Status == "Sent" && !i.IsDeleted)
            .SumAsync(i => i.TotalAmount, cancellationToken);

        metrics.Add(new CommandMetricDto
        {
            Name = "Outstanding Invoices",
            Value = outstandingInvoices,
            Trend = outstandingInvoices > 0 ? "Down" : "Neutral" // Down is bad, meaning we have unpaid invoices. Actually "Down" means negative trend. Let's just say "Neutral".
        });

        // Leads this month
        var leadsThisMonth = await _dbContext.Leads
            .Where(l => l.OrganizationId == organizationId && l.CreatedAt >= thisMonthStart && !l.IsDeleted)
            .CountAsync(cancellationToken);

        metrics.Add(new CommandMetricDto
        {
            Name = "New Leads",
            Value = leadsThisMonth,
            Trend = leadsThisMonth > 0 ? "Up" : "Neutral"
        });

        return metrics;
    }

    public async Task<List<ProactiveAlertDto>> GetAlertsAsync(Guid organizationId, CancellationToken cancellationToken = default)
    {
        return await _alertService.GetUnreadAlertsAsync(organizationId, cancellationToken);
    }

    public async Task<List<CommandOpportunityDto>> GetOpportunitiesAsync(Guid organizationId, CancellationToken cancellationToken = default)
    {
        var opportunities = new List<CommandOpportunityDto>();

        // 1. Unpaid Invoices
        var unpaidInvoices = await _dbContext.FinanceInvoices
            .Where(i => i.OrganizationId == organizationId && i.Status == "Sent" && i.DueDate < DateTime.UtcNow && !i.IsDeleted)
            .OrderByDescending(i => i.TotalAmount)
            .Take(3)
            .ToListAsync(cancellationToken);

        foreach (var inv in unpaidInvoices)
        {
            opportunities.Add(new CommandOpportunityDto
            {
                Id = Guid.NewGuid(),
                Title = $"Overdue Invoice: {inv.InvoiceNumber}",
                Description = $"Invoice is overdue. Follow up to collect {inv.TotalAmount:C}.",
                Category = "Finance",
                Priority = "High",
                PotentialImpact = inv.TotalAmount,
                RecommendedAction = "Send Payment Reminder",
                SourceModule = "Invoices",
                SourceEntityId = inv.Id
            });
        }

        // 2. Uncontacted High Value Leads
        var uncontactedLeads = await _dbContext.Leads
            .Where(l => l.OrganizationId == organizationId && l.Status == backend.Modules.CRM.Enums.LeadStatus.New && l.EstimatedValue > 1000 && !l.IsDeleted)
            .OrderByDescending(l => l.EstimatedValue)
            .Take(2)
            .ToListAsync(cancellationToken);

        foreach (var lead in uncontactedLeads)
        {
            opportunities.Add(new CommandOpportunityDto
            {
                Id = Guid.NewGuid(),
                Title = $"High Value Lead: {lead.FirstName} {lead.LastName}",
                Description = "High potential lead hasn't been contacted yet.",
                Category = "CRM",
                Priority = "High",
                PotentialImpact = lead.EstimatedValue,
                RecommendedAction = "Call Lead",
                SourceModule = "Leads",
                SourceEntityId = lead.Id
            });
        }

        // 3. Low Inventory
        var lowInventory = await _dbContext.InventoryStocks
            .Include(s => s.Product)
            .Where(s => s.OrganizationId == organizationId && s.QuantityOnHand <= s.ReorderLevel)
            .Take(2)
            .ToListAsync(cancellationToken);

        foreach (var stock in lowInventory)
        {
            opportunities.Add(new CommandOpportunityDto
            {
                Id = Guid.NewGuid(),
                Title = $"Low Stock: {stock.Product?.Name}",
                Description = $"Stock is at {stock.QuantityOnHand} (Min: {stock.ReorderLevel}). Restock soon.",
                Category = "Inventory",
                Priority = "Medium",
                RecommendedAction = "Create Purchase Order",
                SourceModule = "Inventory",
                SourceEntityId = stock.ProductId
            });
        }

        return opportunities.OrderByDescending(o => o.Priority == "High" ? 2 : 1).ToList();
    }

    public async Task<List<AiActionDto>> GetActionsAsync(Guid organizationId, CancellationToken cancellationToken = default)
    {
        var actions = await _mediator.Send(new GetPendingActionsQuery { OrganizationId = organizationId }, cancellationToken);
        return actions.ToList();
    }

    public async Task<CommandAutomationSummaryDto> GetAutomationsAsync(Guid organizationId, CancellationToken cancellationToken = default)
    {
        var summary = new CommandAutomationSummaryDto();

        var workflows = await _dbContext.AiEngineWorkflows
            .Where(w => w.OrganizationId == organizationId)
            .ToListAsync(cancellationToken);

        summary.ActiveAutomations = workflows.Count(w => w.Status == "Active");
        summary.PausedAutomations = workflows.Count(w => w.Status == "Paused" || w.Status == "Draft");
        
        var executions = await _dbContext.AiEngineWorkflowExecutions
            .Where(e => e.OrganizationId == organizationId)
            .OrderByDescending(e => e.StartedAt)
            .Take(100)
            .ToListAsync(cancellationToken);

        summary.RecentlyExecuted = executions.Count(e => e.StartedAt >= DateTime.UtcNow.AddDays(-7));
        summary.FailedAutomations = executions.Count(e => e.Status == "Failed" && e.StartedAt >= DateTime.UtcNow.AddDays(-1));
        
        if (executions.Any())
        {
            var successCount = executions.Count(e => e.Status == "Completed");
            summary.AutomationSuccessRate = (decimal)successCount / executions.Count * 100;
        }

        // Optional: Provide automation opportunity if many overdue invoices exist
        var overdueInvoiceCount = await _dbContext.FinanceInvoices.CountAsync(i => i.OrganizationId == organizationId && i.Status == "Sent" && i.DueDate < DateTime.UtcNow && !i.IsDeleted, cancellationToken);
        if (overdueInvoiceCount > 5)
        {
            summary.AutomationOpportunities.Add(new CommandAutomationOpportunityDto
            {
                Title = "Automate Invoice Reminders",
                Description = $"You have {overdueInvoiceCount} overdue invoices. Set up an automation to send follow-up emails automatically."
            });
        }

        return summary;
    }

    public async Task<List<CommandActivityDto>> GetActivityAsync(Guid organizationId, CancellationToken cancellationToken = default)
    {
        var activities = await _dbContext.AuditLogs
            .Where(a => a.OrganizationId == organizationId)
            .OrderByDescending(a => a.CreatedAt)
            .Take(10)
            .Select(a => new CommandActivityDto
            {
                Id = a.Id,
                ActivityType = a.Action,
                Description = a.Description ?? "Action performed",
                Timestamp = a.CreatedAt,
                SourceModule = a.Module
            })
            .ToListAsync(cancellationToken);

        return activities;
    }

    public async Task<List<CommandTrendDto>> GetTrendsAsync(Guid organizationId, CancellationToken cancellationToken = default)
    {
        var trends = new List<CommandTrendDto>();

        // We can just calculate 30 days vs previous 30 days for Customers as an example
        var now = DateTime.UtcNow;
        var thirtyDaysAgo = now.AddDays(-30);
        var sixtyDaysAgo = now.AddDays(-60);

        var newCustomersLast30 = await _dbContext.Customers
            .Where(c => c.OrganizationId == organizationId && c.CreatedAt >= thirtyDaysAgo && !c.IsDeleted)
            .CountAsync(cancellationToken);

        var newCustomersPrev30 = await _dbContext.Customers
            .Where(c => c.OrganizationId == organizationId && c.CreatedAt >= sixtyDaysAgo && c.CreatedAt < thirtyDaysAgo && !c.IsDeleted)
            .CountAsync(cancellationToken);

        decimal customerGrowth = newCustomersPrev30 > 0 
            ? ((decimal)(newCustomersLast30 - newCustomersPrev30) / newCustomersPrev30) * 100 
            : 0;

        trends.Add(new CommandTrendDto
        {
            MetricName = "New Customers",
            Period = "30 Days",
            Value = newCustomersLast30,
            PreviousValue = newCustomersPrev30,
            PercentageChange = Math.Round(customerGrowth, 2),
            TrendDirection = customerGrowth > 0 ? "Up" : customerGrowth < 0 ? "Down" : "Neutral"
        });

        return trends;
    }

    public async Task<CommandCenterAskResponseDto> AskBusinessOsAiAsync(Guid organizationId, string userId, CommandCenterAskRequestDto request, CancellationToken cancellationToken = default)
    {
        var response = new CommandCenterAskResponseDto();

        try
        {
            var execRequest = new ExecuteCommandRequestDto
            {
                Command = request.Question
            };

            var aiResponse = await _aiExecutionEngine.ExecuteCommandAsync(organizationId, userId, execRequest, cancellationToken);

            response.Answer = aiResponse.ResultSummary ?? "I've analyzed your data.";
            response.Confidence = "High";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing AI command.");
            response.Answer = "I'm currently unable to process your request due to a technical issue. Please try again later.";
            response.Confidence = "Low";
        }

        return response;
    }

    // Safe wrappers for graceful degradation
    private async Task<BusinessHealthDto?> GetHealthSafeAsync(Guid orgId, CancellationToken token)
    {
        try { return await _healthCalculator.CalculateHealthAsync(orgId, token); }
        catch (Exception ex) { _logger.LogError(ex, "Failed to get business health"); return null; }
    }

    private async Task<BusinessBriefingDto?> GetBriefingSafeAsync(Guid orgId, CancellationToken token)
    {
        try { return await _briefingService.GetTodayBriefingAsync(orgId, token); }
        catch (Exception ex) { _logger.LogError(ex, "Failed to get executive summary"); return null; }
    }

    private async Task<List<CommandMetricDto>?> GetMetricsSafeAsync(Guid orgId, CancellationToken token)
    {
        try { return await GetMetricsAsync(orgId, token); }
        catch (Exception ex) { _logger.LogError(ex, "Failed to get metrics"); return null; }
    }

    private async Task<List<ProactiveAlertDto>?> GetAlertsSafeAsync(Guid orgId, CancellationToken token)
    {
        try { return await GetAlertsAsync(orgId, token); }
        catch (Exception ex) { _logger.LogError(ex, "Failed to get alerts"); return null; }
    }

    private async Task<List<CommandOpportunityDto>?> GetOpportunitiesSafeAsync(Guid orgId, CancellationToken token)
    {
        try { return await GetOpportunitiesAsync(orgId, token); }
        catch (Exception ex) { _logger.LogError(ex, "Failed to get opportunities"); return null; }
    }

    private async Task<List<AiActionDto>?> GetActionsSafeAsync(Guid orgId, CancellationToken token)
    {
        try { return await GetActionsAsync(orgId, token); }
        catch (Exception ex) { _logger.LogError(ex, "Failed to get actions"); return null; }
    }

    private async Task<CommandAutomationSummaryDto?> GetAutomationsSafeAsync(Guid orgId, CancellationToken token)
    {
        try { return await GetAutomationsAsync(orgId, token); }
        catch (Exception ex) { _logger.LogError(ex, "Failed to get automations"); return null; }
    }

    private async Task<List<CommandActivityDto>?> GetActivitySafeAsync(Guid orgId, CancellationToken token)
    {
        try { return await GetActivityAsync(orgId, token); }
        catch (Exception ex) { _logger.LogError(ex, "Failed to get activity"); return null; }
    }

    private async Task<List<CommandTrendDto>?> GetTrendsSafeAsync(Guid orgId, CancellationToken token)
    {
        try { return await GetTrendsAsync(orgId, token); }
        catch (Exception ex) { _logger.LogError(ex, "Failed to get trends"); return null; }
    }
}
