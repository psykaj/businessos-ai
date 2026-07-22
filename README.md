# BusinessOS AI

> **AI-powered Business Operating System for SMEs.**  
> Help 1 Million Small & Medium Businesses grow online using AI — one unified platform instead of 10 disparate software tools.

---

## 🎯 Vision

BusinessOS AI is an all-in-one AI platform built for SMEs to manage sales, customer relations, automated marketing, and cross-application workflows seamlessly.

| Module | Description | Status |
|--------|-------------|--------|
| 🔲 **QR Code Management** | Dynamic QR creation, styling, tracking & analytics | ✅ Completed |
| ⭐ **AI Review Reply** | Auto-reply to customer platform reviews using AI | ✅ Completed |
| 🧾 **Invoice Generator** | Create, manage, and send professional invoices | ✅ Completed |
| 💬 **WhatsApp & Email Marketing** | Automated WhatsApp and email campaigns at scale | ✅ Completed |
| 👥 **CRM & Sales Pipeline** | Leads, Contacts, Companies, Deals, and Task management | ✅ Completed |
| 📋 **Lead Capture & Forms** | Public form builder, submission tracking, customer journey | ✅ Completed |
| ⚡ **Workflow Automation** | No-code visual workflow engine (Zapier / n8n alternative) | ✅ Completed |
| 🔌 **Integration Platform** | Connect Google Sheets, Slack, Teams, Stripe, Twilio, etc. | ✅ Completed |
| 🤖 **AI Assistant** | 24/7 AI business assistant with natural language workflow builder | ✅ Completed |
| 📊 **Analytics** | Real-time business reporting and scan analytics | ✅ Completed |
| 💼 **Digital Business Card** | NFC & QR-based digital business cards | ✅ Completed |
| 🎨 **White Label Platform** | Custom domains, branding, themes, and SEO settings | ✅ Completed |

**Long-term Goal:** Become the all-in-one Shopify + Zoho + Zapier + Canva + QR Tiger for SMEs.

---

## 🗺️ Development Roadmap & Progress

| Day | Module / Feature | Status | Tech / Details |
|-----|------------------|--------|----------------|
| **Day 1** | Project Initialization & Setup | ✅ **Done** | ASP.NET Core 9, Clean Architecture, Next.js 16 |
| **Day 2** | Dashboard UI & Shell | ✅ **Done** | Tailwind CSS v4, Lucide icons, Dark mode |
| **Day 3** | Authentication & Authorization | ✅ **Done** | JWT authentication, multi-tenant isolation |
| **Day 4** | Dynamic QR Code Platform | ✅ **Done** | QR generation, customization, scan tracking |
| **Day 5** | Billing & Subscriptions | ✅ **Done** | Stripe & Razorpay payment gateway integration |
| **Day 6** | RBAC & Permissions | ✅ **Done** | Role-based permissions, custom role builder |
| **Day 7** | Team Management & API Keys | ✅ **Done** | Organization invites, API key management |
| **Day 8** | Business Analytics | ✅ **Done** | Recharts visual data charts & reporting |
| **Day 9** | White Label Platform | ✅ **Done** | Custom domains, branding, themes, SEO |
| **Day 10** | AI & Communication Center | ✅ **Done** | OpenAI assistant, Email center, WhatsApp campaigns |
| **Day 11** | CRM & Sales Pipeline | ✅ **Done** | Leads, Contacts, Companies, Deals, Tasks, Activities |
| **Day 12** | Lead Capture & Marketing Automation | ✅ **Done** | Form Builder, Submissions, Customer Journey, Webhooks |
| **Day 13** | Workflow Automation & Integration Platform | ✅ **Done** | Visual React Flow builder, 13 Triggers, 13 Actions, 12 Integrations, AES-256 encryption, AI Assistant |

---

## ⚡ Day 13 Highlights: Workflow Automation & Integration Platform

Deeply integrated inside BusinessOS AI, this module replaces expensive third-party automation tools with an in-house visual workflow builder.

