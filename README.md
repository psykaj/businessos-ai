# Simplify

> **AI-powered Business Operating System for SMEs.**  
> Help 1 Million Small & Medium Businesses grow online using AI — one unified platform instead of 10 disparate software tools.

---

## 🎯 Vision

Simplify is an all-in-one AI platform built for SMEs to manage sales, customer relations, automated marketing, cross-application workflows, customer retention, and executive analytics seamlessly.

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
| 🤖 **AI Business Agent & Copilot** | Natural language business command center, execution engine & recommendations | ✅ Completed |
| 👑 **Executive Dashboard & BI** | CEO command center, automatic 14 KPIs, AI decisions & forecasts | ✅ Completed |
| 💖 **Customer Success & Loyalty** | Customer 360°, Health Scores, Loyalty Programs, Referrals & CSAT | ✅ Completed |
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
| **Day 14** | Executive Dashboard, BI & AI Decision Engine | ✅ **Done** | CEO Command Center, 14 Enterprise KPIs, AI Recommendations, Predictive Forecasting, Goal Sync, PDF/Excel/CSV Exports |
| **Day 15** | AI Business Agent Backend & AI Copilot Frontend | ✅ **Done** | Provider-independent Command Engine, Tool Registry (9 Tools), Context Engine, Task Execution Engine, Safety Layer, AI Recommendations, Conversation Memory, Copilot Workspace |
| **Day 16** | Customer Success Center, Customer 360°, Health Engine & Loyalty Platform | ✅ **Done** | Customer Health score Engine (0-100), Loyalty Programs & Rewards, Referral System & Funnel, CSAT Feedback, Automated Success Tasks, Customer Segments, Customer 360° Profile |
| **Day 17** | Document Management System (DMS), E-Signature & Approval Workspace | ✅ **Done** | Storage provider abstraction (Local/Azure Blob), Multi-versioning, Hierarchical folders, Multi-level sequential/parallel approvals, E-Signature foundation with SHA-256 hash certification, Passcode public share links, Next.js Document Center UI (July 26, 2026) |

---

## 📄 Day 17 Highlights: Document Management System (DMS), E-Signature & Approval Workspace (July 26, 2026)

This module provides SMEs with a centralized document management platform comparable to DocuSign, Google Drive, Dropbox Business, and PandaDoc, eliminating the need for multiple external software subscriptions.

### ⚙️ Backend Architecture (ASP.NET Core .NET 10)
- **Storage Abstraction (`IStorageService`)**: Unified storage layer with `LocalStorageService` (HMAC signed URLs) and `AzureBlobStorageService` integration with seamless fallback.
- **Document & Version Control**: Multi-versioning pointer system, tag and metadata management, soft delete & restore, hierarchical path calculation, and full-text search.
- **Multi-Level Approval Engine**: Sequential & parallel approval step workflows, approval comments, rejection handling, due date tracking, and automated escalation hooks.
- **Provider-Independent E-Signatures**: Abstract `IESignatureProviderService` for self-hosted native digital signatures or DocuSign / Adobe Sign APIs. Includes token landing links, security PIN codes, multi-signer roles, completion certificates, and SHA-256 cryptographic hashes.
- **Passcode & Public Link Sharing**: Internal RBAC permission sharing + external passcode-encrypted public links with expiration date and access count logging.
- **Immutable Audit Trail**: Document audit entries capturing IP addresses, User Agents, timestamps, and action types.

### 🎨 Frontend Architecture (Next.js 16 + React 19 + React Query)
- **Document Center (`/dashboard/documents`)**: Folder breadcrumb navigation, grid/list view toggle, drag-and-drop upload dropzone with real-time percentage progress bar, search, tags, favorites, and storage metrics.
- **Document Viewer & Detail (`/dashboard/documents/[id]`)**: Dual-tab view featuring interactive preview/download, historical version timeline with 1-click reversion, and activity audit trail.
- **Document Templates (`/dashboard/documents/templates`)**: Categorized template library with variable placeholder substitution (`{{VariableName}}`) and 1-click document generator.
- **Approval Workspace (`/dashboard/approvals`)**: Tabbed approval queue (Pending, Approved, Rejected) with sequence step badges, due date alerts, and review modal (approve, reject, comment, escalate).
- **E-Signature Workspace (`/dashboard/signatures`)**: Signature request builder, progress tracking badges, SHA-256 hash verification, and audit certificates.
- **Shared Files & Public Links (`/dashboard/shared`)**: Passcode share manager, link expiration settings, and access count logging.

## 💖 Day 16 Highlights: Customer Success Center, Customer Retention & Loyalty Platform

This module provides SMEs with an enterprise-grade customer retention workspace comparable to HubSpot Service Hub, Salesforce Service Cloud, and Freshworks Customer Success.

