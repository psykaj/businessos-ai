# Action Engine Documentation

The Action Engine executes sequential steps in a workflow with retry handling and output context passing.

## Supported Actions

1. `CreateCrmLead`: Creates a CRM lead.
2. `UpdateCrm`: Updates lead/deal properties.
3. `AssignSalesperson`: Assigns a sales agent to a deal.
4. `SendEmail`: Sends transactional or notification emails with variable template resolution.
5. `SendWhatsApp`: Sends WhatsApp business messages.
6. `SendNotification`: Sends real-time in-app notifications.
7. `GenerateInvoice`: Generates an invoice draft or final PDF.
8. `GenerateQR`: Generates dynamic QR code.
9. `CallAiAssistant`: Executes AI prompt to analyze context or generate copy.
10. `CallWebhook`: Outbound HTTP POST to external API.
11. `DelayExecution`: Pauses workflow execution for specified duration.
12. `UpdateCustomerJourney`: Advances customer journey stage.
13. `LogActivity`: Logs activity to audit log.
