# Workflow Automation Engine Architecture & APIs

The Workflow Automation Engine is an enterprise-grade backend module for BusinessOS AI comparable to Zapier, Make.com, n8n, and Microsoft Power Automate.

## Architecture

- **Clean Architecture & CQRS**: Clean separation into Controllers, Services, Repositories, Entities, DTOs, Handlers, and Registries.
- **Multi-Tenant Isolation**: Every workflow and execution is scoped to an `OrganizationId`.
- **Plugin Registry Pattern**: Triggers, Actions, and Integrations are registered via interface registries (`ITriggerHandler`, `IActionHandler`, `IIntegrationProvider`) to allow adding future plugins without touching core execution logic.

## Key APIs

- `GET /api/workflows`: List workflows with search, pagination, and status filters.
- `GET /api/workflows/{id}`: Get workflow details by ID.
- `POST /api/workflows`: Create a new workflow with triggers, actions, and conditions.
- `PUT /api/workflows/{id}`: Update an existing workflow.
- `DELETE /api/workflows/{id}`: Soft delete a workflow.
- `POST /api/workflows/{id}/execute`: Trigger manual execution with custom payload.