### ⚙️ Backend Architecture (ASP.NET Core .NET 10)
- **Customer Health Score Engine**: Multi-factor scoring engine (0-100) calculating account health and risk levels (`Healthy`, `Stable`, `Needs Attention`, `High Risk`).
- **Loyalty & Rewards Platform**: Multi-program points engine with earning rules, minimum redemption thresholds, expiration rules, and manual adjustments.
- **Referral Engine**: Automated code generation (`REF-XXXX-XXXXXX`), referral conversion tracking, and advocate reward disbursement.
- **Customer Satisfaction (CSAT)**: Feedback submission, rating distributions, and negative feedback alert triggers (rating <= 2).
- **Customer Success Tasks**: Automated creation of retention tasks for high-risk accounts, low CSAT alerts, and VIP milestones.
- **Automated Customer Segmentation**: Cohort calculation (`VIP`, `High Spend`, `New`, `Repeat`, `Inactive`, `At-Risk`).

### 🎨 Frontend Architecture (Next.js 16 + React 19 + React Query)
- **Customer Success Center** (`/dashboard/customer-success`): Executive overview dashboard with retention rates, at-risk metrics, and action recommendations.
- **Customer 360° Profile** (`/dashboard/customers/[id]`): Unified account view consolidating personal details, financial stats (LTV, orders, balance), open deals, marketing activity, loyalty balances, CSAT, and activity timeline.
- **Customer Health Dashboard** (`/dashboard/customer-health`): Risk level category filters, customer search, and score recalculations.
- **Loyalty Dashboard** (`/dashboard/loyalty`): Program builder modal, point adjustment tool, reward redemption tool, and transaction logs.
- **Referral Dashboard** (`/dashboard/referrals`): Code generator, status conversion dialog, top advocates leaderboard, and conversion funnel charts.
- **Customer Feedback & CSAT** (`/dashboard/customer-feedback`): Average CSAT gauge, rating distribution chart, negative feedback alert box, and feedback submission dialog.
- **Customer Segments** (`/dashboard/customer-segments`): Cohort filters, segment search, CSV exporter, and recalculation triggers.
- **Customer Success Tasks** (`/dashboard/customer-success/tasks`): Status tabs, priority filters, task creation modal, and auto-generate task runner.

---

## 🛠️ Tech Stack Overview

### Backend
- **Framework**: ASP.NET Core (.NET 10 Web API)
- **Architecture**: Clean Architecture, CQRS, Repository Pattern
- **Database**: PostgreSQL with Entity Framework Core 9
- **Security**: JWT Authentication, Multi-tenant Isolation, AES-256 Credential Encryption
- **Communication**: SignalR for real-time notifications & logs, HttpClient for Webhooks & SaaS APIs
- **Libraries**: FluentValidation, AutoMapper

### Frontend
- **Framework**: Next.js 16 (App Router), React 19, TypeScript
- **Visual Engine**: Recharts, `@xyflow/react` (React Flow)
- **Styling**: Tailwind CSS v4, Lucide Icons, Shadcn UI patterns
- **State & Data**: React Query (`@tanstack/react-query`), Axios, React Hook Form, Zod
- **Notifications**: Sonner

---

## 📚 Documentation & Reference Files

Detailed technical documents are available in the [`docs/`](backend/docs/) directory:

- [**docs/DOCUMENT_MANAGEMENT_SYSTEM.md**](docs/DOCUMENT_MANAGEMENT_SYSTEM.md) — Document Management System (DMS) Backend Architecture & APIs
- [**docs/DOCUMENT_MANAGEMENT_FRONTEND.md**](docs/DOCUMENT_MANAGEMENT_FRONTEND.md) — Document Center & Workspaces Frontend Guide
- [**docs/customer-success.md**](backend/docs/customer-success.md) — Customer Success Platform Architecture
- [**docs/customer-health.md**](backend/docs/customer-health.md) — Customer Health Score Engine
- [**docs/loyalty-program.md**](backend/docs/loyalty-program.md) — Loyalty & Rewards Engine
- [**docs/referrals.md**](backend/docs/referrals.md) — Referral System & Funnel
- [**docs/customer-satisfaction.md**](backend/docs/customer-satisfaction.md) — Customer Satisfaction (CSAT) Architecture
- [**frontend/docs/customer-success-ui.md**](frontend/docs/customer-success-ui.md) — Customer Success Center UI
- [**frontend/docs/customer-health-ui.md**](frontend/docs/customer-health-ui.md) — Customer Health Dashboard UI
- [**frontend/docs/loyalty-ui.md**](frontend/docs/loyalty-ui.md) — Loyalty & Rewards UI
- [**frontend/docs/referrals-ui.md**](frontend/docs/referrals-ui.md) — Referral System UI
- [**frontend/docs/customer-feedback-ui.md**](frontend/docs/customer-feedback-ui.md) — Customer Feedback UI

---

## 🚀 Running Locally

### 1. Backend Server (.NET 10 API)
```bash
dotnet run --project backend/backend.csproj
# API will start on http://localhost:5041
```

### 2. Frontend Development Server (Next.js 16)
```bash
cd frontend
npm install
npm run dev
# App will start on http://localhost:3000
```