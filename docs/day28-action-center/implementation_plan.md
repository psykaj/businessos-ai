# AI Action Center Frontend Implementation Plan

This document outlines the architecture and implementation steps for building the AI Action Center Dashboard frontend.

## User Review Required

> [!IMPORTANT]
> - **Route Placement**: I plan to place the new page at `/dashboard/action-center` since it seems to be part of the authenticated BusinessOS dashboard. Let me know if you prefer it strictly at the root `/action-center`.
> - **UI Components**: I will use `shadcn/ui` components (already present in the project) and will add the `Sheet` component for the Action Details Drawer.
> - **State Management**: Using React Query for data fetching, caching, and optimistic UI updates for the one-click actions.

## Proposed Components

---

### Page Structure

#### [NEW] [page.tsx](file:///Users/pankajanilyadav/Documents/Simplify/frontend/app/dashboard/action-center/page.tsx)
The main page layout composing the Business Impact Dashboard, Filters, Execution Timeline, and the list of Suggested Actions.

---

### Features & UI Sections

#### [NEW] [BusinessImpactDashboard.tsx](file:///Users/pankajanilyadav/Documents/Simplify/frontend/components/action-center/BusinessImpactDashboard.tsx)
Displays top-level aggregate metrics: Revenue Saved, Cost Reduced, Time Saved, Actions Executed, Pending Actions, Success Rate. Will use standard `Card` components.

#### [NEW] [ActionFilters.tsx](file:///Users/pankajanilyadav/Documents/Simplify/frontend/components/action-center/ActionFilters.tsx)
A component for filtering actions by Priority, Category, Status, Date, and a text Search input.

#### [NEW] [AiActionCard.tsx](file:///Users/pankajanilyadav/Documents/Simplify/frontend/components/action-center/AiActionCard.tsx)
A beautiful card displaying an individual action. Includes priority badges, business impact metrics, and one-click action buttons (Approve, Reject, Execute) using optimistic UI updates. Wraps standard Framer Motion animations for list entry.

#### [NEW] [AiActionList.tsx](file:///Users/pankajanilyadav/Documents/Simplify/frontend/components/action-center/AiActionList.tsx)
Container rendering a grid/list of `AiActionCard` components. Implements Skeleton loading states while React Query fetches data.

#### [NEW] [ExecutionTimeline.tsx](file:///Users/pankajanilyadav/Documents/Simplify/frontend/components/action-center/ExecutionTimeline.tsx)
A visual timeline displaying the flow of actions from Pending -> Approved -> Executing -> Completed/Failed. 

#### [NEW] [ActionDetailsDrawer.tsx](file:///Users/pankajanilyadav/Documents/Simplify/frontend/components/action-center/ActionDetailsDrawer.tsx)
A slide-out drawer (using `Sheet`) displaying deep details: Recommendation, Reason, Expected Impact, Execution Steps, Rollback Plan, and Activity Log.

---

### Data Fetching (React Query)

#### [NEW] [useActionCenter.ts](file:///Users/pankajanilyadav/Documents/Simplify/frontend/hooks/useActionCenter.ts)
Contains all custom React Query hooks mapping to the backend we just built:
- `useGetActions()`: Fetches all actions.
- `useGetPendingActions()`
- `useGetActionHistory()`
- `useExecuteActionMutation()`: Executes with optimistic UI & toasts.
- `useApproveActionMutation()`
- `useRejectActionMutation()`

#### [NEW] [action-center.ts](file:///Users/pankajanilyadav/Documents/Simplify/frontend/types/action-center.ts)
TypeScript interfaces matching the backend `AiActionDto` and corresponding enums.

---

## Verification Plan

### Automated Tests
- TypeScript compilation check (`npx tsc --noEmit`).
- ESLint checks (`npm run lint`).

### Manual Verification
- View the UI at `http://localhost:3000/dashboard/action-center`.
- Verify Framer Motion animations and Skeleton loading states.
- Click "Approve" on an action and verify the optimistic UI update and Toast notification.
- Open the Details Drawer and verify the layout.
