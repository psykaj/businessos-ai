# Trigger Engine Documentation

The Trigger Engine detects system events across BusinessOS AI modules and dispatches active workflows.

## Supported Triggers

1. `LeadCreated` - Fired when a new lead is created in CRM.
2. `LeadUpdated` - Fired when lead properties are updated.
3. `LeadQualified` - Fired when lead score crosses qualification threshold.
4. `QRCodeScanned` - Fired when a dynamic QR code is scanned.
5. `CustomerRegistered` - Fired on customer registration.
6. `InvoicePaid` - Fired when an invoice payment succeeds.
7. `PaymentFailed` - Fired when a payment attempt fails.
8. `SubscriptionExpiring` - Fired near subscription expiry.
9. `FormSubmitted` - Fired when a public form is submitted.
10. `CampaignCompleted` - Fired when a marketing campaign finishes.
11. `TeamMemberAdded` - Fired when a team member joins the org.
12. `ScheduledTrigger` - Fired on time/cron schedule.
13. `ManualTrigger` - Triggered manually via API or dashboard button.

## Dispatcher API

`POST /api/workflows/triggers/test-dispatch?triggerType=LeadCreated`
Dispatch a trigger payload across all matching active workflows for the tenant.
