# Integration Platform Documentation

The Integration Platform connects BusinessOS AI with external third-party SaaS services.

## Encryption & Security
All sensitive credentials (`ApiKey`, `AccessToken`, `RefreshToken`) are encrypted at rest using AES-256 (`IEncryptionService`). When returning Integration DTOs, keys are automatically masked (`****ABCD`).

## Supported Integration Providers

1. `GoogleSheets`
2. `Slack`
3. `MicrosoftTeams`
4. `Discord`
5. `GoogleCalendar`
6. `OutlookCalendar`
7. `Resend`
8. `Twilio`
9. `Stripe`
10. `Razorpay`
11. `Webhook`
12. `RestApi`

## APIs

- `GET /api/integrations`: List active integrations for tenant.
- `POST /api/integrations`: Connect a new provider integration.
- `PUT /api/integrations/{id}`: Update integration details or credentials.
- `DELETE /api/integrations/{id}`: Disconnect integration.
- `POST /api/integrations/{id}/test`: Run automated connection check.
