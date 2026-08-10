# Build AI Business Command Center Frontend

This implementation plan outlines the creation of the **AI Business Command Center** frontend (Day 30), providing an actionable, clear overview of the business. As requested, we will overhaul the existing `/dashboard` route rather than creating duplicate screens.

## Proposed Changes

### `frontend/lib/command-center-service.ts`

#### [NEW] [command-center-service.ts](file:///Users/pankajanilyadav/Documents/Simplify/frontend/lib/command-center-service.ts)
Create the API client for the Command Center:
- Export TypeScript interfaces mapping the backend DTOs (`CommandCenterSummaryDto`, `CommandMetricDto`, `CommandOpportunityDto`, etc.).
- Create API fetcher functions.
- Create React Query hooks:
  - `useCommandCenterSummary()`: Fetches the entire dashboard data in one go for speed, leveraging the backend's parallel aggregation.
  - `useAskBusinessOS()`: A mutation hook for the AI Q&A input.

### `frontend/components/dashboard/command-center`

I will create a set of cohesive, action-oriented components that handle their own empty/error states and gracefully degrade.

#### [NEW] [command-center-health.tsx](file:///Users/pankajanilyadav/Documents/Simplify/frontend/components/dashboard/command-center/command-center-health.tsx)
Displays the Business Health score, status, and trend with color-coded indicators.

#### [NEW] [command-center-ai-summary.tsx](file:///Users/pankajanilyadav/Documents/Simplify/frontend/components/dashboard/command-center/command-center-ai-summary.tsx)
A premium-looking card displaying the AI-generated Executive Summary, highlighting key highlights, risks, and recommended actions.

#### [NEW] [command-center-needs-attention.tsx](file:///Users/pankajanilyadav/Documents/Simplify/frontend/components/dashboard/command-center/command-center-needs-attention.tsx)
A highly prioritized list combining `PriorityAlerts` and high-priority `RecommendedActions`. Shows only top 3-5 issues with explicit "Take Action" or "View Details" buttons routing to existing modules.

#### [NEW] [command-center-opportunities.tsx](file:///Users/pankajanilyadav/Documents/Simplify/frontend/components/dashboard/command-center/command-center-opportunities.tsx)
A grid of business opportunities (unpaid invoices, uncontacted leads, low inventory) with their potential financial impact and CTA buttons.

#### [NEW] [command-center-metrics.tsx](file:///Users/pankajanilyadav/Documents/Simplify/frontend/components/dashboard/command-center/command-center-metrics.tsx)
A grid of key metrics (Revenue, Customers, Orders, etc.) with trend indicators. Clicking a metric will route to the respective module (e.g., Finance, CRM).

#### [NEW] [command-center-automations.tsx](file:///Users/pankajanilyadav/Documents/Simplify/frontend/components/dashboard/command-center/command-center-automations.tsx)
A compact widget displaying active automations, success rate, and failed executions needing attention.

#### [NEW] [command-center-activity.tsx](file:///Users/pankajanilyadav/Documents/Simplify/frontend/components/dashboard/command-center/command-center-activity.tsx)
A scrollable timeline of recent business-relevant activities.

#### [NEW] [command-center-ask-ai.tsx](file:///Users/pankajanilyadav/Documents/Simplify/frontend/components/dashboard/command-center/command-center-ask-ai.tsx)
An interactive chat-like input for "Ask anything about your business...". Upon submission, it renders the AI's answer, related metrics, and quick action buttons.

### `frontend/app/dashboard/page.tsx`

#### [MODIFY] [page.tsx](file:///Users/pankajanilyadav/Documents/Simplify/frontend/app/dashboard/page.tsx)
Transform the existing placeholder dashboard into the actual Command Center.
- Layout order: Business Health & AI Ask → AI Executive Summary → Needs Attention → Opportunities → Metrics → Automations & Activity.
- Implement responsive layout: Stacks on mobile, uses grid on desktop.
- Add robust loading states (Skeletons) while React Query fetches data.

## Verification Plan

### Automated Tests
Currently, the frontend doesn't seem to have a comprehensive unit test suite running locally, but I will ensure the code is type-safe and builds cleanly.

### Manual Verification
- Run `npm run lint` and `npm run build`.
- Verify the layout responds correctly on Mobile and Desktop viewports.
- Confirm all CTAs link to valid existing routes (`/dashboard/finance`, `/dashboard/crm`, `/dashboard/inventory`, etc.).
- Ensure that if the AI or Health data fails, the rest of the dashboard renders fallback skeletons or error messages without crashing the page (Graceful Degradation).

## Open Questions

> [!NOTE]
> Since we are modifying the `/dashboard` page directly, any existing placeholder components like `MetricsGrid` will be replaced by the new unified `CommandCenterMetrics`. This adheres to your requirement of not creating duplicate screens.

> [!IMPORTANT]
> Please review the plan above. Click **Proceed** if you approve, and I will begin the implementation!
