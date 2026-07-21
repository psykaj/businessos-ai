# Lead Capture Module

The Lead Capture engine acts as the central router for converting incoming signals (Form submissions, API integrations, Webhooks) into CRM entities.

## Workflow
1. **Ingestion**: Receives structured data from forms or APIs.
2. **Parsing**: Dynamically parses JSON data to extract key fields (Name, Email, Phone, Company).
3. **CRM Lead Creation**: Instantiates a new CRM `Lead`.
4. **Journey Initiation**: Adds the lead to the Customer Journey tracker at the "Lead" stage.
5. **Event Emission**: Dispatches internal events (via `IWebhookDispatchService`) for AI automation and external system synchronization.
