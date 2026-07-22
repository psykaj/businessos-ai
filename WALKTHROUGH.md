# Simplify - Comprehensive Walkthrough

Welcome to the Simplify platform. This walkthrough explains the project structure, the tech stack, and the key components that power the application.

## Tech Stack Overview
- **Frontend**: Next.js 16 (App Router), React, Tailwind CSS, shadcn/ui, Recharts.
- **Backend**: .NET 8 Web API, C#, Entity Framework Core, PostgreSQL/SQL Server (Configurable).
- **Authentication**: JWT tokens with role-based access control (RBAC).
- **Payments**: Razorpay.

---

## Directory Structure

### `frontend/`
The frontend is a modern Next.js application using the App Router (`app/`).
- `app/`: Contains all route definitions.
  - `(auth)/`: Login and registration pages.
  - `dashboard/`: The main authenticated application interface, containing sub-folders for `analytics`, `api-keys`, `billing`, `organization`, `qr`, `roles`, `team`, etc.
  - `r/[shortCode]/`: The dynamic route for resolving and tracking QR code scans.
- `components/`: Reusable React components.
  - `ui/`: Base UI components from shadcn/ui (Buttons, Inputs, Dialogs, Data Tables).
  - `analytics/`, `billing/`, `qr/`: Domain-specific components.
- `hooks/`: Custom React hooks for API data fetching (using SWR or React Query) and state management.
- `lib/`: Utility functions and API service wrappers (e.g., `analytics-service.ts`, `apikeys-service.ts`).
- `types/`: TypeScript interfaces for the data models.

### `backend/`
The backend is a .NET 8 Web API structured using modular architecture.
- `Controllers/`: API entry points for standard operations.
- `Modules/`: Domain-driven folders (e.g., `Analytics`, `ApiKeys`, `Billing`, `QRCode`, `Team`) containing controllers, models, and specialized logic for that domain.
- `Entities/`: The core database entities (e.g., `User`, `Organization`, `QRCode`, `ApiKey`, `Role`, `Permission`).
- `Persistence/`: Entity Framework `ApplicationDbContext` and database configurations.
- `Migrations/`: Auto-generated EF Core migrations for schema changes.
- `Services/`: Business logic layer (e.g., `AuthService`).
- `Middleware/`: Custom ASP.NET Core middleware, such as the `PermissionAuthorizationHandler` for enforcing RBAC.
- `DTOs/`: Data Transfer Objects used to shape JSON request/responses.

---

## Key Components

### 1. Authentication & RBAC
Every API request is secured via JWT. The backend uses a custom `RequirePermission` attribute to enforce granular permissions (e.g., `[RequirePermission("view_api_keys")]`). The `PermissionAuthorizationHandler` dynamically resolves the user's role and checks their permissions against the database.

### 2. Multi-tenant Data Model
The system uses `OrganizationId` as the primary tenant identifier. Users belong to an organization, and all resources (QR Codes, API Keys, Team Members, Analytics) are scoped to this `OrganizationId`.

### 3. QR Code Tracking Engine
When a user scans a QR code, they hit the `frontend/app/r/[shortCode]/page.tsx` route. This route captures metadata (browser, device type, country based on IP) and sends a scan event to the backend before redirecting the user to the original destination URL.

### 4. Analytics Aggregation
The `AnalyticsController` in the backend runs complex LINQ queries to aggregate scan data over time. The frontend consumes this data and visualizes it using `Recharts` inside the `analytics-charts.tsx` component, providing real-time insights to the business owner.
