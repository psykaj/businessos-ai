# Simplify

> **AI-powered Business Operating System for SMEs.**  
> Help 1 Million Small & Medium Businesses grow online using AI — one unified platform instead of 10 disparate software tools.

---

## 🎯 Vision

Simplify is an all-in-one AI platform built for SMEs to manage sales, customer relations, automated marketing, cross-application workflows, and executive analytics seamlessly.

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
| 👑 **Executive Dashboard & BI** | CEO command center, automatic 14 KPIs, AI decisions & forecasts | ✅ Completed |
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

---

## ⚡ Day 14 Highlights: Executive Dashboard, BI & AI Decision Engine

This module builds a CEO-grade command center that consolidates data across CRM, Marketing, Workflows, Billing, and Team performance into real-time actionable intelligence.

### ⚙️ Backend Architecture (ASP.NET Core .NET 9)
- **Automatic KPI Calculation Engine**: Automatically calculates 14 enterprise KPIs (Total Revenue, Monthly Revenue, Retention Rate, Churn Rate, Lead Conversion %, Avg Deal Size, Sales Cycle, Campaign ROI, Active Workflows, AI Usage, etc.).
- **Executive Dashboard API Layer**: Role-tailored endpoints (`/ceo`, `/sales`, `/marketing`, `/finance`, `/operations`, `/team`) with period filters (`7d`, `30d`, `90d`, `1y`, `custom`) and comparison calculations.
- **AI Business Insights Engine**: Heuristic decision engine surfacing prioritized risk alerts (`Critical`, `High`, `Medium`, `Low`) with business impact and recommended actions.
- **Predictive Forecast Engine**: Statistical projection models for Revenue, Sales, Leads, Customer Growth, and Subscriptions with confidence scoring.
- **Report Generator & Multi-Format Exporter**: On-demand report creation with native streaming exports for PDF, Excel (XML/TSV), and CSV.
- **Goal Tracking Framework**: Goal creation, progress calculation against live KPIs, and status progression (`InProgress`, `Achieved`, `Behind`, `Failed`).

### 🎨 Frontend Architecture (Next.js 16 + Recharts + React Query)
- **Executive Command Center** (`/dashboard/executive`): Multi-role dashboard switcher with Recharts revenue velocity curves and customizable layout grid.
- **KPI Performance Center** (`/dashboard/kpis`): Interactive KPI cards across Daily, Weekly, Monthly, Quarterly, and Yearly granularity.
- **AI Decision Center** (`/dashboard/insights`): Surfacing AI recommendations with priority badges, business impact analysis, and resolution triggers.
- **Forecast Dashboard** (`/dashboard/forecast`): Growth trajectory curves over 30, 60, or 90 day horizons with model confidence indicators.
- **Reports Center & Export Hub** (`/dashboard/reports` & `/dashboard/export-center`): Interactive preview modals and instant multi-format downloads.
- **Goal Management** (`/dashboard/goals`): Visual goal progress bars and live KPI syncing.

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
- **Visual Engine**: Recharts, `@xyflow/react` (React Flow)
- **Styling**: Tailwind CSS v4, Lucide Icons, Shadcn UI patterns
- **State & Data**: React Query (`@tanstack/react-query`), Axios, React Hook Form, Zod
- **Notifications**: Sonner

---

## 📚 Documentation & Reference Files

Detailed technical documents are available in the [`docs/`](docs/) directory:

- [**docs/business-intelligence.md**](docs/business-intelligence.md) — BI Backend Engine Architecture
- [**docs/executive-dashboard.md**](docs/executive-dashboard.md) — Executive Dashboard REST APIs
- [**docs/forecasting.md**](docs/forecasting.md) — Predictive Forecast Engine
- [**docs/reports.md**](docs/reports.md) — Report Generator & Multi-Format Exporter
- [**docs/goals.md**](docs/goals.md) — Goal Tracking Architecture
- [**docs/executive-dashboard-ui.md**](docs/executive-dashboard-ui.md) — Executive Dashboard Frontend Architecture
- [**docs/kpi-center.md**](docs/kpi-center.md) — KPI Center UI
- [**docs/ai-insights-ui.md**](docs/ai-insights-ui.md) — AI Decision Center UI
- [**docs/reports-ui.md**](docs/reports-ui.md) — Reports Center UI
- [**docs/goal-tracking-ui.md**](docs/goal-tracking-ui.md) — Goal Tracking UI

---

## 🚀 Running Locally

### 1. Backend Server (.NET 9 API)
```bash
dotnet run --project backend/backend.csproj
# API will start on http://localhost:5294
```

### 2. Frontend Development Server (Next.js 16)
```bash
cd frontend
npm install
npm run dev
# App will start on http://localhost:3000
```