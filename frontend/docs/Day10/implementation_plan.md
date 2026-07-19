# Day 10: AI Assistant, Communication & Automation Frontend

This document outlines the implementation plan for the Day 10 frontend requirements of BusinessOS AI.

## User Review Required

Please review the proposed approach. This involves introducing `@microsoft/signalr` for real-time notifications and `react-markdown` for AI chat rendering.
> [!IMPORTANT]
> The plan assumes we will install `@microsoft/signalr`, `react-markdown`, and `remark-gfm` to support real-time features and rich chat formatting. Let me know if you prefer alternatives.

## Open Questions

- Is there a specific library you want to use for the visual Automation Builder (e.g., `react-flow-renderer`), or should we start with a simpler form-based approach for defining rules? (The plan defaults to a simpler structured form layout for V1).

## Proposed Changes

### 1. Dependencies
- Install `@microsoft/signalr` for real-time notifications.
- Install `react-markdown` and `remark-gfm` for rendering AI chat responses.

### 2. Sidebar Integration
- Update `frontend/components/dashboard/sidebar.tsx` or equivalent navigation configuration to include links to:
  - `/dashboard/ai` (AI Assistant)
  - `/dashboard/email` (Email)
  - `/dashboard/whatsapp` (WhatsApp)
  - `/dashboard/notifications` (Notifications)
  - `/dashboard/automation` (Automation)

### 3. API & Hooks Layer
- Create `frontend/lib/api/ai.ts`, `email.ts`, `whatsapp.ts`, `automation.ts`, `notifications.ts`.
- Create corresponding custom React Query hooks (e.g., `useConversations`, `useSendMessage`, `useNotifications`, etc.).
- Set up a SignalR hook (`useSignalR`) or Context to manage the connection to `/hubs/notifications`.

### 4. AI Assistant (`/dashboard/ai`)
- **Layout**: Sidebar for conversation history, main area for chat.
- **Chat UI**: 
  - Message rendering with `react-markdown` for code blocks and rich text.
  - Form to send messages.
  - Empty state / New Chat state with prompt suggestions.
  - Loading indicators and token usage displays.

### 5. Communication Centers
- **Email Center** (`/dashboard/email` & `/dashboard/email/templates`):
  - Dashboard showing history, drafts, scheduled emails.
  - Compose email dialog/page.
  - Template management.
- **WhatsApp Center** (`/dashboard/whatsapp`):
  - Configuration settings view.
  - Send message UI and template selection.
  - History view.

### 6. Notifications (`/dashboard/notifications`)
- Dedicated page for viewing all notifications.
- Real-time popups (toasts) using SignalR when a new notification arrives.
- Filter by read/unread and category.

### 7. Automation Builder (`/dashboard/automation`)
- List of automation rules and their status/logs.
- `/dashboard/automation/create`: A form builder to select a Trigger (e.g., "On Scan"), define Conditions, and add Actions (e.g., "Send Email").

### 8. Documentation
- Create `frontend/docs/ai-communication-ui.md` detailing the frontend architecture.
- Update the global `roadmap.md` if necessary.

## Verification Plan

### Manual Verification
- Test creating a new AI conversation and sending a message.
- Verify markdown renders correctly in the AI chat.
- Test that notifications appear in real-time when triggered (or simulated).
- Verify the Automation Builder creates rules properly.
- Verify Responsive UI across mobile, tablet, and desktop views.
- Ensure no TypeScript or ESLint errors exist.
