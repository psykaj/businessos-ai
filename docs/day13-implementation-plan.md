# Implementation Plan - Day 13 Frontend: Workflow Automation, Integrations & Business Automation Center

Build a complete, no-code **Workflow Automation Center** frontend for BusinessOS AI (comparable to Zapier, Make.com, n8n, Microsoft Power Automate) integrated with backend APIs.

## Business Value Principle Checklist
- ✅ Saves hundreds of hours of manual labor by automating cross-application workflows.
- ✅ Increases revenue through instant lead follow-up and automated payment reminders.
- ✅ Reduces operational cost by replacing third-party integration tools.
- ✅ Improves decision-making through real-time execution logs and AI-assisted workflow generation.

---

## Proposed Changes

### Pages & Navigation

#### [NEW] Pages in `frontend/app/dashboard/`
1. `workflows/page.tsx` - Workflow Dashboard with metrics, search, filter, actions.
2. `workflows/create/page.tsx` - Visual Workflow Builder (New Workflow).
3. `workflows/[id]/page.tsx` - Workflow Detail View & Status.
4. `workflows/edit/[id]/page.tsx` - Visual Workflow Builder (Edit Mode).
5. `integrations/page.tsx` - Integration Center with provider grid, status, connect modals.
6. `integrations/[provider]/page.tsx` - Provider Connection Detail & Settings.
7. `workflow-history/page.tsx` - Execution History & Real-time Log Monitor.
8. `templates/workflows/page.tsx` - Pre-built Business Workflow Template Library.

#### [MODIFY] [sidebar.tsx](file:///Users/pankajanilyadav/Documents/BusinessOS-AI/frontend/components/layout/sidebar.tsx)
- Add "Workflows", "Integrations", "Execution History", and "Templates" under the Automation section in sidebar navigation.

---

### Core Frontend Components

#### [NEW] Visual Workflow Builder (`frontend/components/workflows/builder/`)
- `workflow-canvas.tsx` - React Flow canvas (`@xyflow/react`) with custom node types, drag-and-drop, connecting, grid snapping, zoom, minimap.
- `trigger-node.tsx` - Trigger node component (`Lead Created`, `Form Submitted`, `QR Code Scanned`, `Payment Received`, `Customer Registered`, `Subscription Expiring`, `Manual Trigger`, `Scheduled Trigger`).
- `action-node.tsx` - Action node component (`Send Email`, `Send WhatsApp`, `Create CRM Task`, `Update CRM Lead`, `Assign Salesperson`, `Send Notification`, `Generate Invoice`, `Generate QR Code`, `Call AI Assistant`, `Call Webhook`).
- `logic-node.tsx` - Logic node component (`IF / ELSE`, `AND / OR`, `Delay`, `Filter`, `Switch`, `Merge`).
- `node-palette.tsx` - Sidebar palette to drag and drop nodes into canvas.
- `node-config-drawer.tsx` - Drawer to configure trigger/action/logic parameters and variable tags.
- `workflow-toolbar.tsx` - Undo/Redo, Auto-save indicator, Zoom, Validate, Publish, AI Assistant trigger.

#### [NEW] AI Workflow Assistant (`frontend/components/workflows/ai-assistant.tsx`)
- AI drawer/modal allowing natural language prompts (e.g., *"When a new lead is created, assign it to John and send a welcome WhatsApp"*).
- Generates suggested graph structure and auto-populates the React Flow canvas.

#### [NEW] Integration Center (`frontend/components/integrations/`)
- `integration-card.tsx` - Status badge, provider logo, test connection button, connect modal.
- `connect-integration-modal.tsx` - Encrypted key input modal (`ApiKey`, `AccessToken`) with health check verification.

#### [NEW] Workflow Templates (`frontend/components/workflows/templates/`)
- `template-card.tsx` - Preview workflow graph, template description, single-click install.

---

### API Client & React Query Custom Hooks

#### [NEW] API Services & Hooks
- `frontend/lib/api/workflows.ts` - Axios endpoints for workflows, triggers, actions, integrations, executions, logs.
- `frontend/hooks/use-workflows.ts` - React Query hooks for fetching, creating, updating, deleting, executing workflows.
- `frontend/hooks/use-integrations.ts` - React Query hooks for fetching, connecting, testing integrations.

---

### Documentation & Roadmap

#### [NEW] [docs/workflow-ui.md](file:///Users/pankajanilyadav/Documents/BusinessOS-AI/docs/workflow-ui.md)
- Complete frontend architecture, builder documentation, React Flow integration details.

#### [MODIFY] [roadmap.md](file:///Users/pankajanilyadav/Documents/BusinessOS-AI/roadmap.md)
- Update roadmap with completed status for Workflow Automation UI, Visual Workflow Builder, Integration Center, Templates, History, and AI Assistant.

---

## Verification Plan

### Automated Verification
- `npm run build` in `frontend/` to ensure zero TypeScript and Next.js compilation errors.
- `npm run lint` in `frontend/` to ensure clean code style.

### Functional Verification
- Verify visual node addition, connection, deletion, undo/redo, auto-save, and graph validation.
- Verify template installation into workflow builder.
- Verify integration connection modal and test status check.
- Verify execution history log viewer.
- Verify AI assistant workflow generation from natural language prompt.
