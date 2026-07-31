# AI Automation Studio Frontend Implementation Plan

Build the complete AI Automation Studio Frontend, featuring a Visual Workflow Builder powered by React Flow, designed for business owners to automate processes without coding. 

## User Review Required

> [!WARNING]
> **API Integration**: The backend currently only has the `WorkflowTemplatesController` fully implemented for the new Day 22 entities. For the rest of the endpoints (CRUD for `AutomationWorkflows`, `Executions`), I will build the frontend using a robust mock API client inside `lib/api/automation-studio.ts`. This ensures the UI is fully functional and can be seamlessly connected to the real endpoints once they are added to the backend. Please confirm this approach.

> [!NOTE]
> **Dependencies**: I will install `framer-motion` to provide the smooth animations requested, as it is not currently in `package.json`. `@xyflow/react` (React Flow) is already installed.

## Open Questions

1. **Existing Workflow Module**: There is an existing `app/dashboard/workflows` and `app/dashboard/automation` from earlier days. I plan to overwrite or heavily modify `app/dashboard/automation` to become the new Automation Studio Hub. Is it okay to replace the old `automation/page.tsx`?

## Proposed Changes

### Dependencies
*   Run `npm install framer-motion`

### API & Hooks
*   **[NEW]** `lib/api/automation-studio.ts`: API client functions for workflows, templates, and execution history (using mocks where backend endpoints are missing).
*   **[NEW]** `hooks/useAutomationStudio.ts`: React Query hooks for data fetching, mutations, and caching.

### Pages (Next.js App Router)
*   **[MODIFY]** `app/dashboard/automation/page.tsx`: Overview dashboard (Stats, recent executions).
*   **[NEW]** `app/dashboard/automation/workflows/page.tsx`: List of user's workflows.
*   **[NEW]** `app/dashboard/automation/workflows/[id]/edit/page.tsx`: The Visual Workflow Builder.
*   **[NEW]** `app/dashboard/automation/templates/page.tsx`: Template library.
*   **[NEW]** `app/dashboard/automation/history/page.tsx`: Execution history table with filters.

### Visual Workflow Builder Components
*   **[NEW]** `components/automation-studio/WorkflowBuilder.tsx`: The main React Flow canvas wrapper.
*   **[NEW]** `components/automation-studio/Sidebar.tsx`: Drag-and-drop panel for Triggers, Conditions, and Actions grouped by category (CRM, Finance, etc.).
*   **[NEW]** `components/automation-studio/nodes/TriggerNode.tsx`: Custom React Flow node for triggers.
*   **[NEW]** `components/automation-studio/nodes/ActionNode.tsx`: Custom React Flow node for actions.
*   **[NEW]** `components/automation-studio/nodes/ConditionNode.tsx`: Custom React Flow node for conditions.
*   **[NEW]** `components/automation-studio/NodeConfigPanel.tsx`: Slide-out panel to configure selected nodes (e.g., setting email template, choosing CRM stage).

### Documentation
*   **[NEW]** `docs/automation-ui.md`
*   **[NEW]** `docs/workflow-builder-ui.md`
*   **[NEW]** `docs/workflow-templates-ui.md`
*   **[NEW]** `docs/execution-history-ui.md`
*   **[MODIFY]** `roadmap.md`

## Verification Plan
### Automated Tests
- Run `npm run build` and `npm run lint` to ensure zero TypeScript or ESLint errors.
### Manual Verification
- Verify drag-and-drop functionality in the Workflow Builder.
- Verify node configuration updates state.
- Verify responsive layouts on desktop, tablet, and mobile views.
