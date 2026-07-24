# Tool Registry Architecture

## Overview
The **Tool Registry** is a modular component in BusinessOS AI that exposes business functionality as callable tools for the AI Business Agent.

## Registered Tools
1. **CrmTool** (`CRM`): Assign leads, view pending tasks, schedule follow-ups.
2. **InvoiceTool** (`Billing`): Create invoices, retrieve unpaid/outstanding invoices.
3. **QRTool** (`Marketing`): Generate dynamic QR codes for URLs and landing pages.
4. **EmailTool** (`Communication`): Send customer emails and payment reminders.
5. **WhatsAppTool** (`Communication`): Send instant WhatsApp notifications.
6. **ReportTool** (`BusinessIntelligence`): Generate executive performance reports.
7. **WorkflowTool** (`Workflow`): List active workflows and trigger automated execution.
8. **AnalyticsTool** (`Analytics`): Retrieve today's revenue and executive dashboard metrics.
9. **CustomerTool** (`CRM`): Search customer contacts and identify inactive leads requiring re-engagement.

## Adding a New Tool
To register a new tool:
1. Implement the `ITool` interface in `backend/Modules/AiAgent/ToolRegistry/Tools/`.
2. Define `Name`, `Description`, `Category`, `RequiredPermissions`, `IsDestructive`, and `ParametersSchema`.
3. Register the tool in `AiAgentExtensions.cs` via Dependency Injection (`services.AddScoped<ITool, YourNewTool>();`).
4. The `ToolRegistry` automatically discovers and syncs tool definitions with the database.

## API Endpoints
- `GET /api/ai-agent/tools`: List all registered tools.
- `GET /api/ai-agent/tools/{name}`: Get detailed tool definition schema.
- `POST /api/ai-agent/tools/sync`: Synchronize tool definitions with database table.
