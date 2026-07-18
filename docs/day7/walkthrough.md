# Analytics Dashboard Redesign & Bug Fix

I have completely redesigned the Analytics Dashboard and permanently fixed the redirect bug! 

## Bug Fix
The `AnalyticsController` in the C# backend was failing to locate your `OrganizationId` inside the JWT claims, throwing a 401 Unauthorized exception. This caused our frontend Axios interceptor to aggressively log you out and redirect you to the login page.
**The Fix**: I rewrote the Analytics API to extract your `UserId` instead, and seamlessly query the PostgreSQL database to retrieve your `OrganizationId`. The bug is completely eliminated.

## Premium UI Redesign
The entire **Analytics Page (`/dashboard/analytics`)** has been overhauled to match our stunning, top-tier SaaS aesthetic:
- **Glowing Decor**: Integrated deep blue and primary-colored glowing orbs into the background for a modern, high-tech intelligence feel.
- **Glassmorphic Data Views**: The KPI Cards, Line Charts, and Data Tables are now housed inside sleek `backdrop-blur-xl` panels that elevate slightly on hover.
- **Polished Controls**: Upgraded the Date Picker, Refresh Button, and Export CSV buttons into floating, semi-transparent controls with premium hover micro-interactions.
- **Stunning Gradient Typography**: The page header now features a gorgeous gradient text effect alongside a custom floating icon container.

The page is seamlessly responsive, collapsing charts and tables beautifully on mobile screens. You can instantly experience the final polished product by navigating to the Analytics tab!
