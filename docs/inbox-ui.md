# Unified Inbox UI & Triage Workflow

## Overview
The Unified Inbox (`/dashboard/inbox`) empowers customer service teams to triage multichannel customer inquiries in real-time. Designed with inspiration from Intercom and Zendesk, it prioritizes speed, clarity, and bulk operations.

## Key Capabilities & Business Value
- **Real-Time SignalR Queuing**: Incoming customer messages instantly update unread badges and position in the queue without page reloads, accelerating customer response and boosting conversion revenue.
- **SLA Breach Warnings**: Automated visual badges clearly highlight tickets exceeding target response windows (&lt;10m for high LTV accounts).
- **Bulk Assignment Modal**: Allows supervisors to rapidly redistribute ticket queues to specific account executives or support engineers in a single click.

## Components
- `InboxHeader`: Contains multi-channel filter tabs, priority/status dropdowns, search bar, and KPI indicators.
- `ConversationList` & `ConversationItem`: Virtualized cards featuring unread indicator animations, timestamps, and customer company metrics.
