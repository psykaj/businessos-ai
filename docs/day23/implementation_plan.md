# Implementation Plan - BusinessOS AI Day 23 (Customer Communication Center Frontend)

## Objective
Build the complete **Customer Communication Center & Omnichannel Unified Inbox Frontend** for BusinessOS AI using Next.js 15, React 19, TypeScript, Tailwind CSS, Shadcn UI, and TanStack React Query, ensuring high performance, vibrant modern dark/light aesthetics, and strict adherence to the **Mandatory Business Value Rule**.

## Business Value Compliance
Every UI module must satisfy at least one core pillar:
1. **Save time**: Stop agent context switching across disconnected vendor portals (WhatsApp, Email, SMS, Live Chat, FB, Instagram) by centralizing all messages into a unified triage workspace.
2. **Increase revenue**: Prioritize urgent SLA countdown targets for High-Lifetime Value (LTV) clients to retain high-paying customers and close enterprise renewals.
3. **Reduce operational costs**: Provide a complete native communication experience inside BusinessOS AI without paying expensive multi-seat licensing fees to external helpdesk tool vendors.
4. **Improve decision-making**: Display executive analytics and a real-time team resolution leaderboard to evaluate staffing volume, response speeds, and Customer Satisfaction (CSAT) ratings.

---

## Technical Architecture & Design

### Phase 1: Shared Service & TanStack Query Hooks
- **Service Layer (`frontend/lib/communication-service.ts`)**:
  - Defines TypeScript DTOs: `ConversationDto`, `MessageDto`, `MessageTemplateDto`, `InboxSummaryDto`, `AnalyticsOverviewDto`, `CommunicationChannelType`, and `ConversationPriority`.
  - Implements API interaction methods targeting `.NET 9` endpoints (`/api/communication/*`).
  - Incorporates a comprehensive offline demonstration fallback dataset so SMEs and developers can test triage workflows seamlessly during prototyping or offline development.
- **Custom React Query Hooks (`frontend/hooks/use-communication.ts`)**:
  - Implements standardized query keys (`communicationKeys.all`, `inboxSummary`, `conversations`, `messages`, `analytics`).
  - Provides hooks: `useInboxSummary`, `useConversations`, `useConversationMessages`, `useSendMessage`, `useAddInternalNote`, `useAssignConversation`, `useUpdateConversationStatus`, `useMessageTemplates`, and `useCommunicationAnalytics`.

### Phase 2: UI Component Architecture (`frontend/components/communication/`)
- **`ChannelBadge.tsx`**: Renders tailored visual styles, Lucide icons, and connection status pulse animations for all 6 messaging channels.
- **`InboxHeader.tsx`**: Provides multi-channel switcher tabs, real-time query search bar, priority/status triage filter dropdowns, SLA KPI indicators, and a bulk action trigger.
- **`ConversationList.tsx` & `ConversationItem.tsx`**: Virtualized conversation queue exhibiting unread indicators, SLA breach warnings, timestamps, and company tags.
- **`CustomerProfileSidebar.tsx`**: Customer 360 inspector displaying Lifetime Revenue Value (LTV), verified client badges, contact coordinates, tags, and ticket routing selectors.
- **`MessageComposer.tsx`**: Rich text input deck featuring dual tabs (Customer Reply vs. Confidential **Internal Team Note** in safety amber), quick canned response insertion, keyboard shortcuts (`Cmd + Enter`), and instantaneous **AI Suggested Answers**.
- **`ConversationWorkspace.tsx`**: 3-column enterprise messaging terminal uniting left-pane queue switching, center transcript stream, and right-pane Customer 360 intelligence.
- **`TemplateManager.tsx`**: Canned template builder with dynamic variable insertion pills (`{{customer.name}}`, `{{customer.company}}`, `{{agent.name}}`) and `/shortcut` command mapping.
- **`AnalyticsDashboard.tsx`**: Recharts executive analytics charts detailing message volume distribution, resolution velocity, CSAT satisfaction scores (1-5★), and team resolution leaderboards.

### Phase 3: Dashboard Pages & Navigation
- Creates 5 protected tenant dashboard routes under `/app/dashboard/`:
  - `/dashboard/communication`: Landing Command Center & Adapter Status Matrix
  - `/dashboard/inbox`: Unified Inbox Triage Workspace
  - `/dashboard/conversations`: Live 3-Column Conversation Terminal
  - `/dashboard/templates`: Canned Response & Shortcut Management
  - `/dashboard/communication-analytics`: Executive Intelligence & Team Leaderboards
- Integrates a dedicated **"Customer Communication"** navigation section directly below CRM & Sales in `frontend/components/layout/sidebar.tsx`.

### Phase 4: Developer Auth Enhancement for Prototype Verification
- Enhances `frontend/lib/auth-service.ts` with an automatic developer **Demo Fallback Session** when local backend database connectivity is unavailable, allowing instant authenticated login to verify all UI screens locally.

---

## Verification Plan
1. **Automated Build Test**: Run `npm run build` in `frontend/` to verify zero TypeScript errors, clean static type validation, and error-free bundling across all 125 application routes.
2. **Local Dev Server Test**: Launch Next.js Turbopack server (`npm run dev`) on port 3000, verify unauthenticated route redirection to `/login`, test offline dev authentication fallback, and check live component interactivity.
3. **Git & GitHub Compliance**: Commit and push changes exclusively to feature branch `feature/day23-communication-hub-frontend`, generate architecture docs in `docs/day23/`, and prepare a Pull Request for user verification and merging.
