# Integration Center UI

The Integration Center allows SME users to easily manage their third-party connections (e.g., Slack, HubSpot, Stripe). 

## Features
- **Dashboard Overview**: Visually appealing dashboard showing active integrations.
- **Health Indicators**: Real-time sync status (Healthy/Failing) with `Activity` indicators.
- **Actions**: Users can easily test sync connections or unplug/disconnect applications.
- **Marketplace Discovery**: When empty, clear call-to-actions navigate users to the Connector Marketplace.

## Design Highlights
- Minimalist card-based layout using Shadcn UI.
- Secure, "Enterprise" feel with encryption badges (AES-256).
- Accessible action menus mapped to backend `useIntegrations` React Query hooks.
