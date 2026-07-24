# AI Business Agent Architecture

## Overview
The **AI Business Agent** is an enterprise copilot for SMEs integrated into BusinessOS AI. It allows business owners to ask questions and execute business operations in natural language—such as viewing revenue, creating invoices, assigning leads, sending WhatsApp/email messages, running reports, and triggering workflows.

## Key Capabilities
- **Natural Language Command Processing**: Translates user prompts into actionable internal system commands.
- **Provider-Independent Engine**: Decoupled design allowing swapping between local fallback matchers, OpenAI, Anthropic Claude, or Google Gemini.
- **Multi-Tenant Safety & Permission Guard**: Strict validation of `OrganizationId` boundaries and user permissions before executing tools.
- **Destructive Action Safety**: Requires explicit confirmation (`isConfirmed=true`) for sensitive operations.
- **Auditable Task Execution Engine**: Every command execution is logged in the `CommandExecutions` database repository.
- **Conversation Memory**: Supports session-based and long-term conversation history management.
- **Proactive AI Recommendations**: Scans tenant data to suggest high-value business actions (e.g. inactive lead follow-ups, overdue invoices).

## API Endpoints
- `POST /api/ai-agent/execute`: Process natural language command and execute registered tools.
- `GET /api/ai-agent/status`: Health check & agent capabilities metadata.

## Execution Flow
1. User submits prompt to `POST /api/ai-agent/execute`.
2. `AiContextEngine` builds organization, user, and live business context.
3. `AiCommandEngine` identifies intent, parameters, and target tool.
4. `AiPermissionSafetyService` checks permissions, tenant boundaries, and safety flags.
5. `AiExecutionEngine` logs execution state (`Pending` -> `Running` -> `Success` / `Failed`).
6. `ToolRegistry` executes the targeted tool and returns structured result.
