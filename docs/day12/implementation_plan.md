# Implementation Plan - Day 12 Lead Capture & Marketing Automation Frontend

This plan outlines the architecture and steps to implement the complete Frontend for Lead Capture, Forms, Campaigns, and Customer Journey mapping in BusinessOS AI.

## Open Questions
- For the Form Builder, should we initially use a JSON-based schema builder (add fields via side panel) or true drag-and-drop (like `dnd-kit`)? I plan to implement a highly interactive click-to-add/reorder schema builder using state to keep it robust and responsive, which gives a "drag-and-drop ready" feel without the complexity of generic drag-and-drop libraries right away.
- Should the public form endpoint (`/public-forms/[id]`) be hosted within this Next.js app as a separate layout, outside the dashboard? I plan to create an `app/(public)/f/[id]/page.tsx` for rendering standalone forms that can be shared via URL or embedded in iframes.

## Proposed Changes

### 1. Navigation & Layout Updates
Modify `sidebar.tsx` to include a new "Marketing & Leads" group.
- **Forms**: `/dashboard/forms`
- **Submissions**: `/dashboard/forms/submissions`
- **Campaigns**: `/dashboard/campaigns`
- **Customer Journey**: `/dashboard/customer-journey`
- **Lead Sources**: `/dashboard/lead-sources`

---

### 2. Form Builder Module (`/dashboard/forms`)
#### [NEW] `app/dashboard/forms/page.tsx`
List of forms with stats (views, submissions, conversion rate).
#### [NEW] `app/dashboard/forms/create/page.tsx` & `edit/[id]/page.tsx`
The visual form builder workspace. Will feature:
- A left sidebar (Form Settings & Field Palette).
- A central canvas (Live Preview).
- A right sidebar (Field Configuration/Validation Rules).
#### [NEW] `components/forms/builder/*`
Reusable components for `FieldPalette`, `FormCanvas`, `FieldSettings`, and `Preview`.
#### [NEW] `app/(public)/f/[id]/page.tsx`
The public-facing form renderer. Fetches form schema and renders the UI for external visitors to submit data.

---

### 3. Submissions Dashboard (`/dashboard/forms/submissions`)
#### [NEW] `app/dashboard/forms/submissions/page.tsx`
Data table showing all submissions across forms. Includes filtering, sorting, and pagination. Connects directly to the backend CRM lead via a "View Lead" action.

---

### 4. Campaign Management (`/dashboard/campaigns`)
#### [NEW] `app/dashboard/campaigns/page.tsx`
Overview dashboard of all campaigns. Displays KPI cards (Total Budget, ROI, Leads Generated).
#### [NEW] `app/dashboard/campaigns/create/page.tsx` & `[id]/page.tsx`
Detailed view for a specific campaign, showing performance charts and associated lead sources.

---

### 5. Customer Journey & Analytics (`/dashboard/customer-journey` & `lead-sources`)
#### [NEW] `app/dashboard/customer-journey/page.tsx`
A visual pipeline/funnel view showing customers transitioning from `Visitor -> Lead -> Customer -> Loyal Customer`. Uses interactive charts (e.g., Recharts) to show drop-off rates and time spent in stages.
#### [NEW] `app/dashboard/lead-sources/page.tsx`
A dashboard highlighting which sources (Website, WhatsApp, Google Ads) generate the most revenue and leads.

---

### 6. API Integration & State Management
#### [NEW] `hooks/use-forms.ts`
React Query hooks for `useForms`, `useForm`, `useCreateForm`, `useUpdateForm`, `usePublishForm`, `useSubmitForm`.
#### [NEW] `hooks/use-campaigns.ts`
React Query hooks for campaign CRUD and analytics fetching.
#### [NEW] `hooks/use-customer-journey.ts`
React Query hooks for journey statistics and individual lead timelines.
#### [NEW] `hooks/use-marketing-analytics.ts`
React Query hooks for fetching data for the executive dashboard.

---

### 7. Documentation
#### [NEW] `docs/marketing-ui.md`
Detailed explanation of the Form Builder architecture, API state management, and reusable components.
#### [MODIFY] `docs/roadmap.md`
Check off all Step 10 Deliverables.

## Verification Plan

### Automated Checks
- `npm run lint` and `tsc --noEmit` to ensure zero TS/ESLint errors.
- Ensure no duplicate components and clean imports.

### Manual Verification
- **Form Builder**: Build a dynamic form, add multiple field types (Text, Dropdown, Email), set validation rules, and publish.
- **Public Submission**: Navigate to the public form URL, fill it out, and submit. Verify success state.
- **CRM Automation**: Check the Submissions page and confirm a CRM Lead was successfully created from the submission.
- **Journey & Analytics**: Verify the Customer Journey funnel updates and Campaign ROI metrics load properly.
- **Responsiveness**: Verify Form Builder sidebar collapses cleanly on Mobile, and Data Tables become scrollable.