### ⚙️ Backend Architecture (ASP.NET Core .NET 9)
- **Plugin Registries**: Interface-based registries (`ITriggerHandler`, `IActionHandler`, `IIntegrationProvider`) allow adding new triggers, actions, and integrations without touching core execution logic.
- **Trigger Engine**: Supports 13 triggers (`LeadCreated`, `FormSubmitted`, `QRCodeScanned`, `PaymentReceived`, `CustomerRegistered`, `SubscriptionExpiring`, `ManualTrigger`, `ScheduledTrigger`, etc.).
- **Action Engine**: Supports 13 actions (`CreateCrmLead`, `UpdateCrm`, `AssignSalesperson`, `SendEmail`, `SendWhatsApp`, `SendNotification`, `GenerateInvoice`, `GenerateQR`, `CallAiAssistant`, `CallWebhook`, `DelayExecution`, `UpdateCustomerJourney`, `LogActivity`).
- **Integration Platform**: Supports 12 SaaS providers (`Google Sheets`, `Slack`, `Teams`, `Discord`, `Stripe`, `Razorpay`, `Twilio`, `Resend`, `Google Calendar`, `Outlook Calendar`, `Webhook`, `REST API`).
- **Security & Encryption**: Sensitive API keys and tokens are encrypted at rest using AES-256 (`EncryptionService`) and masked (`****ABCD`) on read DTOs.
- **Execution Engine**: Asynchronous sequential processing, condition evaluation (`IF/ELSE`, `AND/OR`), up to 3 retries with exponential backoff, and step-by-step audit logging into `WorkflowExecutionLogs`.

### 🎨 Frontend Architecture (Next.js 16 + React Flow)
- **Visual Builder**: Built using `@xyflow/react` with custom node types (`TriggerNode`, `ActionNode`, `LogicNode`), connecting edges, minimap, controls, and dot grid canvas.
- **Node Config Drawer**: Slide-over drawer to configure inputs and insert dynamic variable tags (`{{CustomerName}}`, `{{InvoiceNumber}}`, `{{DealValue}}`, `{{today.date}}`).
- **AI Workflow Assistant**: Accepts natural language prompts (e.g. *"When a new lead is created, assign to John and send a welcome WhatsApp"*) and auto-generates matching node graphs.
- **Integration Center**: Provider cards with real-time status badges, test connection triggers, and encrypted key connection modal.
- **Templates Library**: Pre-configured business workflow templates ready for single-click installation.
- **Execution Monitoring**: Real-time log inspector drawer displaying step inputs, outputs, duration, and error tracebacks.

---

## 🛠️ Tech Stack Overview

### Backend
- **Framework**: ASP.NET Core (.NET 9 Web API)
- **Architecture**: Clean Architecture, CQRS, Repository Pattern
- **Database**: PostgreSQL with Entity Framework Core 9
- **Security**: JWT Authentication, Multi-tenant Isolation, AES-256 Credential Encryption
- **Communication**: SignalR for real-time notifications & logs, HttpClient for Webhooks & SaaS APIs
- **Libraries**: FluentValidation, AutoMapper

### Frontend
- **Framework**: Next.js 16 (App Router), React 19, TypeScript
- **Visual Engine**: `@xyflow/react` (React Flow)
- **Styling**: Tailwind CSS v4, Lucide Icons, Shadcn UI patterns
- **State & Data**: React Query (`@tanstack/react-query`), Axios, React Hook Form, Zod
- **Notifications**: Sonner

---

## 📚 Documentation & Reference Files

Detailed technical documents are available in the [`docs/`](docs/) directory:

- [**docs/workflows.md**](docs/workflows.md) — Workflow Engine Architecture & REST APIs
- [**docs/triggers.md**](docs/triggers.md) — Trigger Engine & 13 Trigger Types
- [**docs/actions.md**](docs/actions.md) — Action Engine & 13 Action Types
- [**docs/integrations.md**](docs/integrations.md) — Integration Platform & 12 SaaS Providers
- [**docs/executions.md**](docs/executions.md) — Execution Monitoring & Step Logging
- [**docs/workflow-ui.md**](docs/workflow-ui.md) — Visual Workflow Builder & AI Assistant Frontend Architecture
- [**docs/day13-implementation-plan.md**](docs/day13-implementation-plan.md) — Day 13 Technical Implementation Plan
- [**docs/day13-walkthrough.md**](docs/day13-walkthrough.md) — Day 13 Feature & Verification Walkthrough

---

## 🚀 Running Locally

### 1. Backend Server (.NET 9 API)
```bash
dotnet run --project backend/backend.csproj
# API will start on http://localhost:5000
```

### 2. Frontend Development Server (Next.js 16)
```bash
cd frontend
npm install
npm run dev
# App will start on http://localhost:3000
```