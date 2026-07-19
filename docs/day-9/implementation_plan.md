# WhatsApp Business API Integration & Campaign Management (Day 9)

This plan outlines the steps to implement Day 9 tasks for BusinessOS AI: 
1. Adding WhatsApp Business API integration.
2. Building campaign management and bulk messaging UI.

## User Review Required

Please review the proposed architectural changes and the frontend pages we'll be adding. The integration requires connecting to the Meta/WhatsApp Cloud API.

## Open Questions
> [!IMPORTANT]
> 1. **Testing Environment**: Do you currently have a WhatsApp test number and Meta Developer App set up for testing the Cloud API? If not, we will mock the API calls during development.
> 2. **Contact Management**: We have a `Contact` entity. Should we build a dedicated `whatsapp/contacts` page to manage contacts (import, add, edit), or is this handled elsewhere?
> 3. **Template Sync**: Should we allow creating new templates from our dashboard (which requires sending them to Meta for approval), or just sync/read existing templates that are already approved in the Meta Business Manager?

## Proposed Changes

---

### Backend: Database & Entities

We need to add data structures to support WhatsApp settings and Campaign tracking.

#### [NEW] `backend/Entities/WhatsAppSettings.cs`
- Store Meta App settings for each organization: `OrganizationId`, `PhoneNumberId`, `BusinessAccountId`, `AccessToken`, `WebhookVerifyToken`.

#### [NEW] `backend/Entities/WhatsAppTemplate.cs`
- Store synchronized approved templates: `Name`, `Language`, `Category`, `Components` (JSON), `Status`.

#### [MODIFY] `backend/Entities/Campaign.cs`
- Add properties: `Status` (Draft, Scheduled, Running, Completed), `Type` (WhatsApp), `ScheduledAt`, `TemplateId`.
- Add metrics: `TotalMessages`, `SentMessages`, `DeliveredMessages`, `ReadMessages`, `FailedMessages`.

#### [NEW] `backend/Entities/CampaignContact.cs`
- Join table to link a `Campaign` with multiple `Contact` entities.

#### [MODIFY] `backend/Persistence/ApplicationDbContext.cs`
- Add `DbSet` properties for `WhatsAppSettings`, `WhatsAppTemplate`, and `CampaignContact`.

---

### Backend: Services & API

#### [NEW] `backend/Services/WhatsAppService.cs` (and `IWhatsAppService.cs`)
- Handles direct communication with Meta's WhatsApp Cloud API.
- Methods: `SendMessageAsync`, `SyncTemplatesAsync`, `ProcessWebhookAsync`.

#### [NEW] `backend/Controllers/WhatsAppController.cs`
- Endpoints for Settings CRUD.
- Webhook endpoint for receiving message delivery status from Meta.
- Endpoint to trigger template sync.

#### [NEW] `backend/Controllers/CampaignController.cs`
- Endpoints to create, update, list campaigns.
- Endpoint to launch/schedule a campaign (triggers sending messages via `WhatsAppService`).

---

### Frontend: UI & Components

We will build out the `whatsapp` module in the dashboard.

#### [NEW] `frontend/app/dashboard/whatsapp/settings/page.tsx`
- Configuration form for users to input their Meta App credentials (Token, IDs).

#### [NEW] `frontend/app/dashboard/whatsapp/templates/page.tsx`
- A data table showing synced WhatsApp templates and their approval status.
- Button to "Sync Templates from Meta".

#### [NEW] `frontend/app/dashboard/whatsapp/contacts/page.tsx` (Pending answer)
- UI to manage contacts and phone numbers for WhatsApp campaigns.

#### [MODIFY] `frontend/app/dashboard/whatsapp/page.tsx`
- Dashboard overview for WhatsApp metrics.
- List of past and ongoing campaigns with status badges and performance metrics (Sent/Delivered/Read).

#### [NEW] `frontend/app/dashboard/whatsapp/campaigns/create/page.tsx`
- A multi-step form to build a bulk messaging campaign:
  1. Campaign details (Name, Schedule).
  2. Select Contacts.
  3. Select WhatsApp Template.
  4. Preview and Launch.

## Verification Plan

### Automated Tests
- Run `.NET build` and `tsc` to verify no compilation errors.
- Basic integration tests if existing testing framework is in place.

### Manual Verification
- Verify the UI renders correctly across all new pages in the dashboard.
- Add mock settings and verify the frontend correctly calls the backend API to save settings.
- If Meta API keys are provided, we will verify template syncing and sending a test message to a verified test number.
