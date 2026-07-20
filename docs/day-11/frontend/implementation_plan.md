# Frontend Implementation Plan: Day 11 (CRM Module)

## Goal Description
Implement the frontend user interface for the CRM module in the Next.js application, allowing users to interact with Leads, Contacts, Companies, Deals, and Tasks seamlessly.

## Proposed Changes

### CRM Client Services
- Create `frontend/lib/crm-service.ts` using `axios` to handle API communication with the backend CRM endpoints.
- Define strongly-typed interfaces in `frontend/types/crm.ts` matching the backend DTOs.

### Dashboard UI (Pages & Components)
- **CRM Overview (`/dashboard/crm`)**: High-level metrics and quick actions.
- **Leads Management (`/dashboard/leads`)**: Kanban/List view of potential customers.
- **Contacts & Companies (`/dashboard/contacts`, `/dashboard/companies`)**: Detailed lists with data tables and individual detail views.
- **Sales Pipeline (`/dashboard/deals`)**: Drag-and-drop or status-based sales pipeline tracking.
- **Tasks (`/dashboard/tasks`)**: Task tracking linked to CRM entities.

### Integration Fixes
- Ensure all API paths are routed through the `/api/crm/` Next.js proxy prefix.
- Fix UI rendering bugs (e.g., unescaped quotes in global search).
- Resolve any live SignalR WebSocket connection looping issues by updating connection strings to bypass the proxy.

## Verification Plan
- `npm run build` must succeed without TypeScript or ESLint errors.
- Confirm pages load without Next.js runtime crashes.
- Verify WebSocket errors are eliminated from the console.
