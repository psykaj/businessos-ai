# CRM Frontend Architecture

## Overview
The CRM frontend is an enterprise-grade extension to BusinessOS AI, designed with Next.js App Router, Tailwind CSS, Shadcn UI, and React Query. It is built to be fast, responsive, and highly modular.

## Component Structure
The CRM pages are located under `app/dashboard/`:
- `crm/`: Dashboard containing high-level KPI cards and `recharts` graphs to visualize pipeline value and conversion rates.
- `leads/`, `contacts/`, `companies/`: List pages with tabular data and detail pages utilizing `Tabs` (Overview, Activities, Notes, Deals).
- `deals/`: Contains the Sales Pipeline.
- `tasks/`, `activities/`, `calendar/`: Core timeline and task management interfaces.
- `tags/`: Organization-wide tag management.

## Kanban Architecture (Sales Pipeline)
The Deals Kanban board (`/dashboard/deals`) is built using **HTML5 Native Drag and Drop**.
- `draggable={true}` on Deal cards.
- `onDragStart` sets the dragged Deal ID into React State and the DataTransfer object.
- `onDragOver` allows dropping on the column container.
- `onDrop` triggers a `PATCH` request via React Query Mutation to update the Deal's Pipeline Stage on the backend.
- **Optimistic Updates:** React Query cache is instantly updated upon drop to ensure a snappy user experience, while the mutation syncs with the server in the background.

## API Integration & State Management
- **Axios Client**: All requests run through `lib/api-client.ts`, which attaches the JWT token securely to headers.
- **Service Layer**: `lib/crm-service.ts` encapsulates all CRM-related API calls.
- **State Management**: `@tanstack/react-query` is the primary state management tool. It handles caching, loading states, refetching on window focus, and cache invalidation after mutations (e.g., updating a Deal stage invalidates the `crm-deals` query).

## Responsive Strategy
- All tables are wrapped in `overflow-x-auto` to allow horizontal scrolling on mobile devices.
- The Kanban board uses horizontal scrolling for columns (`overflow-x-auto flex gap-4 min-w-max`).
- Grid layouts (`grid-cols-1 md:grid-cols-2 lg:grid-cols-4`) ensure KPI cards adapt to screen size.

## Global Search
Implemented as a unified React component (`<GlobalSearch />`) placed inside the main Header. It queries `/api/crm/search` instantly as the user types (with a minimum of 2 characters) and displays an absolute-positioned dropdown overlay showing Contacts, Leads, Companies, and Deals.

## Future Enhancements
- Integration of `react-beautiful-dnd` or `dnd-kit` for animated list re-ordering within columns (currently using native HTML5 which doesn't animate reordering out of the box).
- Real-time updates using SignalR to broadcast pipeline changes across all active sessions in the organization.
- Advanced filtering and saving custom Views (e.g., "My Open Leads").
