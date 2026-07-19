# Implementation Plan: Day 10 Backend (AI Assistant & Automation)

This document outlines the architecture and implementation steps for building the AI Communication & Automation Backend for BusinessOS AI.

## User Review Required
> [!IMPORTANT]
> The requested implementation is extremely comprehensive and involves many moving parts. Please review the architectural decisions below. Once approved, I will execute the plan step-by-step. 

## Open Questions
> [!NOTE]
> 1. **SignalR Configuration**: Do you have a preferred SignalR Hub configuration (e.g., Redis backplane) or should I implement standard local memory scaling for now?
> 2. **WhatsApp Refactor**: In the previous task, I created `WhatsAppSettings` and `WhatsAppTemplate` in the core `Entities/` folder. Should I move them into the new `Modules/WhatsApp` pattern to align with your request?
> 3. **AI Packages**: Should I use the official `Azure.AI.OpenAI` package for the OpenAI abstraction, or do you have another preferred package?
> 4. **Multi-Tenancy Filtering**: Should the new entities explicitly include `OrganizationId` global query filters in the `ApplicationDbContext`? (Assuming yes, as per enterprise multi-tenant standards).

## Architecture & Proposed Changes

### 1. Project Structure & Modules
We will create new feature modules under `backend/Modules/`:
- `AI/`
- `Notifications/`
- `Email/`
- `WhatsApp/` (Will refactor existing services here)
- `Templates/`
- `Automation/`

Each module will follow the standard pattern: `Controllers/`, `DTOs/`, `Interfaces/`, `Repositories/`, and `Services/`.

### 2. Database Entities & EF Core
We will create the following entities in `backend/Entities/` (or within their respective module domain folders if that's the preferred pattern, but standard is `backend/Entities/`):
- **AI**: `AIConversation`, `AIMessage`
- **Notifications**: `Notification`
- **Email**: `EmailTemplate`
- **WhatsApp**: `WhatsAppTemplate` (Update if needed)
- **Automation**: `AutomationRule`, `AutomationLog`

We will configure `ApplicationDbContext` to include these `DbSet` properties, add `OrganizationId` global query filters, and create the EF Core migration.

### 3. AI Provider Abstraction
- Define an `IAIService` interface with methods like `ChatCompletionAsync`.
- Implement a `ProviderFactory` or strategy pattern to select between OpenAI, Gemini, and Claude based on organization settings or requests.
- Implement the `OpenAIAIService` (default provider) tracking token usage.

### 4. Communication Modules (Email & WhatsApp)
- **Email Module**: Create `IEmailService` for standard SMTP sending, with potential expansions for SendGrid/SES. Add endpoints for scheduling and bulk sending.
- **WhatsApp Module**: Migrate the existing `WhatsAppService` to the new module format, adding media message support and scheduling.

### 5. Notification Center
- Build a generic `NotificationService` that handles in-app database persistence and routes messages to SignalR (for real-time), Email, or WhatsApp based on the user's preferences.

### 6. Automation Engine
- Implement an Event-Driven Architecture.
- Define triggers (e.g., "SubscriptionPurchased") and a generic event bus (e.g., using `MediatR` or standard C# events).
- Create an `AutomationEngineService` that evaluates `AutomationRule` conditions when events are fired and executes the defined actions (Send Email, Call AI, etc.).

### 7. SignalR Hub
- Create a `NotificationHub` mapped to `/hubs/notifications`.
- Authenticate the hub connections using JWT tokens.
- Add methods to join organization-specific groups for secure, multi-tenant broadcasts.

### 8. Repository Layer
- Create `IAIRepository`, `INotificationRepository`, `IEmailRepository`, `IWhatsAppRepository`, and `IAutomationRepository` following the generic repository pattern, with specific filtering, sorting, and pagination logic.

### 9. Documentation
- Generate the requested markdown documentation files in `docs/`.

## Verification Plan
1. **Compilation**: Verify the backend builds with `dotnet build`.
2. **Database**: Verify `dotnet ef migrations add` and `dotnet ef database update` succeed.
3. **API Validation**: I will provide curl commands or suggest Swagger endpoints to test the newly created APIs.
4. **Code Quality**: Ensure no duplicate code and that the Clean Architecture pattern is maintained.
