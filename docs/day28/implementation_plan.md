# Day 28: AI Automation Frontend Implementation

This plan outlines the frontend implementation for the AI Automation & Workflow Engine, specifically designed for non-technical business owners.

## Open Questions

> [!IMPORTANT]  
> The existing codebase contains an `/app/dashboard/automation` directory and a `useAutomationStudio.ts` hook. Since the prompt specifies to create `/automation`, I will create the new dashboard at `/app/automation` and ignore/replace the old one to ensure a clean slate that matches your exact requirements. Is this acceptable, or should I replace the contents of `/app/dashboard/automation` directly? 
> **Recommendation:** Put it at `/app/automation` and wrap it with `DashboardShell` to meet the strict requirement of "Create: /automation".

## Proposed Changes

### Core API Client & Hooks
#### [NEW] [automation.ts](file:///Users/pankajanilyadav/Documents/Simplify/frontend/lib/api/automation.ts)
Create the API client connecting to:
- Workflows (GET, GET by ID, POST, PUT, DELETE)
- State management (activate, deactivate, test)
- Executions (GET, GET by ID)
- Templates (GET, install)
- Approvals (integrate with Action Center)

#### [NEW] [useAiAutomation.ts](file:///Users/pankajanilyadav/Documents/Simplify/frontend/hooks/useAiAutomation.ts)
React Query hooks wrapping the `automation.ts` API client. Includes cache invalidation on mutations and toast notifications.

---

### UI Components
#### [NEW] [ai-automation-dashboard.tsx](file:///Users/pankajanilyadav/Documents/Simplify/frontend/components/automation/ai-automation-dashboard.tsx)
The main dashboard view showing active automations, executed actions, business impact metrics, and recent execution history.

#### [NEW] [workflow-wizard.tsx](file:///Users/pankajanilyadav/Documents/Simplify/frontend/components/automation/workflow-wizard.tsx)
A guided 4-step wizard for creating automations:
1. Trigger selection
2. Action selection
3. Optional conditions (simple UI, no JSON)
4. Approval configuration

#### [NEW] [ai-workflow-assistant.tsx](file:///Users/pankajanilyadav/Documents/Simplify/frontend/components/automation/ai-workflow-assistant.tsx)
The "Describe an automation in plain English" feature that converts natural language into a proposed workflow configuration.

#### [NEW] [workflow-preview.tsx](file:///Users/pankajanilyadav/Documents/Simplify/frontend/components/automation/workflow-preview.tsx)
A visual node-like display (but highly simplified for business owners) showing the Trigger → Condition → AI Decision → Action → Result flow.

#### [NEW] [automation-templates.tsx](file:///Users/pankajanilyadav/Documents/Simplify/frontend/components/automation/automation-templates.tsx)
Display grid of the 4 requested templates (Overdue Payments, Inactive Customers, Stockouts, Lead Follow-up) with installation buttons.

---

### Pages
#### [NEW] [layout.tsx](file:///Users/pankajanilyadav/Documents/Simplify/frontend/app/automation/layout.tsx)
Wrap the automation routes in the `DashboardShell` and `ProtectedRoute`.

#### [NEW] [page.tsx](file:///Users/pankajanilyadav/Documents/Simplify/frontend/app/automation/page.tsx)
The main Automation Dashboard route.

#### [NEW] [page.tsx](file:///Users/pankajanilyadav/Documents/Simplify/frontend/app/automation/create/page.tsx)
The route hosting the `workflow-wizard` and `ai-workflow-assistant`.

#### [NEW] [page.tsx](file:///Users/pankajanilyadav/Documents/Simplify/frontend/app/automation/[id]/page.tsx)
Workflow details page showing overview, visual preview, execution history, and business impact.

## Verification Plan

### Automated Tests
- Run `npm run lint` and TypeScript compilation (`npx tsc --noEmit`) to verify strict adherence to typing rules.
- Run `npm run build` to verify Next.js production build succeeds.

### Manual Verification
- Test creating an automation via the 4-step wizard.
- Test activating and deactivating an automation.
- Test generating an automation via the AI Assistant input.
- Verify that risky actions correctly prompt for confirmation in the UI.
- Verify responsive layout on mobile viewport.
