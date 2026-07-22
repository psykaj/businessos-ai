# Workflow Automation & Integration Frontend Documentation

The Workflow Automation Center is a no-code visual workflow builder for BusinessOS AI comparable to Zapier, Make.com, n8n, and Microsoft Power Automate.

## Architecture

- **Framework**: Next.js 16 (App Router), React 19, TypeScript.
- **Visual Canvas**: React Flow (`@xyflow/react`) with custom Node Types (`TriggerNode`, `ActionNode`, `LogicNode`).
- **State Management**: React Query (`@tanstack/react-query`) with modular custom hooks (`useWorkflows`, `useIntegrations`).
- **Styling**: Tailwind CSS v4, Lucide Icons, Shadcn UI patterns.

## Route Structure

- `/dashboard/workflows`: Workflow Dashboard with analytics summary, search, status filter, and management actions.
- `/dashboard/workflows/create`: Visual Workflow Builder canvas with drag-and-drop node palette, config drawer, and AI Assistant.
- `/dashboard/workflows/[id]`: Workflow Detail viewer and execution history.
- `/dashboard/workflows/edit/[id]`: Visual Workflow Builder edit mode.
- `/dashboard/integrations`: Integration Center displaying 12 SaaS providers (`Google Sheets`, `Slack`, `Teams`, `Discord`, `Stripe`, `Razorpay`, `Twilio`, `Resend`, `Google Calendar`, `Outlook Calendar`, `Webhook`, `REST API`).
- `/dashboard/workflow-history`: Real-time execution monitoring and step log inspection drawer.
- `/dashboard/templates/workflows`: Pre-built business automation template library.

## AI Workflow Assistant

Allows business owners to type natural language prompts e.g.:
*"When a new lead is created, assign to John and send a welcome WhatsApp."*
Generates an editable visual node graph and applies it directly to the canvas in one click.
