# Customer Journey Module

The Customer Journey module provides an event-sourced timeline of a contact's lifecycle.

## Stages
- **Visitor**: Unknown traffic.
- **Lead**: Captured via form, missing qualification.
- **Qualified Lead**: Scored or manually vetted.
- **Customer**: Converted via deal won/payment.
- **Repeat Customer**: Multiple successful transactions.
- **Loyal Customer**: High LTV.

## Functionality
- `TransitionStageAsync`: Safely moves a lead between stages. Records the timestamp and previous state.
- Emits `CustomerStageChanged` webhooks to trigger follow-up campaigns.
