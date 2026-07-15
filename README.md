# BusinessOS AI

> **AI-powered Business Operating System for SMEs.**
> Help 1 Million Indian Businesses grow online using AI — one platform instead of 10 different tools.

---

## 🎯 Vision

BusinessOS AI is an all-in-one AI platform built for Indian small and medium businesses.

Instead of juggling 10 different software products, businesses use **one platform** to manage everything:

| Module | Description |
|--------|-------------|
| 🔲 QR Code Management | Create, track and manage dynamic QR codes |
| ⭐ AI Review Reply | Auto-reply to Google/platform reviews using AI |
| 🧾 Invoice Generator | Create and send professional invoices |
| 💬 WhatsApp Marketing | Run WhatsApp campaigns at scale |
| 👥 CRM | Manage customer relationships |
| 📅 Appointment Booking | Online booking system |
| 🌐 Landing Pages | Build landing pages instantly |
| 🤖 AI Chatbot | 24/7 AI assistant for customers |
| 📊 Analytics | Business insights and reporting |
| 💼 Digital Business Card | NFC + QR-based digital cards |

**Long-term goal:** Become the Shopify + Zoho + Canva + QR Tiger for SMEs.

---

## 🗺️ Development Roadmap

| Day | Task | Status | Details |
|-----|------|--------|---------|
| **Day 1** | Project Initialization & Setup | ✅ **Done** | [See below ↓](#day-1--project-initialization) |
| **Day 2** | Dashboard UI | ✅ **Done** | [See below ↓](#day-2--dashboard-ui) |
| **Day 3** | Authentication Module | ✅ **Done** | [See below ↓](#day-3--authentication-module) |
| **Day 4** | QR Code Module | 🔜 **Next** | Dynamic QR creation + tracking |
| **Day 5** | Reviews Module | ⏳ Pending | AI-powered review replies |
| **Day 6** | Invoices Module | ⏳ Pending | PDF invoice generation |
| **Day 7** | CRM / Customers | ⏳ Pending | Customer management |
| **Day 8** | Analytics | ⏳ Pending | Charts and reporting |
| **Day 9** | WhatsApp Integration | ⏳ Pending | Campaign management |
| **Day 10** | AI Assistant | ⏳ Pending | OpenAI chatbot integration |

---

## Day 1 — Project Initialization

**Date:** Day 1 | **Status:** ✅ Complete

### What was done
- Initialized the **monorepo** project structure with separate `frontend/` and `backend/` directories
- Set up **Next.js 16** with TypeScript, Tailwind CSS v4, App Router
- Set up **ASP.NET Core** Web API project with .NET 10
- Established the **folder architecture** for both frontend and backend
- Created project documentation (`docs/vision.md`, `docs/tech-stack.md`, `docs/roadmap.md`)
- Configured `.gitignore` and initialized **Git** repository

### Folder Structure Created

```
BusinessOS-AI/
├── frontend/               ← Next.js 16 app
│   ├── app/                ← App Router pages
│   ├── components/         ← Reusable UI components
│   ├── hooks/              ← Custom React hooks
│   ├── lib/                ← Utility functions
│   ├── types/              ← TypeScript type definitions
│   └── public/             ← Static assets
│
├── backend/                ← ASP.NET Core Web API
│   ├── Authentication/     ← JWT logic
│   ├── Configurations/     ← Settings POCOs
│   ├── Controllers/        ← API controllers
│   ├── Data/               ← DbContext + Migrations
│   ├── DTOs/               ← Data Transfer Objects
│   ├── Entities/           ← Domain models
│   ├── Helpers/            ← Utility classes
│   ├── Interfaces/         ← Service abstractions
│   ├── Middleware/         ← Custom middleware
│   ├── Repositories/       ← Data access layer
│   ├── Services/           ← Business logic
│   └── Validators/         ← Request validators
│
├── database/               ← DB scripts (future)
├── docs/                   ← Project documentation
└── scripts/                ← Dev/build scripts
```

### Tech Stack Chosen

| Layer | Technology | Version |
|-------|-----------|---------|
| Frontend | Next.js | 16.2.10 |
| UI Library | React | 19.2.4 |
| Language | TypeScript | 5.x |
| Styling | Tailwind CSS | 4.x |
| Component Library | shadcn/ui | 4.x |
| Backend | ASP.NET Core | .NET 10 |
| Database | PostgreSQL | - |
| ORM | Entity Framework Core | 9.x |
| AI | OpenAI + Gemini | - |
| Payments | Razorpay + Stripe | - |
| Storage | Cloudinary | - |
| Hosting (Frontend) | Vercel | - |
| Hosting (Backend) | Railway | - |
| Database (Cloud) | Neon PostgreSQL | - |

---

## Day 2 — Dashboard UI

**Date:** Day 2 | **Status:** ✅ Complete

### What was done
- Built the complete **Dashboard layout** with sidebar + header + main content area
- Implemented a **responsive sidebar** with all 9 navigation items
- Built a **sticky header** with search, notifications, theme toggle, and avatar
- Created **6 KPI metric cards** with trend indicators (up/down)
- Implemented **dark mode** with system preference detection and localStorage persistence
- Set up **global theme provider** using React Context
- Added placeholder sections for Activity Feed and Revenue Overview widgets
- Created **TypeScript types** for navigation items and metric card data
- Applied **smooth theme transitions** (0.2s) across the entire UI

### Files Created — Day 2

| File | Purpose |
|------|---------|
| [app/dashboard/page.tsx](frontend/app/dashboard/page.tsx) | Main dashboard page with metrics grid |
| [app/dashboard/layout.tsx](frontend/app/dashboard/layout.tsx) | Dashboard layout wrapper |
| [app/globals.css](frontend/app/globals.css) | Global styles, CSS variables, dark mode tokens |
| [app/layout.tsx](frontend/app/layout.tsx) | Root layout with font + theme provider |
| [components/layout/dashboard-shell.tsx](frontend/components/layout/dashboard-shell.tsx) | Main shell — sidebar + header + content area |
| [components/layout/sidebar.tsx](frontend/components/layout/sidebar.tsx) | Responsive sidebar with 9 nav items |
| [components/layout/header.tsx](frontend/components/layout/header.tsx) | Sticky header — search, bell, theme toggle, avatar |
| [components/layout/theme-toggle.tsx](frontend/components/layout/theme-toggle.tsx) | Light/dark/system theme switcher |
| [components/dashboard/metric-card.tsx](frontend/components/dashboard/metric-card.tsx) | Individual KPI card with trend arrow |
| [components/dashboard/metrics-grid.tsx](frontend/components/dashboard/metrics-grid.tsx) | 3-column responsive grid of 6 metric cards |
| [hooks/use-theme.tsx](frontend/hooks/use-theme.tsx) | ThemeProvider + useTheme hook |
| [types/navigation.ts](frontend/types/navigation.ts) | NavItem and MetricCardData TypeScript interfaces |
| [lib/utils.ts](frontend/lib/utils.ts) | `cn()` utility (clsx + tailwind-merge) |

### Dashboard Pages Stubbed (Placeholder)

| Route | Page |
|-------|------|
| `/dashboard` | Main overview with metrics |
| `/dashboard/qr` | QR Codes (coming Day 4) |
| `/dashboard/reviews` | Reviews (coming Day 5) |
| `/dashboard/invoices` | Invoices (coming Day 6) |
| `/dashboard/customers` | Customers (coming Day 7) |
| `/dashboard/analytics` | Analytics (coming Day 8) |
| `/dashboard/whatsapp` | WhatsApp (coming Day 9) |
| `/dashboard/ai-assistant` | AI Assistant (coming Day 10) |
| `/dashboard/settings` | Settings |

### UI Design System

- **Typography:** Inter (Google Fonts) via `next/font`
- **Colors:** OKLCH-based CSS variables for both light and dark modes
- **Radius:** Configurable via `--radius` CSS variable (0.625rem base)
- **Transitions:** 0.15–0.2s smooth transitions on theme changes
- **Responsive:** Mobile-first, sidebar collapses on mobile with overlay

### npm Packages Installed — Day 2

| Package | Purpose |
|---------|---------|
| `lucide-react` | Icon library |
| `clsx` | Conditional classnames |
| `tailwind-merge` | Merge Tailwind classes safely |
| `class-variance-authority` | Component variant management |
| `shadcn` | Component library CLI |
| `@base-ui/react` | Accessible UI primitives |
| `@phosphor-icons/react` | Additional icon set |
| `tw-animate-css` | Tailwind animation utilities |

---

## Day 3 — Authentication Module

**Date:** Day 3 | **Status:** ✅ Complete

### What was done
- Built a complete **production-ready authentication system** end-to-end
- Implemented **JWT authentication** with HMAC-SHA256 signing
- **BCrypt** password hashing — passwords are never stored in plain text
- **4 REST API endpoints**: register, login, me, logout
- **FluentValidation** on all request inputs (backend)
- **Global exception middleware** — no raw error messages ever reach the client
- **EF Core + PostgreSQL** — `Users` table with unique email index, auto UTC timestamps
- **Auth Context/Provider** with session restore on refresh + 60s expiry watchdog
- **Next.js 16 Proxy** (renamed from middleware) for server-side route protection
- **3 Auth Pages**: `/login`, `/register`, `/forgot-password`
- **Protected Routes**: all `/dashboard/**` routes require authentication

### Authentication Flow

```
User visits /dashboard (unauthenticated)
         ↓
proxy.ts checks businessos_token cookie
         ↓ No cookie
Redirect → /login?from=/dashboard
         ↓
User fills Login form (client-side validation)
         ↓
POST /api/auth/login
         ↓
FluentValidation validates request
         ↓
AuthService: find user by email → BCrypt.Verify password
         ↓
JwtTokenGenerator: HMAC-SHA256 signed JWT
  claims: sub (userId), email, name, role, jti, iat
         ↓
AuthResponse { token, expiresAt, user }
         ↓
Frontend AuthProvider.login()
  ├─ localStorage: businessos_token + businessos_user
  └─ Cookie: businessos_token (for proxy.ts server reads)
         ↓
router.push('/dashboard')
         ↓
ProtectedRoute: isAuthenticated = true → renders dashboard ✅
         ↓
Axios interceptor: attaches Bearer token on every API call
         ↓
Session Persistence: reads localStorage on page refresh
60s Watchdog: auto-logout if token expires
```

### Files Created — Day 3

**Backend (15 new files, 2 modified)**

| File | Action | Purpose |
|------|--------|---------|
| [Entities/User.cs](backend/Entities/User.cs) | NEW | User domain entity |
| [Configurations/JwtSettings.cs](backend/Configurations/JwtSettings.cs) | NEW | Strongly-typed JWT settings POCO |
| [Data/ApplicationDbContext.cs](backend/Data/ApplicationDbContext.cs) | NEW | EF Core DbContext — unique email index, auto timestamps |
| [Data/Migrations/](backend/Data/Migrations/) | NEW | `InitialCreate` — Users table migration |
| [DTOs/Auth/RegisterRequest.cs](backend/DTOs/Auth/RegisterRequest.cs) | NEW | Registration input record |
| [DTOs/Auth/LoginRequest.cs](backend/DTOs/Auth/LoginRequest.cs) | NEW | Login input record |
| [DTOs/Auth/UserDto.cs](backend/DTOs/Auth/UserDto.cs) | NEW | Safe user projection (no password hash) |
| [DTOs/Auth/AuthResponse.cs](backend/DTOs/Auth/AuthResponse.cs) | NEW | JWT + user response envelope |
| [Validators/RegisterRequestValidator.cs](backend/Validators/RegisterRequestValidator.cs) | NEW | FluentValidation: name, email, password strength, match |
| [Validators/LoginRequestValidator.cs](backend/Validators/LoginRequestValidator.cs) | NEW | FluentValidation: email + password presence |
| [Interfaces/IAuthService.cs](backend/Interfaces/IAuthService.cs) | NEW | Auth service abstraction |
| [Interfaces/IJwtTokenGenerator.cs](backend/Interfaces/IJwtTokenGenerator.cs) | NEW | JWT generator abstraction |
| [Authentication/JwtTokenGenerator.cs](backend/Authentication/JwtTokenGenerator.cs) | NEW | HMAC-SHA256 JWT with all required claims |
| [Services/AuthService.cs](backend/Services/AuthService.cs) | NEW | Register (BCrypt hash) + Login (BCrypt verify) |
| [Controllers/AuthController.cs](backend/Controllers/AuthController.cs) | NEW | 4 REST endpoints + ApiResponse<T> envelope |
| [Middleware/ExceptionHandlingMiddleware.cs](backend/Middleware/ExceptionHandlingMiddleware.cs) | NEW | Global exception → structured JSON, no stack traces |
| [appsettings.json](backend/appsettings.json) | MODIFIED | Added ConnectionStrings + JwtSettings |
| [Program.cs](backend/Program.cs) | MODIFIED | Full DI wiring: JWT, CORS, EF Core, validators, auto-migrate |

**Frontend (14 new files, 3 modified)**

| File | Action | Purpose |
|------|--------|---------|
| [types/auth.ts](frontend/types/auth.ts) | NEW | User, AuthResponse, LoginRequest, RegisterRequest types |
| [lib/api-client.ts](frontend/lib/api-client.ts) | NEW | Axios instance — JWT interceptor + 401 auto-logout |
| [lib/auth-service.ts](frontend/lib/auth-service.ts) | NEW | All API calls + localStorage + cookie sync |
| [contexts/auth-context.tsx](frontend/contexts/auth-context.tsx) | NEW | AuthContext + AuthProvider + session restore + expiry watchdog |
| [proxy.ts](frontend/proxy.ts) | NEW | Next.js 16 server-side route protection |
| [components/auth/protected-route.tsx](frontend/components/auth/protected-route.tsx) | NEW | Client-side guard with loading spinner |
| [components/auth/auth-form-wrapper.tsx](frontend/components/auth/auth-form-wrapper.tsx) | NEW | Reusable auth card: brand, title, card shell, footer link |
| [components/auth/form-field.tsx](frontend/components/auth/form-field.tsx) | NEW | Labeled input + error display |
| [app/(auth)/layout.tsx](frontend/app/(auth)/layout.tsx) | NEW | Centered auth layout with gradient blobs |
| [app/(auth)/login/page.tsx](frontend/app/(auth)/login/page.tsx) | NEW | Login form — validation, show/hide password, friendly errors |
| [app/(auth)/register/page.tsx](frontend/app/(auth)/register/page.tsx) | NEW | Register form — 4 fields, password strength rules |
| [app/(auth)/forgot-password/page.tsx](frontend/app/(auth)/forgot-password/page.tsx) | NEW | Forgot password UI — simulated flow + success state |
| [.env.local](frontend/.env.local) | NEW | `NEXT_PUBLIC_API_URL=http://localhost:5041` |
| [app/layout.tsx](frontend/app/layout.tsx) | MODIFIED | Added AuthProvider wrapping ThemeProvider |
| [app/dashboard/layout.tsx](frontend/app/dashboard/layout.tsx) | MODIFIED | Wrapped with ProtectedRoute |
| [components/layout/sidebar.tsx](frontend/components/layout/sidebar.tsx) | MODIFIED | Live user info from useAuth() + logout button |

### API Endpoints — Day 3

| Method | Endpoint | Auth Required | Description |
|--------|----------|:---:|-------------|
| `POST` | `/api/auth/register` | ❌ | Register new account → returns JWT |
| `POST` | `/api/auth/login` | ❌ | Authenticate → returns JWT |
| `GET` | `/api/auth/me` | ✅ Bearer | Get current user profile |
| `POST` | `/api/auth/logout` | ✅ Bearer | Sign out (token cleared client-side) |

### NuGet Packages Installed — Day 3

| Package | Version | Purpose |
|---------|---------|---------|
| `Microsoft.AspNetCore.Authentication.JwtBearer` | 9.0.6 | JWT Bearer auth middleware |
| `Microsoft.EntityFrameworkCore` | 9.0.6 | ORM base |
| `Microsoft.EntityFrameworkCore.Design` | 9.0.6 | EF CLI migrations tooling |
| `Npgsql.EntityFrameworkCore.PostgreSQL` | 9.0.4 | PostgreSQL EF provider |
| `BCrypt.Net-Next` | 4.0.3 | Secure password hashing |
| `AutoMapper.Extensions.Microsoft.DependencyInjection` | 12.0.1 | Object mapping (ready for Day 4+) |
| `FluentValidation.AspNetCore` | 11.3.0 | Request input validation |

### npm Packages Installed — Day 3

| Package | Purpose |
|---------|---------|
| `axios` | HTTP client with interceptors |

### Protected Routes

All routes under `/dashboard/**` are protected:

```
/dashboard              → requires auth
/dashboard/qr           → requires auth
/dashboard/reviews      → requires auth
/dashboard/invoices     → requires auth
/dashboard/customers    → requires auth
/dashboard/analytics    → requires auth
/dashboard/whatsapp     → requires auth
/dashboard/ai-assistant → requires auth
/dashboard/settings     → requires auth
```

---

## 🏗️ Full Tech Stack

| Layer | Technology | Version |
|-------|-----------|---------|
| Frontend Framework | Next.js | 16.2.10 |
| UI Library | React | 19.2.4 |
| Language (Frontend) | TypeScript | 5.x |
| Styling | Tailwind CSS | 4.x |
| Component Library | shadcn/ui | 4.x |
| Backend Framework | ASP.NET Core Web API | .NET 10 |
| Language (Backend) | C# | 13 |
| Database | PostgreSQL | - |
| ORM | Entity Framework Core | 9.x |
| Auth | JWT (HMAC-SHA256) + BCrypt | - |
| Validation | FluentValidation | 11.x |
| HTTP Client | Axios | - |
| Icons | Lucide React | - |
| AI | OpenAI + Gemini | Day 10 |
| Payments | Razorpay + Stripe | Future |
| Storage | Cloudinary | Future |
| Hosting Frontend | Vercel | - |
| Hosting Backend | Railway | - |
| Database Cloud | Neon PostgreSQL | - |

---

## 🚀 Running Locally

### Prerequisites
- Node.js 20+
- .NET 10 SDK
- PostgreSQL running locally

### Start Backend
```bash
cd backend
dotnet run
# → API: http://localhost:5041
# → OpenAPI: http://localhost:5041/openapi
# → Auto-creates PostgreSQL tables on first run
```

### Start Frontend
```bash
cd frontend
npm install
npm run dev
# → App: http://localhost:3000
# → Redirects to /login if not authenticated
```

### Default Database Config
Update `backend/appsettings.json` if your PostgreSQL credentials differ:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=businessos_db;Username=postgres;Password=postgres"
  }
}
```
