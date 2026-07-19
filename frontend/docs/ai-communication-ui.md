# AI Communication & Automation Frontend Architecture

## Frontend Architecture
The Day 10 frontend module adds the AI Assistant, Email, WhatsApp, Notifications, and Automation workflows to the BusinessOS AI platform. The architecture strictly follows Clean Architecture and SOLID principles, utilizing Next.js App Router for routing, React hooks for state management, and Tailwind CSS with Shadcn UI for presentation.

## AI Chat Component Structure
- `AiLayout`: Houses the common structure with a persistent `AiSidebar` on the left and dynamic content on the right.
- `AiSidebar`: Loads conversation history using `getConversations` and manages navigation.
- `AiChat`: A reusable conversation interface. It supports an optional `conversationId`. 
  - If no ID is passed, it shows the empty state with prompt suggestions. 
  - On the first message, it optimistically creates the message, calls the backend to create a conversation, and redirects to the new `/dashboard/ai/[conversationId]` route.
  - Messages are rendered using `react-markdown` and `remark-gfm` for rich text and code blocks.

## Communication Center
- **Email Center** (`/dashboard/email`): A dashboard featuring KPI cards (Sent Mails, Open Rate, Drafts, Templates) and a tabs-based interface to switch between Inbox, Sent, and Drafts.
- **WhatsApp Center** (`/dashboard/whatsapp`): Provides a unified view of WhatsApp campaigns, templates, and delivery statistics. 

## Automation Builder Architecture
- `/dashboard/automation`: Displays a grid of active automation rules and key metrics like executions and success rate.
- `/dashboard/automation/create`: A form-based visual builder. The V1 iteration uses structured dropdowns for Triggers (e.g., "Subscription Created") and Actions (e.g., "Send Email"). The architecture is designed so this can easily be replaced by a node-based visual editor (like React Flow) in the future.

## API Integration Strategy
Each module has its own typed API client service under `frontend/lib/api/`. These services abstract away `axios` calls and use the common `api-client.ts` which automatically attaches JWT tokens to requests and handles 401 unauthorized errors via interceptors.

## SignalR Integration
A global `NotificationContext` (`NotificationProvider`) uses `@microsoft/signalr` to maintain a persistent WebSocket connection to the backend. It handles:
- Automatic reconnection.
- Fetching unread notifications on mount.
- Listening for `ReceiveNotification` and `ReceiveBroadcast` events.
- Toasting new notifications globally via `sonner` so the user is always informed regardless of which page they are on.

## State Management
- **Server State**: Managed via `React Query` (or direct API calls encapsulated in components for simplicity in V1).
- **Global UI State**: Context API is used for real-time notifications (`NotificationContext`) and Authentication (`AuthContext`).
- **Local State**: Managed using standard `useState` hooks for forms and temporary UI states (e.g., loading spinners, active tabs).

## Responsive Strategy
All pages are fully responsive.
- **AI Chat**: Sidebar hides on mobile or converts to a drawer (managed by `layout.tsx`). The chat window flexes to fill available space.
- **Dashboards**: CSS Grid (`grid-cols-1 md:grid-cols-3`) is heavily utilized to stack metric cards and workflows cleanly on smaller devices.

## Future Enhancements
- Introduce React Flow for drag-and-drop automation workflows.
- Expand AI model selection (OpenAI, Anthropic) via UI toggles.
- Add live HTML preview for Email Templates.
- Implement pagination and infinite scrolling in the Notification Center.
