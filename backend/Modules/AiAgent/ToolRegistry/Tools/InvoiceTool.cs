using backend.Entities;
using backend.Modules.AiAgent.ToolRegistry.Interfaces;
using backend.Modules.AiAgent.ToolRegistry.Models;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.AiAgent.ToolRegistry.Tools;

public class InvoiceTool : ITool
{
    private readonly ApplicationDbContext _context;

    public InvoiceTool(ApplicationDbContext context)
    {
        _context = context;
    }

    public string Name => "InvoiceTool";
    public string Description => "Billing & Invoice Tool: create invoices and check unpaid/outstanding balance.";
    public string Category => "Billing";
    public string[] RequiredPermissions => new[] { "Billing.View", "Billing.Create" };
    public bool IsDestructive => false;
    public string ParametersSchema => "{\"type\":\"object\",\"properties\":{\"action\":{\"type\":\"string\",\"enum\":[\"create_invoice\",\"show_unpaid_invoices\"]},\"amount\":{\"type\":\"number\"},\"number\":{\"type\":\"string\"}}}";

    public async Task<ToolResult> ExecuteAsync(
        ToolExecutionContext context,
        IDictionary<string, object> parameters,
        CancellationToken cancellationToken = default)
    {
        var action = parameters.TryGetValue("action", out var actionObj) ? actionObj?.ToString()?.ToLower() : "show_unpaid_invoices";

        switch (action)
        {
            case "create_invoice":
                {
                    var amount = 100.0m;
                    if (parameters.TryGetValue("amount", out var amtObj) && decimal.TryParse(amtObj?.ToString(), out var parsedAmt))
                    {
                        amount = parsedAmt;
                    }

                    var invoiceNumber = parameters.TryGetValue("number", out var numObj)
                        ? numObj?.ToString()
                        : $"INV-{Random.Shared.Next(1000, 9999)}";

                    var invoice = new Invoice
                    {
                        Id = Guid.NewGuid(),
                        OrganizationId = context.OrganizationId,
                        Number = invoiceNumber ?? $"INV-{Random.Shared.Next(1000, 9999)}",
                        Amount = amount,
                        Status = "Pending",
                        PdfUrl = $"/invoices/{invoiceNumber}.pdf"
                    };

                    await _context.Invoices.AddAsync(invoice, cancellationToken);
                    await _context.SaveChangesAsync(cancellationToken);

                    return ToolResult.Ok($"Invoice #{invoice.Number} generated for ${invoice.Amount:F2}.", new { invoice.Id, invoice.Number, invoice.Amount, invoice.Status });
                }

            case "show_unpaid_invoices":
            default:
                {
                    var unpaidInvoices = await _context.Invoices
                        .AsNoTracking()
                        .Where(i => i.OrganizationId == context.OrganizationId && i.Status != "Paid" && !i.IsDeleted)
                        .OrderByDescending(i => i.CreatedAt)
                        .Take(10)
                        .Select(i => new { i.Id, i.Number, i.Amount, i.Status, i.CreatedAt })
                        .ToListAsync(cancellationToken);

                    var totalOutstanding = unpaidInvoices.Sum(i => i.Amount);

                    return ToolResult.Ok($"Found {unpaidInvoices.Count} unpaid invoices totaling ${totalOutstanding:F2}.", new { TotalOutstanding = totalOutstanding, Invoices = unpaidInvoices });
                }
        }
    }
}
