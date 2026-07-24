using backend.Modules.AiAgent.CommandEngine.Interfaces;
using backend.Modules.AiAgent.CommandEngine.Models;
using backend.Modules.AiAgent.Context.Models;
using backend.Modules.AiAgent.ToolRegistry.Interfaces;
using Microsoft.Extensions.Logging;

namespace backend.Modules.AiAgent.CommandEngine.Services;

public class AiCommandEngine : IAiCommandEngine
{
    private readonly IToolRegistry _toolRegistry;
    private readonly ILogger<AiCommandEngine> _logger;

    public AiCommandEngine(IToolRegistry toolRegistry, ILogger<AiCommandEngine> logger)
    {
        _toolRegistry = toolRegistry;
        _logger = logger;
    }

    public Task<CommandParseResult> ParseCommandAsync(
        string userPrompt,
        AiContext context,
        CancellationToken cancellationToken = default)
    {
        var lowerPrompt = userPrompt.Trim().ToLowerInvariant();
        var result = new CommandParseResult();

        // 1. Revenue & Finance Intent
        if (lowerPrompt.Contains("revenue") || lowerPrompt.Contains("sales total") || lowerPrompt.Contains("how much money"))
        {
            result.ToolName = "AnalyticsTool";
            result.Intent = "show_revenue";
            result.Parameters["action"] = "show_revenue";
            result.Explanation = "Parsed request for Organization Revenue metrics.";
            result.Confidence = 0.95;
            return Task.FromResult(result);
        }

        // 2. Executive Dashboard Summary
        if (lowerPrompt.Contains("dashboard") || lowerPrompt.Contains("overview") || lowerPrompt.Contains("business health"))
        {
            result.ToolName = "AnalyticsTool";
            result.Intent = "show_dashboard";
            result.Parameters["action"] = "show_dashboard";
            result.Explanation = "Parsed request for Executive Dashboard metrics.";
            result.Confidence = 0.95;
            return Task.FromResult(result);
        }

        // 3. Invoice Management
        if (lowerPrompt.Contains("create invoice") || lowerPrompt.Contains("generate invoice") || lowerPrompt.Contains("new invoice"))
        {
            result.ToolName = "InvoiceTool";
            result.Intent = "create_invoice";
            result.Parameters["action"] = "create_invoice";
            
            // Extract amount if present (e.g., "$500" or "500")
            var numbers = System.Text.RegularExpressions.Regex.Matches(userPrompt, @"\d+(\.\d+)?");
            if (numbers.Count > 0 && decimal.TryParse(numbers[0].Value, out var parsedAmount))
            {
                result.Parameters["amount"] = parsedAmount;
            }

            result.Explanation = "Parsed request to create a new customer invoice.";
            result.Confidence = 0.92;
            return Task.FromResult(result);
        }

        if (lowerPrompt.Contains("unpaid invoice") || lowerPrompt.Contains("outstanding invoice") || lowerPrompt.Contains("pending invoice") || lowerPrompt.Contains("invoice status"))
        {
            result.ToolName = "InvoiceTool";
            result.Intent = "show_unpaid_invoices";
            result.Parameters["action"] = "show_unpaid_invoices";
            result.Explanation = "Parsed request for unpaid/outstanding invoices.";
            result.Confidence = 0.95;
            return Task.FromResult(result);
        }

        // 4. Lead Assignment & CRM
        if (lowerPrompt.Contains("assign lead") || lowerPrompt.Contains("assign new leads") || lowerPrompt.Contains("assign leads to"))
        {
            result.ToolName = "CrmTool";
            result.Intent = "assign_lead";
            result.Parameters["action"] = "assign_lead";

            // Extract assignee name (e.g. "assign leads to Rahul")
            if (lowerPrompt.Contains("to "))
            {
                var idx = lowerPrompt.IndexOf("to ");
                var assigneeName = userPrompt[(idx + 3)..].Trim().Split(' ')[0];
                if (!string.IsNullOrWhiteSpace(assigneeName))
                {
                    result.Parameters["assigneeName"] = assigneeName;
                }
            }

            result.Explanation = "Parsed request to assign lead to team member.";
            result.Confidence = 0.90;
            return Task.FromResult(result);
        }

        if (lowerPrompt.Contains("pending task") || lowerPrompt.Contains("my tasks") || lowerPrompt.Contains("todo list"))
        {
            result.ToolName = "CrmTool";
            result.Intent = "show_pending_tasks";
            result.Parameters["action"] = "show_pending_tasks";
            result.Explanation = "Parsed request for pending CRM tasks.";
            result.Confidence = 0.95;
            return Task.FromResult(result);
        }

        if (lowerPrompt.Contains("schedule follow") || lowerPrompt.Contains("follow up") || lowerPrompt.Contains("set reminder"))
        {
            result.ToolName = "CrmTool";
            result.Intent = "schedule_followup";
            result.Parameters["action"] = "schedule_followup";
            result.Parameters["title"] = userPrompt;
            result.Explanation = "Parsed request to schedule follow-up task.";
            result.Confidence = 0.90;
            return Task.FromResult(result);
        }

        // 5. Customer Insights & Inactive Leads
        if (lowerPrompt.Contains("haven't been contacted") || lowerPrompt.Contains("inactive customer") || lowerPrompt.Contains("inactive lead") || lowerPrompt.Contains("search customer") || lowerPrompt.Contains("find contact"))
        {
            result.ToolName = "CustomerTool";
            result.Intent = lowerPrompt.Contains("inactive") || lowerPrompt.Contains("contacted") ? "show_inactive_customers" : "search_customer";
            result.Parameters["action"] = result.Intent;
            result.Parameters["searchTerm"] = userPrompt;
            result.Explanation = "Parsed request for customer search or inactive lead discovery.";
            result.Confidence = 0.90;
            return Task.FromResult(result);
        }

        // 6. Communication (Email / WhatsApp)
        if (lowerPrompt.Contains("send email") || lowerPrompt.Contains("payment reminder") || lowerPrompt.Contains("email reminder"))
        {
            result.ToolName = "EmailTool";
            result.Intent = "send_email";
            result.Parameters["subject"] = "Payment Reminder";
            result.Parameters["body"] = userPrompt;
            result.Explanation = "Parsed request to dispatch email communication.";
            result.Confidence = 0.90;
            return Task.FromResult(result);
        }

        if (lowerPrompt.Contains("whatsapp") || lowerPrompt.Contains("send text"))
        {
            result.ToolName = "WhatsAppTool";
            result.Intent = "send_whatsapp";
            result.Parameters["message"] = userPrompt;
            result.Explanation = "Parsed request to send WhatsApp message.";
            result.Confidence = 0.90;
            return Task.FromResult(result);
        }

        // 7. Reports
        if (lowerPrompt.Contains("report") || lowerPrompt.Contains("generate report") || lowerPrompt.Contains("this week's report"))
        {
            result.ToolName = "ReportTool";
            result.Intent = "generate_report";
            result.Parameters["reportType"] = "Weekly Performance";
            result.Explanation = "Parsed request to generate executive performance report.";
            result.Confidence = 0.95;
            return Task.FromResult(result);
        }

        // 8. QR Code Generation
        if (lowerPrompt.Contains("qr code") || lowerPrompt.Contains("generate qr") || lowerPrompt.Contains("create qr"))
        {
            result.ToolName = "QRTool";
            result.Intent = "create_qr";
            result.Parameters["name"] = "Business QR";
            result.Explanation = "Parsed request to generate QR code.";
            result.Confidence = 0.95;
            return Task.FromResult(result);
        }

        // 9. Workflow Automation
        if (lowerPrompt.Contains("workflow") || lowerPrompt.Contains("trigger automation") || lowerPrompt.Contains("run workflow"))
        {
            result.ToolName = "WorkflowTool";
            result.Intent = "trigger_workflow";
            result.Parameters["action"] = "trigger";
            result.Explanation = "Parsed request to execute automated workflow.";
            result.Confidence = 0.90;
            return Task.FromResult(result);
        }

        // Default Fallback: Executive Dashboard summary via AnalyticsTool
        result.ToolName = "AnalyticsTool";
        result.Intent = "show_dashboard";
        result.Parameters["action"] = "show_dashboard";
        result.Explanation = "Generic business intent parsed to Executive Dashboard summary.";
        result.Confidence = 0.70;

        return Task.FromResult(result);
    }
}
