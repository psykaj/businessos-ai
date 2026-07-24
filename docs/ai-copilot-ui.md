# AI Copilot UI & Smart Workspace

## Overview
The **AI Copilot UI** (`/dashboard/copilot`) provides SME business owners with a centralized natural language interface to run their entire business.

## Features
- **Conversational Interface**: Chat history sidebar, real-time message thread, Markdown formatting, code block rendering, and tool invocation tags.
- **Message Controls**: One-click prompt copy, thread regeneration, thread clear/delete, and rename.
- **Safety Confirmation Modal**: Triggered automatically when an action requires approval (`isConfirmed=false`).
- **Command Center Integration**: Quick prompt shortcut cards grouped by business domain (Sales, Customers, Finance, Marketing, Operations).

## Architecture & State Management
- Built with React 19, Next.js 16, and TypeScript.
- Powered by React Query via custom `useCopilot()` hooks in `hooks/use-copilot.ts`.
- Communicates with backend via `copilotService` in `lib/copilot-service.ts`.
