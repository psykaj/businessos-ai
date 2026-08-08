using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using backend.Persistence;
using backend.Modules.Automation.Domain.Entities;
using backend.Modules.Automation.Domain.Enums;

namespace backend.Modules.Automation.Seed;

public static class AutomationEngineSeeder
{
    public static async Task SeedTemplatesAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var templates = new[]
        {
            new WorkflowTemplate
            {
                Name = "Recover Overdue Payments",
                Description = "Triggered when an invoice is overdue by 7 days. Checks if amount > threshold and creates a payment reminder.",
                Category = "Finance",
                TriggerType = WorkflowTriggerType.InvoiceOverdue.ToString(),
                Configuration = @"{
                  ""Steps"": [
                    {
                      ""StepOrder"": 1,
                      ""StepType"": ""Condition"",
                      ""Name"": ""Check Amount"",
                      ""Condition"": ""{\""logic\"": \""AND\"", \""rules\"": [{\""field\"": \""Amount\"", \""operator\"": \"">\"", \""value\"": 1000}]}""
                    },
                    {
                      ""StepOrder"": 2,
                      ""StepType"": ""Action"",
                      ""Name"": ""Create Payment Reminder"",
                      ""Configuration"": ""{\""ActionType\"": \""CreateTask\"", \""TaskName\"": \""Follow up on overdue payment\""}""
                    }
                  ]
                }"
            },
            new WorkflowTemplate
            {
                Name = "Recover Inactive Customers",
                Description = "Triggered when customer is inactive for 30 days. Uses AI to decide on retention offer.",
                Category = "Customer Success",
                TriggerType = WorkflowTriggerType.CustomerInactive.ToString(),
                Configuration = @"{
                  ""Steps"": [
                    {
                      ""StepOrder"": 1,
                      ""StepType"": ""AIDecision"",
                      ""Name"": ""Determine Offer Eligibility""
                    },
                    {
                      ""StepOrder"": 2,
                      ""StepType"": ""Action"",
                      ""Name"": ""Create Retention Campaign"",
                      ""Configuration"": ""{\""ActionType\"": \""CreateTask\"", \""TaskName\"": \""Send Retention Offer\""}""
                    }
                  ]
                }"
            },
            new WorkflowTemplate
            {
                Name = "Prevent Stockout",
                Description = "Triggered when inventory drops below reorder level.",
                Category = "Inventory",
                TriggerType = WorkflowTriggerType.InventoryLow.ToString(),
                Configuration = @"{
                  ""Steps"": [
                    {
                      ""StepOrder"": 1,
                      ""StepType"": ""Action"",
                      ""Name"": ""Create Reorder Task"",
                      ""Configuration"": ""{\""ActionType\"": \""CreateTask\"", \""TaskName\"": \""Reorder Stock\""}""
                    }
                  ]
                }"
            },
            new WorkflowTemplate
            {
                Name = "New Lead Follow-up",
                Description = "Triggered when a new lead is created.",
                Category = "Sales",
                TriggerType = WorkflowTriggerType.NewLeadCreated.ToString(),
                Configuration = @"{
                  ""Steps"": [
                    {
                      ""StepOrder"": 1,
                      ""StepType"": ""Action"",
                      ""Name"": ""Create Follow-up Task"",
                      ""Configuration"": ""{\""ActionType\"": \""CreateTask\"", \""TaskName\"": \""Call new lead\""}""
                    }
                  ]
                }"
            }
        };

        foreach (var template in templates)
        {
            var exists = await context.AiEngineWorkflowTemplates.AnyAsync(t => t.Name == template.Name);
            if (!exists)
            {
                context.AiEngineWorkflowTemplates.Add(template);
            }
        }

        await context.SaveChangesAsync();
    }
}
