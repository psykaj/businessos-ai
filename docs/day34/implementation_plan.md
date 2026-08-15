# Frontend AI Daily Business Operating Loop Implementation Plan

The objective is to create the "Today" page for the AI Daily Business Operating Loop. This page will become the central hub for the business owner to quickly assess "What matters today?", answer why it matters, and provide immediate actionable pathways.

## Proposed Changes

We will introduce a new page route at `app/dashboard/today/page.tsx` and integrate it smoothly into the existing `DashboardShell` layout. This ensures backward compatibility while adding a dedicated loop.

### 1. API Services and React Query Integration

- #### [NEW] [daily-operating-loop-service.ts](file:///Users/pankajanilyadav/Documents/Simplify/frontend/lib/daily-operating-loop-service.ts)
  - Provides the API client bindings and `react-query` hooks:
    - `useTodayBriefing()` (calls `/api/daily-briefing/today`)
    - `useGenerateBriefing()` (calls `/api/daily-briefing/generate`)
    - `useRefreshBriefing()` (calls `/api/daily-briefing/{id}/refresh`)
    - `useCompletePriority(id)`, `useDismissPriority(id)`, `useSnoozePriority(id)`

### 2. Page & Layout Components

- #### [MODIFY] [sidebar.tsx](file:///Users/pankajanilyadav/Documents/Simplify/frontend/components/layout/sidebar.tsx)
  - Add "Today" at the top of the "Overview" section, pointing to `/dashboard/today`.
- #### [NEW] [page.tsx](file:///Users/pankajanilyadav/Documents/Simplify/frontend/app/dashboard/today/page.tsx)
  - The main page orchestrator. It uses `useTodayBriefing` (falling back to generation if missing), displays skeleton loaders, empty states, and errors.
  - Composes the top-level subcomponents: `TodayHeader`, `TodayBusinessHealth`, `TodaySummary`, `TodayPriorities`, `YesterdayReview`, and `TodayBusinessValue`.

### 3. "Today" Components 

We will create a new directory `components/dashboard/today/` to house the modular components.

- #### [NEW] [today-header.tsx](file:///Users/pankajanilyadav/Documents/Simplify/frontend/components/dashboard/today/today-header.tsx)
  - Standard page title ("Today") with a "Updated X mins ago" timestamp and a manual "Refresh" button.
- #### [NEW] [today-health.tsx](file:///Users/pankajanilyadav/Documents/Simplify/frontend/components/dashboard/today/today-health.tsx)
  - Visual indicator (pill/badge) of the overall `BusinessHealth` with the AI summary immediately below it.
- #### [NEW] [today-priorities.tsx](file:///Users/pankajanilyadav/Documents/Simplify/frontend/components/dashboard/today/today-priorities.tsx)
  - Iterates through the top 5 `Priorities` (sorted by `PriorityScore`) and renders a `PriorityCard`.
- #### [NEW] [priority-card.tsx](file:///Users/pankajanilyadav/Documents/Simplify/frontend/components/dashboard/today/priority-card.tsx)
  - Compact view: Shows Title, "Why it matters" (`Reason`), "Recommended Action", and Status. Includes action buttons (Complete, Dismiss, Snooze) and a primary action linking out (e.g. `View Customer` or `Take Action`).
  - Implements a clickable area that expands into a `PriorityDrawer`.
- #### [NEW] [priority-drawer.tsx](file:///Users/pankajanilyadav/Documents/Simplify/frontend/components/dashboard/today/priority-drawer.tsx)
  - Detailed slide-out panel utilizing `Sheet` from `shadcn/ui`.
  - Shows full evidence, connected goals/KPIs, expected impact, confidence levels, and prior outcomes related to this item.
- #### [NEW] [yesterday-review.tsx](file:///Users/pankajanilyadav/Documents/Simplify/frontend/components/dashboard/today/yesterday-review.tsx)
  - Compact summary section reporting `CompletedPriorityCount`, `DismissedCount`, and recent business value context pulled from the backend models.

### 4. AI Copilot Integration

- #### [MODIFY] [CopilotNav.tsx](file:///Users/pankajanilyadav/Documents/Simplify/frontend/components/copilot/CopilotNav.tsx) & related context
  - Ensure the AI Copilot is aware of the `/today` context, allowing it to accurately answer prompts like "What should I do first today?".

## Verification Plan

### Manual Verification
1. **Empty/New State**: Verify that calling the page without prior briefings successfully calls `/generate` and renders an empty state if no business data exists.
2. **Health & Summary**: Ensure the health state correlates correctly to colors (Green for Healthy, Red for Critical) and the AI summary is accurately placed.
3. **Priority Drawer & Actions**: Open a priority card, confirm details map to DTO, hit "Complete", and verify optimistic UI updates remove it or check it off immediately via React Query mutation.
4. **Mobile Responsiveness**: Simulate mobile device and ensure cards wrap and the primary action button is sticky/prominent.
5. **Navigation**: Ensure clicking "View Invoice" or "View Customer" correctly navigates to the respective module without breaking state.

> [!WARNING]
> **API Dependency**: This frontend relies strictly on the exact DTO output of the backend `DailyBriefingController`. The UI logic will never generate fake insights. If the backend priority array is empty, the UI will respectfully fall back to the "caught up" state.
