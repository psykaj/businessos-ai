# Frontend Implementation Plan: AI Business Memory

This plan outlines the frontend development for Day 31: Business Memory UI and Copilot enhancements.

## Proposed Changes

### 1. API Services & State Management
- **NEW**: `frontend/lib/memory-service.ts`: Axios client functions for `/api/memory` (GET list, GET by customer, GET relevant, PUT deactivate, DELETE) and `/api/copilot/context`.
- **NEW**: `frontend/hooks/use-memory.ts`: React Query hooks (`useMemories`, `useCustomerMemories`, `useCopilotContext`, `useDeleteMemory`, `useUpdateMemory`).
- **NEW**: `frontend/types/memory.ts`: TypeScript interfaces mirroring backend DTOs (`MemoryDto`, etc.).

### 2. Business Memory Settings Page
- **MODIFY**: `frontend/app/dashboard/settings/page.tsx` - Add a distinct card link navigating to the Business Memory settings.
- **NEW**: `frontend/app/dashboard/settings/business-memory/page.tsx` - Main view for listing and managing memories.
- **NEW**: `frontend/components/settings/MemoryListTable.tsx` - Data table displaying Type, Source, Confidence, Created Date, and Actions.
- **Features**: Implement Search/Filter by Type, Empty States ("BusinessOS AI is still learning"), and action menus (Deactivate/Delete/Edit allowed preferences).

### 3. Customer 360 Enhancement
- **MODIFY**: `frontend/app/dashboard/customers/[id]/page.tsx`
  - Add a new tab: `<TabsTrigger value="ai-context">✨ AI Context</TabsTrigger>`
  - Add `<TabsContent value="ai-context">` which renders a new component: `<CustomerAiContext customerId={customerId} />`.
- **NEW**: `frontend/components/customer-success/CustomerAiContext.tsx`
  - Uses `useCustomerMemories(customerId)` to fetch and display known patterns (Purchase frequency, preferences, previous successful actions).
  - Handles Loading, Error, and Empty states gracefully.

### 4. Copilot Enhancements
- **MODIFY**: `frontend/components/copilot/CopilotWorkspace.tsx`
  - Integrate `useCopilotContext` hook.
  - When a user submits a prompt, the UI can optionally fetch `/api/copilot/context` to retrieve relevance before displaying the result.
  - Add a "Using Business Context" indicator badge below the input or next to the AI response.
  - Clicking the badge opens a Side Drawer or Modal (`BusinessContextDrawer`) showing:
    - Concise evidence/memories used.
    - Clickable related records (e.g., links to Customer 360).
    - Source references (e.g., CRM, Inventory).

## User Review Required
> [!IMPORTANT]
> Currently, `CopilotWorkspace` sends the prompt directly to `/api/ai-agent/execute`. To satisfy the requirement of showing "Relevant Business Context" *inside* Copilot, I will fetch `/api/copilot/context` in parallel or right before executing the command so we can display the context used for the answer alongside the AI's response. Is this the desired flow?

## Verification Plan
1. **TypeScript & Linting**: Run `npx tsc --noEmit` and `npm run lint`.
2. **Build**: Run `npm run build` to ensure no build errors.
3. **Manual Testing**: 
   - Verify Settings page routes and Memory List loads (even if empty).
   - Test opening the AI Context tab in Customer 360.
   - Verify Copilot Context badge appears when context is fetched.
