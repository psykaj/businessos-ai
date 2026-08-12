# BusinessOS AI Day 32 Frontend Mission: Business Value & ROI Experience

## Goal Description
The objective of this mission is to integrate the backend AI Outcome & ROI Intelligence Engine into the frontend. We will display measurable outcomes (Revenue Recovered, Time Saved, Successful Actions/Automations) within the existing BusinessOS AI Command Center. A dedicated `Business Outcomes` dashboard will be added to provide timeline tracking, attribution confidence (Verified, Estimated, Associated), and a detailed breakdown without over-engineering complex financial analytics.

## User Review Required
> [!IMPORTANT]  
> New route `/dashboard/outcomes` will be created and linked under the "Executive & Intelligence" section in the Sidebar. A new component `CommandCenterBusinessValue` will be added prominently to the `Command Center` page.

> [!CAUTION]
> As per the requirement, AI Copilot integration will instruct the user on how to query these values, relying on existing prompt contexts. We won't rebuild the chatbot but we will add natural language capability context for Copilot to fetch outcome data.

## Open Questions
- Is there a specific preference for where the `CommandCenterBusinessValue` block should sit on the `DashboardPage`? (My plan places it right below `Ask AI` as the Top Row alongside Health/Summary).
- For the "User-Reported Outcome", where should this trigger live? I propose adding a "Record Outcome" button on the Outcomes Dashboard and possibly in the Action Center near completed actions.

## Proposed Changes

---

### Data Fetching (Hooks & Services)

#### [NEW] [use-outcomes.ts](file:///Users/pankajanilyadav/Documents/Simplify/frontend/hooks/use-outcomes.ts)
Create custom React Query hooks to interface with the new backend endpoints:
- `useOutcomes(filters)` - Fetches paginated timeline/list of outcomes.
- `useRoiSummary(dateRange)` - Fetches the aggregated ROI data.
- `useCreateOutcome()` - Mutation for user-reported outcomes.

#### [NEW] [outcomes-service.ts](file:///Users/pankajanilyadav/Documents/Simplify/frontend/lib/outcomes-service.ts)
Create the API client calls matching the backend routes:
- `GET /api/outcomes`
- `GET /api/outcomes/roi-summary`
- `POST /api/outcomes`

---

### Components (New UI Elements)

#### [NEW] [CommandCenterBusinessValue.tsx](file:///Users/pankajanilyadav/Documents/Simplify/frontend/components/dashboard/command-center/command-center-business-value.tsx)
A new module for the Command Center. Will display total Revenue Recovered, Estimated Time Saved, and Successful Actions/Automations using `useRoiSummary`. Includes a CTA button to navigate to `/dashboard/outcomes`.

#### [NEW] [OutcomeBadge.tsx](file:///Users/pankajanilyadav/Documents/Simplify/frontend/components/dashboard/outcomes/outcome-badge.tsx)
A specialized badge to display attribution confidence levels clearly (e.g., "Verified", "Estimated", "Associated", "User Reported") using distinct but accessible colors/icons.

#### [NEW] [RecordOutcomeDialog.tsx](file:///Users/pankajanilyadav/Documents/Simplify/frontend/components/dashboard/outcomes/record-outcome-dialog.tsx)
A modal form allowing users to submit manual outcomes (Amount, Type, Notes) linked to an Action.

#### [NEW] [OutcomeDetailsSheet.tsx](file:///Users/pankajanilyadav/Documents/Simplify/frontend/components/dashboard/outcomes/outcome-details-sheet.tsx)
A slide-out drawer (using Shadcn `Sheet`) that displays the full timeline (Recommendation → Action → Outcome), business impact, attribution confidence, and an evidence-based explanation.

---

### Page Routes

#### [NEW] [page.tsx](file:///Users/pankajanilyadav/Documents/Simplify/frontend/app/dashboard/outcomes/page.tsx)
The main "Business Outcomes" dashboard. It will feature:
- Top ROI Summary Cards (Revenue, Time Saved, Actions)
- An Outcome Timeline/Table with server-side filters (Date, Outcome Type, Source)
- Empty states ("Your BusinessOS AI impact report will appear here...")
- Integration of the `RecordOutcomeDialog`.

#### [MODIFY] [page.tsx](file:///Users/pankajanilyadav/Documents/Simplify/frontend/app/dashboard/page.tsx)
Integrate the `CommandCenterBusinessValue` component into the primary dashboard layout.

---

### Navigation & Layout

#### [MODIFY] [sidebar.tsx](file:///Users/pankajanilyadav/Documents/Simplify/frontend/components/layout/sidebar.tsx)
Add "Business Outcomes" link to the `Executive & Intelligence` section of the main sidebar.

---

### AI Copilot Integration

#### [MODIFY] [CopilotWorkspace.tsx](file:///Users/pankajanilyadav/Documents/Simplify/frontend/components/copilot/CopilotWorkspace.tsx) (or relevant Copilot file)
Inject brief system prompt context or tool instructions indicating how the AI can query `/api/outcomes/roi-summary` if the user asks "How much money did AI recover?". 
*(Note: If Copilot is fully backend-driven, this might just involve updating the frontend's system prompt context sent to the backend, or adding a UI "Suggested Prompt" for Outcomes).*

---

## Verification Plan

### Automated Tests
- Run `npm run lint` and `npm run typecheck` to ensure there are no TypeScript or React errors.
- Run frontend unit/component tests if available.

### Manual Verification
- View the new `/dashboard/outcomes` route and verify empty states, loading skeletons, and real data integration.
- Ensure the Command Center displays the new Business Value summary correctly.
- Test responsive layout on mobile views to ensure Business Value and Timeline are mobile-friendly.
- Test Dark Mode compatibility using the existing Tailwind config.
