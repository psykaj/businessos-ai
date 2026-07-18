# Dashboard Navigation & Layout Revamp

Based on your request for a highly attractive, polished, and professional layout that makes the new Billing pages easily accessible, I propose a significant UI enhancement to the main Dashboard Shell.

## Proposed Changes

### 1. Sidebar Navigation Restructuring (`sidebar.tsx`)
Currently, the sidebar is a flat list of 9 items. To make it feel like a true enterprise SaaS application, I will group the navigation items into logical, categorized sections with section headers:

**Overview**
- Dashboard (`/dashboard`)
- Analytics (`/dashboard/analytics`)
- Customers (`/dashboard/customers`)

**Apps & Tools**
- QR Codes (`/dashboard/qr`)
- Reviews (`/dashboard/reviews`)
- WhatsApp (`/dashboard/whatsapp`)
- AI Assistant (`/dashboard/ai-assistant`)

**Billing & Plans (New Section)**
- Billing Dashboard (`/dashboard/billing`)
- Subscriptions (`/dashboard/subscription`)
- Invoices (`/dashboard/invoices`)
- Payments (`/dashboard/payments`)
- Pricing & Upgrade (`/pricing`)

### 2. Visual Polish & "Upgrade" Banner
- **Pro Banner**: I will add a visually striking, glassmorphic "Upgrade to Pro" or "Plan Status" mini-card at the bottom of the sidebar (just above the user profile). This is a common pattern in top-tier SaaS apps (like Vercel/Stripe) to keep pricing and upgrades highly visible.
- **Active States**: I will enhance the active states of the sidebar links using subtle gradients and bold typography for a premium feel.

### 3. Header Enhancements (`header.tsx`)
- Implement dynamic breadcrumbs or a clean Page Title in the header so users always know exactly which billing page they are on.
- Ensure the mobile toggle is perfectly polished for responsive viewing across all devices.

## Verification Plan
- Build the frontend locally and verify that the layout degrades gracefully on mobile views.
- Test routing across all newly added billing links directly from the redesigned sidebar.
