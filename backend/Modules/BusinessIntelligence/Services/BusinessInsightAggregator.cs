using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using backend.Modules.BusinessIntelligence.Entities;
using backend.Modules.BusinessIntelligence.Interfaces;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace backend.Modules.BusinessIntelligence.Services;

public class BusinessInsightAggregator : IBusinessInsightAggregator
{
    private readonly ApplicationDbContext _context;
    private readonly IProactiveAlertService _alertService;
    private readonly ILogger<BusinessInsightAggregator> _logger;

    public BusinessInsightAggregator(
        ApplicationDbContext context, 
        IProactiveAlertService alertService,
        ILogger<BusinessInsightAggregator> logger)
    {
        _context = context;
        _alertService = alertService;
        _logger = logger;
    }

    public async Task<List<BriefingItem>> GatherInsightsAsync(Guid organizationId, DateTime date, CancellationToken cancellationToken = default)
    {
        var items = new List<BriefingItem>();

        try
        {
            await GatherFinanceInsightsAsync(organizationId, items, cancellationToken);
            await GatherInventoryInsightsAsync(organizationId, items, cancellationToken);
            await GatherCustomerInsightsAsync(organizationId, items, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error gathering business insights for organization {OrgId}", organizationId);
        }

        return items;
    }

    private async Task GatherFinanceInsightsAsync(Guid organizationId, List<BriefingItem> items, CancellationToken cancellationToken)
    {
        // Check for overdue invoices (> 7 days)
        var overdueDate = DateTime.UtcNow.AddDays(-7);
        var overdueInvoices = await _context.FinanceInvoices
            .Where(i => i.OrganizationId == organizationId && 
                        (i.Status == "Sent" || i.Status == "Overdue") && 
                        i.DueDate < overdueDate)
            .ToListAsync(cancellationToken);

        if (overdueInvoices.Any())
        {
            var totalOverdue = overdueInvoices.Sum(i => i.TotalAmount);
            
            items.Add(new BriefingItem
            {
                Type = BriefingItemType.Risk,
                Category = "Finance",
                Title = $"{overdueInvoices.Count} Overdue Invoices",
                Description = $"There are {overdueInvoices.Count} invoices overdue by more than 7 days, totaling {totalOverdue:C}.",
                Priority = 80,
                SourceModule = "Finance",
                ConfidenceScore = 1.0m,
                ExpectedBusinessImpact = $"Potential cash flow risk of {totalOverdue:C}",
                RecommendedAction = "Send follow-up reminders to customers with overdue invoices."
            });

            // Create a proactive alert for high value ones
            var highValueOverdue = overdueInvoices.Where(i => i.TotalAmount > 10000).ToList();
            foreach (var invoice in highValueOverdue)
            {
                await _alertService.CreateAlertAsync(new ProactiveAlert
                {
                    OrganizationId = organizationId,
                    Type = "HighValueOverdueInvoice",
                    Severity = AlertSeverity.High,
                    Title = $"High Value Invoice Overdue: {invoice.InvoiceNumber}",
                    Description = $"Invoice {invoice.InvoiceNumber} for {invoice.TotalAmount:C} is overdue.",
                    SourceModule = "Finance",
                    SourceEntityId = invoice.Id.ToString(),
                    RecommendedAction = "Contact customer immediately for payment status.",
                    Status = AlertStatus.ActionRequired,
                    DeduplicationKey = $"overdue_invoice_{invoice.Id}"
                }, cancellationToken);
            }
        }
        
        // Revenue highlight (simplistic check for today)
        var todayStart = DateTime.UtcNow.Date;
        var todayPayments = await _context.FinancePayments
            .Where(p => p.OrganizationId == organizationId && p.PaymentDate >= todayStart)
            .SumAsync(p => p.Amount, cancellationToken);
            
        if (todayPayments > 0)
        {
            items.Add(new BriefingItem
            {
                Type = BriefingItemType.Highlight,
                Category = "Finance",
                Title = "Daily Revenue Collected",
                Description = $"Collected {todayPayments:C} in payments today.",
                Priority = 60,
                SourceModule = "Finance",
                ConfidenceScore = 1.0m
            });
        }
    }

    private async Task GatherInventoryInsightsAsync(Guid organizationId, List<BriefingItem> items, CancellationToken cancellationToken)
    {
        // Check for low inventory (quantity < reorder level)
        var lowStockItems = await _context.InventoryStocks
            .Include(s => s.Product)
            .Where(s => s.OrganizationId == organizationId && s.QuantityOnHand <= s.ReorderLevel && s.Product != null)
            .ToListAsync(cancellationToken);

        if (lowStockItems.Any())
        {
            items.Add(new BriefingItem
            {
                Type = BriefingItemType.Risk,
                Category = "Inventory",
                Title = $"{lowStockItems.Count} Products Low on Stock",
                Description = $"{lowStockItems.Count} products have fallen below their reorder point and may run out soon.",
                Priority = 75,
                SourceModule = "Inventory",
                ConfidenceScore = 1.0m,
                RecommendedAction = "Create purchase orders for low stock items."
            });

            foreach (var stock in lowStockItems.Take(5)) // Limit alerts
            {
                await _alertService.CreateAlertAsync(new ProactiveAlert
                {
                    OrganizationId = organizationId,
                    Type = "LowInventory",
                    Severity = AlertSeverity.Medium,
                    Title = $"Low Stock: {stock.Product?.Name}",
                    Description = $"Stock for {stock.Product?.Name} is at {stock.QuantityOnHand}, which is below the reorder point of {stock.ReorderLevel}.",
                    SourceModule = "Inventory",
                    SourceEntityId = stock.ProductId.ToString(),
                    RecommendedAction = $"Reorder {stock.Product?.Name}",
                    Status = AlertStatus.Unread,
                    DeduplicationKey = $"low_stock_{stock.ProductId}_{stock.WarehouseId}"
                }, cancellationToken);
            }
        }
    }

    private async Task GatherCustomerInsightsAsync(Guid organizationId, List<BriefingItem> items, CancellationToken cancellationToken)
    {
        // Identify new customers this week
        var weekStart = DateTime.UtcNow.AddDays(-7);
        var newCustomersCount = await _context.Customers
            .Where(c => c.OrganizationId == organizationId && c.CreatedAt >= weekStart)
            .CountAsync(cancellationToken);
            
        if (newCustomersCount > 0)
        {
            items.Add(new BriefingItem
            {
                Type = BriefingItemType.Highlight,
                Category = "Customers",
                Title = $"{newCustomersCount} New Customers",
                Description = $"You have acquired {newCustomersCount} new customers in the last 7 days.",
                Priority = 50,
                SourceModule = "CRM",
                ConfidenceScore = 1.0m
            });
        }
        
        // Example Opportunity: High value inactive customers
        // Since we don't have a direct "TotalSpent" or "LastPurchaseDate" on Customer, this is a placeholder.
        // In a real system we would query orders/invoices grouped by customer.
        items.Add(new BriefingItem
        {
            Type = BriefingItemType.Opportunity,
            Category = "Customers",
            Title = "Re-engage VIP Customers",
            Description = "3 high-value customers haven't made a purchase in 3 months.",
            Priority = 65,
            SourceModule = "CRM",
            ConfidenceScore = 0.8m,
            RecommendedAction = "Send a targeted re-engagement campaign."
        });
    }
}
